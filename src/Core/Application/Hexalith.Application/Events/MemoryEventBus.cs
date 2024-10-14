// <copyright file="MemoryEventBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.MessageMetadatas;

/// <summary>
/// Memory Event Bus.
/// </summary>
/// <param name="dateTimeService">The date time service.</param>
public class MemoryEventBus(TimeProvider dateTimeService) : IEventBus
{
    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly TimeProvider _dateTimeService = dateTimeService;

    private readonly List<(object, Metadata)>? _messageStream;

    public List<(object, Metadata)> MessageStream => _messageStream ?? [];

    /// <inheritdoc/>
    public Task PublishAsync(object message, MessageMetadatas.Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        MessageStream.Add((message, metadata));
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task PublishAsync(MessageState message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        MessageStream.Add((message.Message, message.Metadata));
        return Task.CompletedTask;
    }
}