// <copyright file="MemoryEventBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;

/// <summary>
/// Memory Event Bus.
/// </summary>
public class MemoryEventBus : IEventBus
{
    private List<(object, Metadata)>? _messageStream;

    /// <summary>
    /// Gets the message stream.
    /// </summary>
    public List<(object Message, Metadata Metadata)> MessageStream => _messageStream ??= [];

    /// <summary>
    /// Publishes a message asynchronously with the specified metadata.
    /// </summary>
    /// <param name="message">The message to be published.</param>
    /// <param name="metadata">The metadata associated with the message.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the publish operation.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> or <paramref name="metadata"/> is null.</exception>
    public Task PublishAsync(object message, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        MessageStream.Add((message, metadata));
        return Task.CompletedTask;
    }
}