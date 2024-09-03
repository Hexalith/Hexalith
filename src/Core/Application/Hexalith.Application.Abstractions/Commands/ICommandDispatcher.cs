// <copyright file="ICommandDispatcher.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Messages;

/// <summary>
/// Interface ICommandDispatcher.
/// </summary>
public interface ICommandDispatcher
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
    /// Uns the do asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    Task<IEnumerable<BaseMessage>> UnDoAsync(ICommand command, IAggregate? aggregate, CancellationToken cancellationToken);
}