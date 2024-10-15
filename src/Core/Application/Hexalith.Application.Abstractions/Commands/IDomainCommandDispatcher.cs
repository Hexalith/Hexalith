// <copyright file="IDomainCommandDispatcher.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using Hexalith.Application.MessageMetadatas;
using Hexalith.Domain.Aggregates;

/// <summary>
/// Domain command dispatcher interface.
/// </summary>
public interface IDomainCommandDispatcher
{
    /// <summary>
    /// Does the asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="metadata"> The message metadata.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;object&gt;&gt;.</returns>
    Task<ExecuteCommandResult> DoAsync(object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken);

    /// <summary>
    /// Uns the do asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="metadata"> The message metadata.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;object&gt;&gt;.</returns>
    Task<ExecuteCommandResult> UnDoAsync(object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken);
}