// <copyright file="DaprCosmosAggregateMaintenance.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.CosmosDatabases.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors.Client;
using Dapr.Client;

using Hexalith.Application.Aggregates;
using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.CosmosDb.Configurations;
using Hexalith.Infrastructure.CosmosDb.Providers;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

/// <summary>
/// Class DaprCosmosAggregateMaintenance.
/// Implements the <see cref="CosmosDbProvider" />
/// Implements the <see cref="IAggregateMaintenance{TAggregate}" />.</summary>
/// <typeparam name="TAggregate">The type of the t aggregate.</typeparam>
/// <seealso cref="CosmosDbProvider" />
/// <seealso cref="IAggregateMaintenance{TAggregate}" />
public class DaprCosmosAggregateMaintenance<TAggregate> :
    CosmosDbProvider,
    IAggregateMaintenance<TAggregate>
    where TAggregate : IAggregate, new()
{
    private readonly DaprClient _daprClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="DaprCosmosAggregateMaintenance{TAggregate}"/> class.
    /// </summary>
    /// <param name="settings">The Cosmos DB settings.</param>
    /// <param name="daprClient">The Dapr client.</param>
    public DaprCosmosAggregateMaintenance(IOptions<CosmosDbSettings> settings, DaprClient daprClient)
        : base(settings)
    {
        ArgumentNullException.ThrowIfNull(daprClient);
        _daprClient = daprClient;
    }

    /// <inheritdoc/>
    public async Task ClearAllCommandsAsync(CancellationToken cancellationToken)
    {
        (string? applicationId, string actorType) = await GetActorNameAsync(cancellationToken).ConfigureAwait(false);
        List<string> ids = GetAllAggregateIds(applicationId, actorType);
        foreach (string id in ids)
        {
            await ClearCommandsAsync(applicationId, actorType, id, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public Task ClearAllStatesAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

    /// <inheritdoc/>
    public async Task ClearCommandsAsync(string aggregateId, CancellationToken cancellationToken)
    {
        (string? applicationId, string actorType) = await GetActorNameAsync(cancellationToken).ConfigureAwait(false);
        await ClearCommandsAsync(applicationId, actorType, aggregateId, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task ClearStateAsync(string aggregateId, CancellationToken cancellationToken) => throw new NotImplementedException();

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetAllActorIdsAsync(CancellationToken cancellationToken)
    {
        (string appId, string actorType) = await GetActorNameAsync(cancellationToken).ConfigureAwait(false);

        return GetAllAggregateIds(appId, actorType);
    }

    /// <inheritdoc/>
    public async Task SendAllSnapshotsAsync(CancellationToken cancellationToken)
    {
        (string appId, string actorType) = await GetActorNameAsync(cancellationToken).ConfigureAwait(false);
        IEnumerable<string> ids = GetAllAggregateIds(appId, actorType);
        foreach (string id in ids)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IAggregateActor actor = ActorProxy.Create<IAggregateActor>(new Dapr.Actors.ActorId(id), actorType);
            await actor.SendSnapshotEventAsync().ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public async Task SendSnapshotAsync(string aggregateId, CancellationToken cancellationToken)
    {
        (_, string actorType) = await GetActorNameAsync(cancellationToken).ConfigureAwait(false);
        IAggregateActor actor = ActorProxy.Create<IAggregateActor>(new Dapr.Actors.ActorId(aggregateId), actorType);
        cancellationToken.ThrowIfCancellationRequested();
        await actor.SendSnapshotEventAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Get actor id from the state store item id.
    /// State id format:. <appId>||<actorType>||<actorId>||<stateName>
    /// </summary>
    /// <param name="stateId">State store item id</param>
    /// <returns>Actor id</returns>
    private static string GetActorId(string stateId) => stateId.Split("||").Skip(2).FirstOrDefault() ?? string.Empty;

    private async Task ClearCommandsAsync(string applicationId, string actorType, string aggregateId, CancellationToken cancellationToken)
    {
        IAggregateActor actor = ActorProxy.Create<IAggregateActor>(new Dapr.Actors.ActorId(aggregateId), actorType);
        cancellationToken.ThrowIfCancellationRequested();
        await actor.ClearCommandsAsync().ConfigureAwait(false);
        IEnumerable<string> ids = GetCommandStreamStateIds(applicationId, actorType, aggregateId);
        foreach (string id in ids)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _ = await Container.DeleteItemAsync<DaprStateItem>(id, new PartitionKey(id), cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task<(string AppId, string ActorType)> GetActorNameAsync(CancellationToken cancellationToken)
    {
        DaprMetadata meta = await _daprClient.GetMetadataAsync(cancellationToken).ConfigureAwait(false);
        string appId = meta.Id;
        string actorType = $"{new TAggregate().AggregateName}Aggregate";
        DaprActorMetadata? actorMeta = meta.Actors.FirstOrDefault(p => p.Type == actorType);
        return actorMeta is null
            ? throw new InvalidOperationException($"Actor type {actorType} is not registered. Registered actors: {string.Join("; ", meta.Actors.Select(p => p.Type))}")
            : (appId, actorType);
    }

    private List<string> GetAllAggregateIds(string applicationId, string actorType)
    {
        IOrderedQueryable<DaprStateItem> feedIterator = Container.GetItemLinqQueryable<DaprStateItem>();
        List<string> ids = [.. feedIterator
            .Where(x => x.Id.StartsWith($"{applicationId}||{actorType}") && x.Id.EndsWith("||State"))
            .Select(p => p.Id)];
        return [.. ids
            .Select(GetActorId)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .Distinct()
            .OrderBy(p => p)];
    }

    private List<string> GetCommandStreamStateIds(string applicationId, string actorType, string aggredateId)
    {
        IOrderedQueryable<DaprStateItem> feedIterator = Container.GetItemLinqQueryable<DaprStateItem>();
        return [.. feedIterator
            .Where(x => x.Id.StartsWith($"{applicationId}||{actorType}||{aggredateId}||CommandStream"))
            .Select(p => p.Id)];
    }
}