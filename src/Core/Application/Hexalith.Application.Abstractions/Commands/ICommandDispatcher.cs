// <copyright file="ICommandDispatcher.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Commands;

using Hexalith.Domain.Abstractions.Events;

/// <summary>
/// Interface ICommandDispatcher.
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// Does the execution of the specified command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task<IEnumerable<BaseEvent>> DoAsync(ICommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Undo the command execution of the specified command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IEnumerable&lt;BaseEvent&gt;.</returns>
    Task<IEnumerable<BaseEvent>> UnDoAsync(ICommand command, CancellationToken cancellationToken);
}