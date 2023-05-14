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

/// <summary>
/// Interface IAggregateStateManager.
/// </summary>
/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
public interface IAggregateStateManager
{
    /// <summary>
    /// Adds the command.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="commands">The command.</param>
    /// <param name="metadatas">The metadata.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task AddCommandAsync(
        IStateStoreProvider stateProvider,
        BaseCommand[] commands,
        BaseMetadata[] metadatas,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        CancellationToken cancellationToken);

    /// <summary>
    /// Continues the asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="aggregateInitializer">The aggregate initializer.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <param name="unregisterReminder">The remove reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;IAggregate&gt;&gt;.</returns>
    Task<IAggregate?> ContinueAsync(
        IStateStoreProvider stateProvider,
        ResiliencyPolicy resiliencyPolicy,
        IAggregate? aggregate,
        Func<BaseEvent, IAggregate> aggregateInitializer,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        Func<string, Task> unregisterReminder,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the aggregate asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="aggregateInitializer">The aggregate initializer.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Nullable&lt;IAggregate&gt;&gt;.</returns>
    Task<IAggregate?> GetAggregateAsync(IStateStoreProvider stateProvider, Func<BaseEvent, IAggregate> aggregateInitializer, CancellationToken cancellationToken);

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