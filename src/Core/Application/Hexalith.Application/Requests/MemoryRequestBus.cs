// <copyright file="MemoryRequestBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Commons.Metadatas;

/// <summary>
/// Represents an in-memory implementation of the request bus.
/// This class stores messages and their metadata in memory for processing.
/// </summary>
public class MemoryRequestBus : IRequestBus
{
    /// <summary>
    /// The internal message stream storage.
    /// </summary>
    private readonly List<(object, Metadata)> _messageStream = [];

    /// <summary>
    /// Gets the message stream containing all published messages and their metadata.
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