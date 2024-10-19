// <copyright file="MemoryRequestBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.States;

/// <summary>
/// Represents an in-memory implementation of the request bus.
/// This class stores messages and their metadata in memory for processing.
/// </summary>
public class MemoryRequestBus : IRequestBus
{
    /// <summary>
    /// The internal message stream storage.
    /// </summary>
    private readonly List<(object, Metadata)>? _messageStream;

    /// <summary>
    /// Gets the message stream containing all published messages and their metadata.
    /// </summary>
    public List<(object Message, Metadata Metadata)> MessageStream => _messageStream ?? [];

    /// <inheritdoc/>
    public Task PublishAsync(MessageState requestState, CancellationToken cancellationToken)
    {
        return requestState is null
            ? Task.FromException(new ArgumentNullException(nameof(requestState)))
            : PublishAsync(requestState.Message, requestState.Metadata, cancellationToken);
    }

    /// <inheritdoc/>
    public Task PublishAsync(object message, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        MessageStream.Add((message, metadata));
        return Task.CompletedTask;
    }
}