// <copyright file="IAggregateMaintenance.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Aggregates;

/// <summary>
/// Represents an interface for maintaining aggregates.
/// </summary>
public interface IAggregateMaintenance
{
    /// <summary>
    /// Clears all commands for the aggregate.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ClearAllCommandsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Clears all states for the aggregate.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ClearAllStatesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Clears the commands for the specified aggregate.
    /// </summary>
    /// <param name="aggregateGlobalId">The ID of the aggregate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ClearCommandsAsync(string aggregateGlobalId, CancellationToken cancellationToken);

    /// <summary>
    /// Clears the state for the specified aggregate.
    /// </summary>
    /// <param name="aggregateGlobalId">The ID of the aggregate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ClearStateAsync(string aggregateGlobalId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all actor IDs.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<IEnumerable<string>> GetAllActorIdsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Sends all snapshots for the aggregate.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendAllSnapshotsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Sends a snapshot for the specified aggregate.
    /// </summary>
    /// <param name="aggregateGlobalId">The ID of the aggregate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendSnapshotAsync(string aggregateGlobalId, CancellationToken cancellationToken);
}