// <copyright file="IProjectionStateManager.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Projections;

using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;

/// <summary>
/// Interface IProjectionStateManager.
/// </summary>
public interface IProjectionStateManager
{
    /// <summary>
    /// Adds the event asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="events">The events.</param>
    /// <param name="metadatas">The metadata.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task AddEventAsync(
        IStateStoreProvider stateProvider,
        object[] events,
        Metadata[] metadatas,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        CancellationToken cancellationToken);

    /// <summary>
    /// Continues the asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="projection">The projection.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <param name="unregisterReminder">The unregister reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task ContinueAsync(
        IStateStoreProvider stateProvider,
        ResiliencyPolicy resiliencyPolicy,
        IProjection projection,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        Func<string, Task> unregisterReminder,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the event count asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Int64&gt;.</returns>
    Task<long> GetEventCountAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the commands.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;T&gt;&gt;.</returns>
    Task<IEnumerable<object>> GetEventsAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken);
}