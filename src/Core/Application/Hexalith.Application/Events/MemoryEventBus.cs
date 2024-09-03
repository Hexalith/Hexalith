// <copyright file="MemoryEventBus.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

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
/// Memory Event Bus.
/// </summary>
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
    public async Task PublishAsync(BaseEvent baseEvent, BaseMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);
        await PublishAsync(new EventState(_dateTimeService.UtcNow, baseEvent, metadata), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(EventState state, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(state);
        _stream.Add(state);
        await Task.CompletedTask.ConfigureAwait(false);
    }
}