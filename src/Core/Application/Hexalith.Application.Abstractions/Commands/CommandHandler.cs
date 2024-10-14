// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-29-2023
// ***********************************************************************
// <copyright file="CommandHandler.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Commands;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Messages;

/// <summary>
/// Class CommandHandler.
/// Implements the <see cref="ICommandHandler{TCommand}" />.
/// </summary>
/// <typeparam name="TCommand">The type of the t command.</typeparam>
/// <seealso cref="ICommandHandler{TCommand}" />
public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand
{
    /// <inheritdoc/>
    public abstract Task<IEnumerable<BaseMessage>> DoAsync(TCommand command, IDomainAggregate? aggregate, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task<IEnumerable<BaseMessage>> UndoAsync(TCommand command, IDomainAggregate? aggregate, CancellationToken cancellationToken);

    /// <inheritdoc/>
    Task<IEnumerable<BaseMessage>> ICommandHandler.DoAsync(ICommand command, IDomainAggregate? aggregate, CancellationToken cancellationToken) => DoAsync(ToCommand(command), aggregate, cancellationToken);

    /// <inheritdoc/>
    Task<IEnumerable<BaseMessage>> ICommandHandler.UndoAsync(ICommand command, IDomainAggregate? aggregate, CancellationToken cancellationToken) => DoAsync(ToCommand(command), aggregate, cancellationToken);

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