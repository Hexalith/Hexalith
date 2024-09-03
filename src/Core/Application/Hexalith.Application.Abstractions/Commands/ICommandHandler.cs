// <copyright file="ICommandHandler.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Messages;

/// <summary>
/// Command handler interface.
/// </summary>
public interface ICommandHandler
{
    /// <summary>
    /// Does the asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    Task<IEnumerable<BaseMessage>> DoAsync(ICommand command, IAggregate? aggregate, CancellationToken cancellationToken);

    /// <summary>
    /// Undoes the asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    Task<IEnumerable<BaseMessage>> UndoAsync(ICommand command, IAggregate? aggregate, CancellationToken cancellationToken);
}

/// <summary>
/// Interface ICommandHandler
/// Extends the <see cref="ICommandHandler" />.
/// </summary>
/// <typeparam name="TCommand">The type of the t command.</typeparam>
/// <seealso cref="ICommandHandler" />
public interface ICommandHandler<TCommand> : ICommandHandler
    where TCommand : ICommand
{
    /// <summary>
    /// Does the asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    Task<IEnumerable<BaseMessage>> DoAsync(TCommand command, IAggregate? aggregate, CancellationToken cancellationToken);

    /// <summary>
    /// Undoes the asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    Task<IEnumerable<BaseMessage>> UndoAsync(TCommand command, IAggregate? aggregate, CancellationToken cancellationToken);
}