// <copyright file="DaprCommandBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Buses;

using System;
using System.Diagnostics.CodeAnalysis;

using Dapr.Client;

using Hexalith.Application;
using Hexalith.Application.Buses;
using Hexalith.Applications.Commands;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Represents a Dapr-based implementation of the command bus.
/// </summary>
/// <remarks>
/// This class extends the DaprApplicationBus to provide command bus functionality
/// using Dapr as the underlying infrastructure.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="DaprCommandBus"/> class.
/// </remarks>
/// <param name="client">The Dapr client used for communication.</param>
/// <param name="dateTimeService">The service providing date and time information.</param>
/// <param name="settings">The options containing command bus settings.</param>
/// <param name="logger">The logger for DaprCommandBus.</param>
/// <exception cref="ArgumentException">Thrown when the command bus name is not defined in settings.</exception>
public class DaprCommandBus(
    DaprClient client,
    TimeProvider dateTimeService,
    IOptions<CommandBusSettings> settings,
    ILogger<DaprCommandBus> logger) : DaprApplicationBus(
        client,
        dateTimeService,
        GetName(settings?.Value.Name),
        ApplicationConstants.CommandBusSuffix,
        logger), ICommandBus
{
    private static string GetName([NotNull] string? name)
    {
        return string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException(
                    $"The name of the command bus is not defined in settings ({CommandBusSettings.ConfigurationName()}.{nameof(CommandBusSettings.Name)}).",
                    nameof(name))
                : name;
    }
}