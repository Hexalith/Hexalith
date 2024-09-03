// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprBus
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="DaprNotificationBus.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Buses;

using System;

using Dapr.Client;

using Hexalith.Application.Buses;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Notifications;
using Hexalith.Application.States;
using Hexalith.Domain.Notifications;
using Hexalith.Extensions.Common;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class DaprNotificationBus.
/// Implements the <see cref="DaprBus.DaprApplicationBus{Abstractions.Notifications.BaseNotification, Metadata}" />
/// Implements the <see cref="INotificationBus" />.
/// </summary>
/// <seealso cref="DaprBus.DaprApplicationBus{Abstractions.Notifications.BaseNotification, Metadata}" />
/// <seealso cref="INotificationBus" />
/// <remarks>
/// Initializes a new instance of the <see cref="DaprNotificationBus"/> class.
/// </remarks>
/// <param name="client">The client.</param>
/// <param name="dateTimeService">The date time service.</param>
/// <param name="settings">The settings.</param>
/// <param name="logger">The logger.</param>
public class DaprNotificationBus(DaprClient client, IDateTimeService dateTimeService, IOptions<NotificationBusSettings> settings, ILogger<DaprNotificationBus> logger)
    : DaprApplicationBus<BaseNotification, BaseMetadata, NotificationState>(
    client,
    dateTimeService,
    string.IsNullOrWhiteSpace(settings?.Value.Name) ? throw new ArgumentException($"The name of the notification bus is not defined in settings ({NotificationBusSettings.ConfigurationName()}.{nameof(NotificationBusSettings.Name)}).", nameof(settings)) : settings.Value.Name,
    "-notifications",
    logger), INotificationBus
{
}