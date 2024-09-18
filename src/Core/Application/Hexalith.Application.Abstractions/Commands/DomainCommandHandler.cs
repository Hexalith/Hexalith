// <copyright file="DomainCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Commands;

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.MessageMetadatas;
using Hexalith.Domain.Aggregates;

/// <summary>
/// Command handler for domain commands.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
public abstract class DomainCommandHandler<TCommand> : IDomainCommandHandler<TCommand>
{
    /// <inheritdoc/>
    public abstract Task<ExecuteCommandResult> DoAsync(TCommand command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public Task<ExecuteCommandResult> DoAsync(object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken) => DoAsync(ToCommand(command), metadata, aggregate, cancellationToken);

    /// <inheritdoc/>
    public abstract Task<ExecuteCommandResult> UndoAsync(TCommand command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public Task<ExecuteCommandResult> UndoAsync(object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken) => UndoAsync(ToCommand(command), metadata, aggregate, cancellationToken);

    /// <summary>
    /// Converts to command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>TCommand.</returns>
    /// <exception cref="System.ArgumentException">command.</exception>
    private static TCommand ToCommand(object command)
    {
        return command is TCommand c
            ? c
            : throw new ArgumentException($"Invalid command type. Expected: {typeof(TCommand).Name}. Command: {JsonSerializer.Serialize(command)}", nameof(command));
    }
}