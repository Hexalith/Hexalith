// <copyright file="IAggregateStateManager.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Aggregates;

using Hexalith.Application.Abstractions.Commands;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Application.Abstractions.States;
using Hexalith.Application.Abstractions.Tasks;

/// <summary>
/// Aggregate state manager interface.
/// </summary>
public interface IAggregateStateManager
{
    /// <summary>
    /// Adds the command asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="command">The command.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task AddCommandAsync(IStateStoreProvider stateProvider, BaseCommand command, Metadata metadata, CancellationToken cancellationToken);

    /// <summary>
    /// Executes the commands asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task ExecuteCommandsAsync(IStateStoreProvider stateProvider, ResiliencyPolicy resiliencyPolicy, CancellationToken cancellationToken);

    /// <summary>
    /// Initializes aggregate state manager from the state store.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task InitializeAsync(
        IStateStoreProvider stateProvider,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        CancellationToken cancellationToken);

    /// <summary>
    /// Publishes the events asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task PublishEventsAsync(IStateStoreProvider stateProvider, ResiliencyPolicy resiliencyPolicy, CancellationToken cancellationToken);
}