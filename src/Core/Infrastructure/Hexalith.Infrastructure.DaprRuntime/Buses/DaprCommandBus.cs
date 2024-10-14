// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprBus
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="DaprCommandBus.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Buses;

using System;

using Dapr.Client;

using Hexalith.Application;
using Hexalith.Application.Buses;
using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Extensions.Common;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class DaprCommandBus.
/// Implements the <see cref="DaprBus.DaprApplicationBus{Hexalith.Domain.Abstractions.Commands.BaseCommand, Hexalith.Application.Abstractions.Metadatas.Metadata}" />
/// Implements the <see cref="ICommandBus" />.
/// </summary>
/// <seealso cref="DaprBus.DaprApplicationBus{Hexalith.Domain.Abstractions.Commands.BaseCommand, Hexalith.Application.Abstractions.Metadatas.Metadata}" />
/// <seealso cref="ICommandBus" />
/// <remarks>
/// Initializes a new instance of the <see cref="DaprCommandBus"/> class.
/// </remarks>
/// <param name="client">The client.</param>
/// <param name="dateTimeService">The date time service.</param>
/// <param name="settings">The settings.</param>
/// <param name="logger">The logger.</param>
public class DaprCommandBus(DaprClient client, TimeProvider dateTimeService, IOptions<CommandBusSettings> settings, ILogger<DaprCommandBus> logger)
    : DaprApplicationBus<BaseCommand, BaseMetadata, CommandState>(
    client,
    dateTimeService,
    string.IsNullOrWhiteSpace(settings?.Value.Name) ? throw new ArgumentException($"The name of the command bus is not defined in settings ({CommandBusSettings.ConfigurationName()}.{nameof(CommandBusSettings.Name)}).", nameof(settings)) : settings.Value.Name,
    ApplicationConstants.CommandBusSuffix,
    logger), ICommandBus
{
}