// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprBus
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="DaprEventBus.cs" company="Jérôme Piquot">
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
using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class DaprEventBus.
/// Implements the <see cref="DaprBus.DaprApplicationBus{BaseEvent, Hexalith.Application.Abstractions.Metadatas.Metadata}" />
/// Implements the <see cref="IEventBus" />.
/// </summary>
/// <seealso cref="DaprBus.DaprApplicationBus{BaseEvent, Hexalith.Application.Abstractions.Metadatas.Metadata}" />
/// <seealso cref="IEventBus" />
/// <remarks>
/// Initializes a new instance of the <see cref="DaprEventBus"/> class.
/// </remarks>
/// <param name="client">The client.</param>
/// <param name="dateTimeService">The date time service.</param>
/// <param name="settings">The settings.</param>
/// <param name="logger">The logger.</param>
public class DaprEventBus(DaprClient client, TimeProvider dateTimeService, IOptions<EventBusSettings> settings, ILogger<DaprEventBus> logger) : DaprApplicationBus<BaseEvent, BaseMetadata, EventState>(
    client,
    dateTimeService,
    string.IsNullOrWhiteSpace(settings?.Value.Name) ? throw new ArgumentException($"The name of the event bus is not defined in settings ({EventBusSettings.ConfigurationName()}.{nameof(EventBusSettings.Name)}).", nameof(settings)) : settings.Value.Name,
    "-events",
    logger), IEventBus
{
}