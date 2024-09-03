// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="INotificationBus.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Notifications;

using Hexalith.Application.Envelopes;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Notifications;

/// <summary>
/// A notification bus is a component that allows to send notifications.
/// </summary>
public interface INotificationBus : IMessageBus<BaseNotification, BaseMetadata, NotificationState>
{
    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    new Task PublishAsync(IEnvelope<BaseNotification, BaseMetadata> envelope, CancellationToken cancellationToken);

    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="notificationState">State of the notification.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    new Task PublishAsync(NotificationState notificationState, CancellationToken cancellationToken);

    /// <summary>
    /// Publishes the asynchronous.
    /// </summary>
    /// <param name="notification">The message.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    new Task PublishAsync(BaseNotification notification, BaseMetadata metadata, CancellationToken cancellationToken);
}