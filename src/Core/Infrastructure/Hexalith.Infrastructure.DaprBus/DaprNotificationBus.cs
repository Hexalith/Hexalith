// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprBus
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="DaprNotificationBus.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprBus;

using System;

using Dapr.Client;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Application.Abstractions.Notifications;
using Hexalith.Application.Abstractions.States;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.DaprBus.Configuration;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class DaprNotificationBus.
/// Implements the <see cref="Hexalith.Infrastructure.DaprBus.DaprApplicationBus{Hexalith.Domain.Abstractions.Notifications.BaseNotification, Hexalith.Application.Abstractions.Metadatas.Metadata}" />
/// Implements the <see cref="INotificationBus" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.DaprBus.DaprApplicationBus{Hexalith.Domain.Abstractions.Notifications.BaseNotification, Hexalith.Application.Abstractions.Metadatas.Metadata}" />
/// <seealso cref="INotificationBus" />
public class DaprNotificationBus : DaprApplicationBus<BaseNotification, Metadata, NotificationState>, INotificationBus
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DaprNotificationBus"/> class.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="logger">The logger.</param>
    public DaprNotificationBus(DaprClient client, IDateTimeService dateTimeService, IOptions<DaprNotificationBusSettings> settings, ILogger<DaprNotificationBus> logger)
         : base(
        client,
        dateTimeService,
        string.IsNullOrWhiteSpace(settings.Value.Name) ? throw new ArgumentException($"The name of the notification bus is not defined in settings ({DaprNotificationBusSettings.ConfigurationName()}.{nameof(DaprNotificationBusSettings.Name)}).", nameof(settings)) : settings.Value.Name,
        "-notification",
        logger)
    {
    }
}