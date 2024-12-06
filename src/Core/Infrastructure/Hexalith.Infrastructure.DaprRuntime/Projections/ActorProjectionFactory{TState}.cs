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
    /// <summary>
    /// Initializes a new instance of the <see cref="ActorProjectionFactory{TState}" /> class.
    /// </summary>
    /// <param name="actorFactory">The actor factory.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when actorFactory is null.</exception>
    public ActorProjectionFactory(IActorProxyFactory actorFactory, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(actorFactory);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationName);
        ActorFactory = actorFactory;
        ApplicationName = applicationName;
    }

    /// <summary>
    /// Gets the name of the projection actor.
    /// </summary>
    /// <value>The name of the projection actor.</value>
    public string ProjectionActorName => ProjectionActorHelper.GetProjectionActorName<TState>(ApplicationName);

    /// <summary>
    /// Gets the actor factory.
    /// </summary>
    /// <value>The actor factory.</value>
    protected IActorProxyFactory ActorFactory { get; }

    /// <summary>
    /// Gets the name of the application.
    /// </summary>
    /// <value>The name of the application.</value>
    protected string ApplicationName { get; }

    /// <inheritdoc/>
    public IKeyValueActor GetProjectionActor(string aggregateGlobalId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateGlobalId);
        return ActorFactory.CreateActorProxy<IKeyValueActor>(aggregateGlobalId.ToActorId(), ProjectionActorName);
    }

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
    public async Task SetStateAsync(string aggregateId, TState state, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateId);
        await GetProjectionActor(aggregateId)
            .SetAsync(JsonSerializer.Serialize(state))
            .ConfigureAwait(false);
    }
}