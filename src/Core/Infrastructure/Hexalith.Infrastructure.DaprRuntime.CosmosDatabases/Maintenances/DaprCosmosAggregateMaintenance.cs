// <copyright file="DaprCosmosAggregateMaintenance.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.CosmosDatabases.Maintenances;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Client;
using Dapr.Client;

using Hexalith.Application.Aggregates;
using Hexalith.Domain.Aggregates;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.CosmosDb.Configurations;
using Hexalith.Infrastructure.CosmosDb.Providers;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.CosmosDatabases.Models;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

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
    IAggregateMaintenance<TAggregate>, IDisposable
    where TAggregate : IDomainAggregate, new()
{
    private readonly string _connectionString;
    private readonly DaprClient _daprClient;
    private readonly string _databaseName;
    private string? _containerName;
    private CosmosDbProvider? _cosmosDbProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DaprCosmosAggregateMaintenance{TAggregate}"/> class.
    /// </summary>
    /// <param name="settings">The Cosmos DB settings.</param>
    /// <param name="daprClient">The Dapr client.</param>
    public DaprCosmosAggregateMaintenance(IOptions<CosmosDbSettings> settings, DaprClient daprClient)
    {
        ArgumentNullException.ThrowIfNull(daprClient);
        ArgumentNullException.ThrowIfNull(settings);
        SettingsException<CosmosDbSettings>.ThrowIfNullOrWhiteSpace(settings.Value.ConnectionString);
        SettingsException<CosmosDbSettings>.ThrowIfNullOrWhiteSpace(settings.Value.DatabaseName);
        _connectionString = settings.Value.ConnectionString;
        _databaseName = settings.Value.DatabaseName;
        _containerName = settings.Value.ContainerName;
        _daprClient = daprClient;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="DaprCosmosAggregateMaintenance{TAggregate}"/> class.
    /// </summary>
    ~DaprCosmosAggregateMaintenance()
    {
        Dispose(false);
    }

    /// <inheritdoc/>
    public async Task ClearAllCommandsAsync(CancellationToken cancellationToken)
    {
        CosmosDbProvider cosmos = await GetCosmosProviderAsync(cancellationToken).ConfigureAwait(false);
        (string? applicationId, string actorType) = await GetActorNameAsync(cancellationToken).ConfigureAwait(false);
        await foreach (string id in GetAllAggregateIdsAsync(cosmos, applicationId, actorType, cancellationToken))
        {
            await ClearCommandsAsync(applicationId, actorType, id, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public Task ClearAllStatesAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

    /// <inheritdoc/>
    public async Task ClearCommandsAsync(string aggregateGlobalId, CancellationToken cancellationToken)
    {
        (string? applicationId, string actorType) = await GetActorNameAsync(cancellationToken).ConfigureAwait(false);
        await ClearCommandsAsync(applicationId, actorType, aggregateGlobalId, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task ClearStateAsync(string aggregateGlobalId, CancellationToken cancellationToken) => throw new NotImplementedException();

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetAllActorIdsAsync(CancellationToken cancellationToken)
    {
        CosmosDbProvider cosmos = await GetCosmosProviderAsync(cancellationToken).ConfigureAwait(false);
        (string appId, string actorType) = await GetActorNameAsync(cancellationToken).ConfigureAwait(false);
        return GetAllAggregateIdsAsync(cosmos, appId, actorType, cancellationToken).ToBlockingEnumerable(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SendAllSnapshotsAsync(CancellationToken cancellationToken)
    {
        CosmosDbProvider cosmos = await GetCosmosProviderAsync(cancellationToken).ConfigureAwait(false);
        (string appId, string actorType) = await GetActorNameAsync(cancellationToken).ConfigureAwait(false);
        await foreach (string id in GetAllAggregateIdsAsync(cosmos, appId, actorType, cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            IDomainAggregateActor actor = ActorProxy.Create<IDomainAggregateActor>(new Dapr.Actors.ActorId(id), actorType);
            await actor.SendSnapshotEventAsync().ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public async Task SendSnapshotAsync(string aggregateGlobalId, CancellationToken cancellationToken)
    {
        (_, string actorType) = await GetActorNameAsync(cancellationToken).ConfigureAwait(false);
        IDomainAggregateActor actor = ActorProxy.Create<IDomainAggregateActor>(aggregateGlobalId.ToActorId(), actorType);
        cancellationToken.ThrowIfCancellationRequested();
        await actor.SendSnapshotEventAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    Task<IEnumerable<string>> IAggregateMaintenance<TAggregate>.GetAllActorIdsAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_cosmosDbProvider != null)
            {
                _cosmosDbProvider.Dispose();
                _cosmosDbProvider = null;
            }
        }
    }

    /// <summary>
    /// Get actor id from the state store item id.
    /// State id format:. <appId>||<actorType>||<actorId>||<stateName>
    /// </summary>
    /// <param name="stateId">State store item id</param>
    /// <returns>Actor id</returns>
    private static string GetActorId(string stateId) => stateId.Split("||").Skip(2).FirstOrDefault() ?? string.Empty;

    private static async IAsyncEnumerable<string> GetAllAggregateIdsAsync(
        CosmosDbProvider cosmos,
        string applicationId,
        string actorType,
        CancellationToken cancellationToken)
    {
        QueryDefinition queryDefinition = new QueryDefinition($"Select c.id From c Where StartWith(c.id,@actorType) and EndsWith(c.id, @state)")
            .WithParameter("@actorType", $"{applicationId}||{actorType}||")
            .WithParameter("@state", $"||State");
        FeedIterator<string> feedIterator = cosmos
            .Container
            .GetItemQueryIterator<string>(queryDefinition);
        while (feedIterator.HasMoreResults)
        {
            FeedResponse<string> results = await feedIterator.ReadNextAsync(cancellationToken).ConfigureAwait(false);
            foreach (string id in results)
            {
                yield return GetActorId(id);
            }
        }
    }

    private static async IAsyncEnumerable<string> GetCommandStreamStateIdsAsync(
        CosmosDbProvider cosmos,
        string applicationId,
        string actorType,
        string aggregateGlobalId,
        CancellationToken cancellationToken)
    {
        QueryDefinition queryDefinition = new QueryDefinition($"Select c.id From c Where StartWith(c.id,@aggregateGlobalId)")
            .WithParameter("@aggregateGlobalId", $"{applicationId}||{actorType}||{aggregateGlobalId}||CommandStream");
        FeedIterator<string> feedIterator = cosmos
            .Container
            .GetItemQueryIterator<string>(queryDefinition);
        while (feedIterator.HasMoreResults)
        {
            FeedResponse<string> results = await feedIterator.ReadNextAsync(cancellationToken).ConfigureAwait(false);
            foreach (string id in results)
            {
                yield return id;
            }
        }
    }

    private async Task ClearCommandsAsync(string applicationId, string actorType, string aggregateGlobalId, CancellationToken cancellationToken)
    {
        CosmosDbProvider cosmos = await GetCosmosProviderAsync(cancellationToken).ConfigureAwait(false);
        IDomainAggregateActor actor = ActorProxy.Create<IDomainAggregateActor>(aggregateGlobalId.ToActorId(), actorType);
        cancellationToken.ThrowIfCancellationRequested();
        await actor.ClearCommandsAsync().ConfigureAwait(false);
        await foreach (string id in GetCommandStreamStateIdsAsync(cosmos, applicationId, actorType, aggregateGlobalId, cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            _ = await cosmos
                .Container
                .DeleteItemAsync<DaprStateItem>(id, new PartitionKey(id), cancellationToken: cancellationToken)
                .ConfigureAwait(false);
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

    private async Task<CosmosDbProvider> GetCosmosProviderAsync(CancellationToken cancellationToken)
    {
        if (_cosmosDbProvider is not null)
        {
            return _cosmosDbProvider;
        }

        if (string.IsNullOrWhiteSpace(_containerName))
        {
            _containerName = (await _daprClient.GetMetadataAsync(cancellationToken).ConfigureAwait(false)).Id;
        }

        return new CosmosDbProvider(_connectionString, _databaseName, _containerName);
    }
}