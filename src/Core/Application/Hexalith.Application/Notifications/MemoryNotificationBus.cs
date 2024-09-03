// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="MemoryNotificationBus.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
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
using Hexalith.Extensions.Common;

/// <summary>
/// Memory Notification Bus.
/// </summary>
/// <param name="dateTimeService">The date time service.</param>
public class MemoryNotificationBus(IDateTimeService dateTimeService) : INotificationBus
{
    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService = dateTimeService;

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
        await PublishAsync(new NotificationState(_dateTimeService.UtcNow, notification, metadata), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(NotificationState notificationState, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notificationState);
        _stream.Add(notificationState);
        await Task.CompletedTask.ConfigureAwait(false);
    }
}