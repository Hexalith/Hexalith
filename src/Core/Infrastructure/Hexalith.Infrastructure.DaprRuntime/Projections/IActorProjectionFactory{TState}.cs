// <copyright file="IActorProjectionFactory{TState}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Projections;

using Hexalith.Infrastructure.DaprRuntime.Actors;

/// <summary>
/// Interface for creating actor projections.
/// </summary>
/// <typeparam name="TState">The type of the state.</typeparam>
public interface IActorProjectionFactory<TState>
{
    /// <summary>
    /// Gets the projection actor.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <returns>IKeyValueActor.</returns>
    IKeyValueActor GetProjectionActor(string aggregateId);

    /// <summary>
    /// Gets the state asynchronously.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;TState&gt;&gt;.</returns>
    Task<TState?> GetStateAsync(string aggregateId, CancellationToken cancellationToken);

    /// <summary>
    /// Sets the state asynchronously.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="state">The state.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SetStateAsync(string aggregateId, TState state, CancellationToken cancellationToken);
}