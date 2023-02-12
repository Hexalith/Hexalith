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
        Metadata[] metadatas,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        CancellationToken cancellationToken);

    /// <summary>
    /// Continues the execution of the commands and publish all events.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task ContinueAsync(
        IStateStoreProvider stateProvider,
        ResiliencyPolicy resiliencyPolicy,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets the command count asynchronous.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Int64&gt;.</returns>
    Task<long> GetCommandCountAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken);
}