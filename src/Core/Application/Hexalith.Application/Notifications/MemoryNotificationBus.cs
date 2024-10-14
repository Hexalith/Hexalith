// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="MemoryNotificationBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Notifications;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Envelopes;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Notifications;

/// <summary>
/// Memory Notification Bus.
/// </summary>
/// <param name="dateTimeService">The date time service.</param>
public class MemoryNotificationBus(TimeProvider dateTimeService) : INotificationBus
{
    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly TimeProvider _dateTimeService = dateTimeService;

    private readonly List<(object, MessageMetadatas.Metadata)> _messageStream = [];

    /// <summary>
    /// The stream.
    /// </summary>
    private readonly List<NotificationState> _stream = [];

    /// <summary>
    /// Gets the stream.
    /// </summary>
    /// <value>The stream.</value>
    public IEnumerable<NotificationState> Stream => _stream;

    /// <inheritdoc/>
    public async Task PublishAsync(IEnvelope<BaseNotification, BaseMetadata> envelope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        await PublishAsync(envelope.Message, envelope.Metadata, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(BaseNotification notification, BaseMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notification);
        await PublishAsync(new NotificationState(_dateTimeService.GetUtcNow(), notification, metadata), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(NotificationState notificationState, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notificationState);
        _stream.Add(notificationState);
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task PublishAsync(object message, MessageMetadatas.Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        _messageStream.Add((message, metadata));
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task PublishAsync(MessageMetadatas.MessageState message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        _messageStream.Add((message.Message, message.Metadata));
        return Task.CompletedTask;
    }
}