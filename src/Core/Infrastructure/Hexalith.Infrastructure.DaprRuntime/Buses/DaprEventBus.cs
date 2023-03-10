// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprBus
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="DaprEventBus.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Buses;

using System;

using Dapr.Client;

using Hexalith.Application.Abstractions.Events;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Application.Abstractions.States;
using Hexalith.Application.Configuration;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Common;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class DaprEventBus.
/// Implements the <see cref="DaprBus.DaprApplicationBus{Hexalith.Domain.Abstractions.Events.BaseEvent, Hexalith.Application.Abstractions.Metadatas.Metadata}" />
/// Implements the <see cref="IEventBus" />.
/// </summary>
/// <seealso cref="DaprBus.DaprApplicationBus{Hexalith.Domain.Abstractions.Events.BaseEvent, Hexalith.Application.Abstractions.Metadatas.Metadata}" />
/// <seealso cref="IEventBus" />
public class DaprEventBus : DaprApplicationBus<BaseEvent, BaseMetadata, EventState>, IEventBus
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DaprEventBus"/> class.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="logger">The logger.</param>
    public DaprEventBus(DaprClient client, IDateTimeService dateTimeService, IOptions<EventBusSettings> settings, ILogger<DaprEventBus> logger)
         : base(
        client,
        dateTimeService,
        string.IsNullOrWhiteSpace(settings.Value.Name) ? throw new ArgumentException($"The name of the event bus is not defined in settings ({EventBusSettings.ConfigurationName()}.{nameof(EventBusSettings.Name)}).", nameof(settings)) : settings.Value.Name,
        "-event",
        logger)
    {
    }
}