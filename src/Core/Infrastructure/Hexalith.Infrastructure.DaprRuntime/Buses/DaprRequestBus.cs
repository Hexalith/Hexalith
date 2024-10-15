// <copyright file="DaprRequestBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Buses;

using System;

using Dapr.Client;

using Hexalith.Application.Buses;
using Hexalith.Application.Requests;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Represents a Dapr-based implementation of the request bus.
/// </summary>
/// <remarks>
/// This class extends the DaprApplicationBus and implements the IRequestBus interface,
/// providing a mechanism for handling requests using Dapr runtime.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="DaprRequestBus"/> class.
/// </remarks>
/// <param name="client">The Dapr client for interacting with the Dapr runtime.</param>
/// <param name="dateTimeService">The service for providing date and time information.</param>
/// <param name="settings">The configuration settings for the request bus.</param>
/// <param name="logger">The logger for logging information and errors.</param>
/// <exception cref="ArgumentException">Thrown when the request bus name is not defined in the settings.</exception>
public class DaprRequestBus(
    DaprClient client,
    TimeProvider dateTimeService,
    IOptions<RequestBusSettings> settings,
    ILogger<DaprRequestBus> logger) : DaprApplicationBus(
        client,
        dateTimeService,
        string.IsNullOrWhiteSpace(settings?.Value.Name) ? throw new ArgumentException($"The name of the request bus is not defined in settings ({RequestBusSettings.ConfigurationName()}.{nameof(RequestBusSettings.Name)}).", nameof(settings)) : settings.Value.Name,
        "-requests",
        logger), IRequestBus
{
}