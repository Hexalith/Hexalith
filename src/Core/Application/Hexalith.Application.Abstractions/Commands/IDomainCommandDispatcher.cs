// <copyright file="IDomainCommandDispatcher.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    Task<IEnumerable<object>> DoAsync(object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken);

    /// <summary>
    /// Uns the do asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="metadata"> The message metadata.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    Task<IEnumerable<object>> UnDoAsync(object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken);
}