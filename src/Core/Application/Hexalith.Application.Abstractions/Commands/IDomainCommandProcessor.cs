// <copyright file="IDomainCommandProcessor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Commands;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Command processor interface.
/// </summary>
public interface IDomainCommandProcessor
{
    /// <summary>
    /// Submits commands.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="metadata">The command metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SubmitAsync(object command, MessageMetadatas.Metadata metadata, CancellationToken cancellationToken);
}