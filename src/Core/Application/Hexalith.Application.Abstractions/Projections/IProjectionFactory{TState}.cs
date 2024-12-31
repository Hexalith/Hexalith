// <copyright file="IProjectionFactory{TState}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Projections;

/// <summary>
/// Interface for creating actor projections.
/// </summary>
/// <typeparam name="TState">The type of the state.</typeparam>
public interface IProjectionFactory<TState>
{
    /// <summary>
    /// Gets the state asynchronously.
    /// </summary>
    /// <param name="aggregateGlobalId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;TState&gt;&gt;.</returns>
    Task<TState?> GetStateAsync(string aggregateGlobalId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes the state asynchronously.
    /// </summary>
    /// <param name="aggregateGlobalId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveStateAsync(string aggregateGlobalId, CancellationToken cancellationToken);

    /// <summary>
    /// Sets the state asynchronously.
    /// </summary>
    /// <param name="aggregateGlobalId">The aggregate identifier.</param>
    /// <param name="state">The state.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SetStateAsync(string aggregateGlobalId, TState state, CancellationToken cancellationToken);
}