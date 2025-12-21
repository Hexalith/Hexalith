// <copyright file="MemoryEventBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Commons.Metadatas;

/// <summary>
/// Memory Event Bus.
/// </summary>
public class MemoryEventBus : IEventBus
{
    private readonly List<(object, Metadata)> _messageStream = [];

    /// <summary>
    /// Gets the message stream.
    /// </summary>
    public IEnumerable<(object Message, Metadata Metadata)> MessageStream => _messageStream;

    /// <inheritdoc/>
    public Task PublishAsync(object message, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        _messageStream.Add((message, metadata));
        return Task.CompletedTask;
    }
}