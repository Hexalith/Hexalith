// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-10-2023
// ***********************************************************************
// <copyright file="IAggregateStateManager.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Aggregates;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;

using Microsoft.Extensions.Logging;

/// <summary>
/// Interface IAggregateStateManager.
/// </summary>
/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
public interface IAggregateStateManager
{
    /// <summary>
    /// Adds the command asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="retryManager">The retry manager.</param>
    /// <param name="commands">The commands.</param>
    /// <param name="metadatas">The metadatas.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task AddCommandAsync(
        IStateStoreProvider stateProvider,
        IRetryCallbackManager retryManager,
        BaseCommand[] commands,
        BaseMetadata[] metadatas,
        ILogger logger,
        CancellationToken cancellationToken);

    /// <summary>
    /// Continues the asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="retryManager">The retry manager.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="aggregateInitializer">The aggregate initializer.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;IAggregate&gt;&gt;.</returns>
    Task<IAggregate?> ContinueAsync(
        IStateStoreProvider stateProvider,
        IRetryCallbackManager retryManager,
        ResiliencyPolicy resiliencyPolicy,
        IAggregate? aggregate,
        Func<BaseEvent, IAggregate> aggregateInitializer,
        ILogger logger,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the aggregate asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="aggregateInitializer">The aggregate initializer.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;IAggregate&gt;&gt;.</returns>
    Task<IAggregate?> GetAggregateAsync(
        IStateStoreProvider stateProvider,
        Func<BaseEvent, IAggregate> aggregateInitializer,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the command count asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Int64&gt;.</returns>
    Task<long> GetCommandCountAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the commands.
    /// </summary>
    /// <typeparam name="T">Command type.</typeparam>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;T&gt;&gt;.</returns>
    Task<IEnumerable<T>> GetCommandsAsync<T>(IStateStoreProvider stateProvider, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the event count asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Int64&gt;.</returns>
    Task<long> GetEventCountAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the events.
    /// </summary>
    /// <typeparam name="T">Event type.</typeparam>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;T&gt;&gt;.</returns>
    Task<IEnumerable<T>> GetEventsAsync<T>(IStateStoreProvider stateProvider, CancellationToken cancellationToken);
}