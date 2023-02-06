// <copyright file="ICommandHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Commands;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Domain.Abstractions.Events;

/// <summary>
/// Command handler interface.
/// </summary>
public interface ICommandHandler
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The generated events with their metadata.</returns>
    Task<IEnumerable<BaseEvent>> DoAsync(ICommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Undo the execution of the command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The generated events with their metadata.</returns>
    Task<IEnumerable<BaseEvent>> UndoAsync(ICommand command, CancellationToken cancellationToken);
}

/// <summary>
/// Interface ICommandHandler
/// Extends the <see cref="Hexalith.Application.Abstractions.Commands.ICommandHandler" />.
/// </summary>
/// <typeparam name="TCommand">The type of the t command.</typeparam>
/// <seealso cref="Hexalith.Application.Abstractions.Commands.ICommandHandler" />
public interface ICommandHandler<TCommand> : ICommandHandler
    where TCommand : ICommand
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The generated events with their metadata.</returns>
    Task<IEnumerable<BaseEvent>> DoAsync(TCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Undo the execution of the command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The generated events with their metadata.</returns>
    Task<IEnumerable<BaseEvent>> UndoAsync(TCommand command, CancellationToken cancellationToken);
}