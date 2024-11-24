// <copyright file="MemoryCommandBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;

/// <summary>
/// Memory Command Bus.
/// </summary>
public class MemoryCommandBus : ICommandBus
{
    private List<(object, Metadata)>? _messagestream;

    /// <summary>
    /// Gets the message stream.
    /// </summary>
    public List<(object Message, Metadata Metadata)> MessageStream => _messagestream ??= [];

    /// <summary>
    /// Publishes a command asynchronously with the specified metadata.
    /// </summary>
    /// <param name="command">The command to be published.</param>
    /// <param name="metadata">The metadata associated with the command.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the publish operation.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="command"/> or <paramref name="metadata"/> is null.</exception>
    public async Task PublishAsync(object command, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        MessageStream.Add((command, metadata));
        await Task.CompletedTask.ConfigureAwait(false);
    }
}