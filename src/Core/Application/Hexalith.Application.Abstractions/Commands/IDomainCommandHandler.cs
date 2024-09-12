﻿// <copyright file="IDomainCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Commands;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Command handler interface.
/// </summary>
public interface IDomainCommandHandler
{
    /// <summary>
    /// Does the asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    Task<ExecuteCommandResult> DoAsync(object command, IDomainAggregate? aggregate, CancellationToken cancellationToken);

    /// <summary>
    /// Undoes the asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    Task<ExecuteCommandResult> UndoAsync(object command, IDomainAggregate? aggregate, CancellationToken cancellationToken);
}

/// <summary>
/// Interface ICommandHandler
/// Extends the <see cref="ICommandHandler" />.
/// </summary>
/// <typeparam name="TCommand">The type of the t command.</typeparam>
/// <seealso cref="ICommandHandler" />
public interface IDomainCommandHandler<TCommand> : ICommandHandler
    where TCommand : ICommand
{
    /// <summary>
    /// Does the asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    Task<ExecuteCommandResult> DoAsync(object command, IDomainAggregate? aggregate, CancellationToken cancellationToken);

    /// <summary>
    /// Undoes the asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    Task<ExecuteCommandResult> UndoAsync(object command, IDomainAggregate? aggregate, CancellationToken cancellationToken);
}