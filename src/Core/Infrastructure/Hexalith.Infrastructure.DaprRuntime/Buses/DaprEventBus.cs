// <copyright file="DaprEventBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Buses;

using System;

using Dapr.Client;

using Hexalith.Application.Buses;
using Hexalith.Application.Events;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Represents a Dapr-based implementation of the event bus.
/// </summary>
/// <remarks>
/// This class extends the DaprApplicationBus and implements the IEventBus interface,
/// providing event publishing capabilities using Dapr.
/// </remarks>
public class DaprEventBus : DaprApplicationBus, IEventBus
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DaprEventBus"/> class.
    /// </summary>
    /// <param name="client">The Dapr client used for communication.</param>
    /// <param name="dateTimeService">The service providing date and time information.</param>
    /// <param name="settings">The options containing event bus settings.</param>
    /// <param name="logger">The logger used for logging information and errors.</param>
    /// <exception cref="ArgumentException">Thrown when the event bus name is not defined in the settings.</exception>
    public DaprEventBus(
        DaprClient client,
        TimeProvider dateTimeService,
        IOptions<EventBusSettings> settings,
        ILogger<DaprEventBus> logger)
        : base(
            client,
            dateTimeService,
            string.IsNullOrWhiteSpace(settings?.Value.Name)
                ? throw new ArgumentException($"The name of the event bus is not defined in settings ({EventBusSettings.ConfigurationName()}.{nameof(EventBusSettings.Name)}).", nameof(settings))
                : settings.Value.Name,
            "-events",
            logger)
    {
    }
}
