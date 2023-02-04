// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="MemoryEventBus.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Events;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Envelopes;
using Hexalith.Application.Abstractions.Events;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Application.Abstractions.Notifications;
using Hexalith.Application.Abstractions.Requests;
using Hexalith.Domain.Abstractions.Events;

/// <summary>
/// Class MemoryMessageBus.
/// Implements the <see cref="IMessageBus{TMessage, TMetadata}" />.
/// </summary>
/// <typeparam name="TMessage">The type of the t message.</typeparam>
/// <typeparam name="TMetadata">The type of the t metadata.</typeparam>
/// <seealso cref="IMessageBus{TMessage, TMetadata}" />
public class MemoryEventBus : IEventBus
{
    /// <summary>
    /// The stream.
    /// </summary>
    private readonly List<(BaseEvent, Metadata)> _stream = new();

    /// <summary>
    /// Gets the stream.
    /// </summary>
    /// <value>The stream.</value>
    public IEnumerable<(BaseEvent Message, Metadata Metadata)> Stream => _stream;

    /// <inheritdoc/>
    public Task PublishAsync(IEnvelope<BaseEvent, Metadata> envelope, CancellationToken cancellationToken)
    {
        return PublishAsync(envelope.Message, envelope.Metadata, cancellationToken);
    }

    /// <inheritdoc/>
    public Task PublishAsync(BaseEvent message, Metadata metadata, CancellationToken cancellationToken)
    {
        _stream.Add((message, metadata));
        return Task.CompletedTask;
    }
}