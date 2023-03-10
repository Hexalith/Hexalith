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

using Hexalith.Application.Abstractions.Envelopes;
using Hexalith.Application.Abstractions.Events;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Application.Abstractions.States;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Common;

/// <summary>
/// Class MemoryMessageBus.
/// Implements the <see cref="IMessageBus{TMessage, TMetadata}" />.
/// </summary>
/// <seealso cref="IMessageBus{TMessage, TMetadata}" />
public class MemoryEventBus : IEventBus
{
    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// The stream.
    /// </summary>
    private readonly List<EventState> _stream = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryEventBus"/> class.
    /// </summary>
    /// <param name="dateTimeService">The date time service.</param>
    public MemoryEventBus(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    /// <summary>
    /// Gets the stream.
    /// </summary>
    /// <value>The stream.</value>
    public IEnumerable<EventState> Stream => _stream;

    /// <inheritdoc/>
    public Task PublishAsync(IEnvelope<BaseEvent, BaseMetadata> envelope, CancellationToken cancellationToken)
    {
        return PublishAsync(envelope.Message, envelope.Metadata, cancellationToken);
    }

    /// <inheritdoc/>
    public Task PublishAsync(BaseEvent message, BaseMetadata metadata, CancellationToken cancellationToken)
    {
        return PublishAsync(new EventState(_dateTimeService.UtcNow, message, metadata), cancellationToken);
    }

    /// <inheritdoc/>
    public Task PublishAsync(EventState state, CancellationToken cancellationToken)
    {
        _stream.Add(state);
        return Task.CompletedTask;
    }
}