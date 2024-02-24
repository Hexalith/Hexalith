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

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Envelopes;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;

/// <summary>
/// Class MemoryMessageBus.
/// Implements the <see cref="IMessageBus{TMessage, TMetadata}" />.
/// </summary>
/// <seealso cref="IMessageBus{TMessage, TMetadata}" />
/// <remarks>
/// Initializes a new instance of the <see cref="MemoryEventBus"/> class.
/// </remarks>
/// <param name="dateTimeService">The date time service.</param>
public class MemoryEventBus(IDateTimeService dateTimeService) : IEventBus
{
    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService = dateTimeService;

    /// <summary>
    /// The stream.
    /// </summary>
    private readonly List<EventState> _stream = [];

    /// <summary>
    /// Gets the stream.
    /// </summary>
    /// <value>The stream.</value>
    public IEnumerable<EventState> Stream => _stream;

    /// <inheritdoc/>
    public async Task PublishAsync([NotNull] IEnvelope<BaseEvent, BaseMetadata> envelope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        await PublishAsync(envelope.Message, envelope.Metadata, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(BaseEvent message, BaseMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        await PublishAsync(new EventState(_dateTimeService.UtcNow, message, metadata), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(EventState state, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(state);
        _stream.Add(state);
        await Task.CompletedTask.ConfigureAwait(false);
    }
}