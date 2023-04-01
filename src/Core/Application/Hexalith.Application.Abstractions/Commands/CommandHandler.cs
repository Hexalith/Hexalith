// <copyright file="CommandHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Commands;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Hexalith.Domain.Abstractions.Messages;

/// <summary>
/// Class CommandHandler.
/// Implements the <see cref="Hexalith.Application.Abstractions.Commands.ICommandHandler{TCommand}" />.
/// </summary>
/// <typeparam name="TCommand">The type of the t command.</typeparam>
/// <seealso cref="Hexalith.Application.Abstractions.Commands.ICommandHandler{TCommand}" />
public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    /// <inheritdoc/>
    public abstract Task<IEnumerable<BaseMessage>> DoAsync(TCommand command, CancellationToken cancellationToken);

    /// <inheritdoc/>
    Task<IEnumerable<BaseMessage>> ICommandHandler.DoAsync(ICommand command, CancellationToken cancellationToken)
    {
        return DoAsync(ToCommand(command), cancellationToken);
    }

    /// <inheritdoc/>
    public abstract Task<IEnumerable<BaseMessage>> UndoAsync(TCommand command, CancellationToken cancellationToken);

    /// <inheritdoc/>
    Task<IEnumerable<BaseMessage>> ICommandHandler.UndoAsync(ICommand command, CancellationToken cancellationToken)
    {
        return DoAsync(ToCommand(command), cancellationToken);
    }

    /// <summary>
    /// Converts to command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>TCommand.</returns>
    /// <exception cref="System.ArgumentException">command.</exception>
    private static TCommand ToCommand(ICommand command)
    {
        return command is TCommand c
            ? c
            : throw new ArgumentException($"{command.GetType().Name} is an invalid command type. Expected: {typeof(TCommand).Name}.", nameof(command));
    }
}