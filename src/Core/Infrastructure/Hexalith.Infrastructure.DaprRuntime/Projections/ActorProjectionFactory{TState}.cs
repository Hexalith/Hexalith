// <copyright file="ActorProjectionFactory{TState}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Projections;

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Client;

using Hexalith.Application.Projections;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

/// <summary>
/// Factory class for creating projection actors.
/// </summary>
/// <typeparam name="TState">The type of the state.</typeparam>
public class ActorProjectionFactory<TState> : IProjectionFactory<TState>
{
    private string? _projectionName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorProjectionFactory{TState}" /> class.
    /// </summary>
    /// <param name="actorFactory">The actor factory.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when actorFactory is null.</exception>
    public ActorProjectionFactory(IActorProxyFactory actorFactory)
    {
        ArgumentNullException.ThrowIfNull(actorFactory);
        ActorFactory = actorFactory;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorProjectionFactory{TState}" /> class.
    /// </summary>
    /// <param name="actorFactory">The actor factory.</param>
    /// <param name="projectionName">Name of the projection.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when actorFactory is null.</exception>
    /// <exception cref="System.ArgumentException">Thrown when projectionName or applicationName is null or whitespace.</exception>
    public ActorProjectionFactory(IActorProxyFactory actorFactory, string projectionName)
    {
        ArgumentNullException.ThrowIfNull(actorFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(projectionName);

        ActorFactory = actorFactory;
        _projectionName = projectionName;
    }

    /// <summary>
    /// Gets the name of the projection actor.
    /// </summary>
    /// <value>The name of the projection actor.</value>
    public string ProjectionActorName => _projectionName ??= ProjectionActorHelper.GetProjectionActorName<TState>();

    /// <summary>
    /// Gets the actor factory.
    /// </summary>
    /// <value>The actor factory.</value>
    protected IActorProxyFactory ActorFactory { get; }

    /// <summary>
    /// Get as an asynchronous operation.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;TState&gt; representing the asynchronous operation.</returns>
    public async Task<TState?> GetStateAsync(string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateId);
        string? result = await GetProjectionActor(aggregateId)
            .GetAsync()
            .ConfigureAwait(false);
        return result is null ? (TState?)(object?)null : JsonSerializer.Deserialize<TState>(result);
    }

    /// <inheritdoc/>
    public Task RemoveStateAsync(string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateId);
        return GetProjectionActor(aggregateId).RemoveAsync();
    }

    /// <inheritdoc/>
    public async Task SetStateAsync(string aggregateId, TState state, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateId);
        await GetProjectionActor(aggregateId)
            .SetAsync(JsonSerializer.Serialize(state))
            .ConfigureAwait(false);
    }

    private IKeyValueActor GetProjectionActor(string aggregateGlobalId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateGlobalId);
        return ActorFactory.CreateActorProxy<IKeyValueActor>(aggregateGlobalId.ToActorId(), ProjectionActorName);
    }
}