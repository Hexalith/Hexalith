// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprBus
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="DaprRequestBus.cs" company="Jérôme Piquot">
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
using Hexalith.Application.Requests;
using Hexalith.Application.States;
using Hexalith.Extensions.Common;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class DaprRequestBus.
/// Implements the <see cref="DaprBus.DaprApplicationBus{Abstractions.Requests.BaseRequest, Metadata}" />
/// Implements the <see cref="IRequestBus" />.
/// </summary>
/// <seealso cref="DaprBus.DaprApplicationBus{Abstractions.Requests.BaseRequest, Metadata}" />
/// <seealso cref="IRequestBus" />
/// <remarks>
/// Initializes a new instance of the <see cref="DaprRequestBus"/> class.
/// </remarks>
/// <param name="client">The client.</param>
/// <param name="dateTimeService">The date time service.</param>
/// <param name="settings">The settings.</param>
/// <param name="logger">The logger.</param>
public class DaprRequestBus(DaprClient client, IDateTimeService dateTimeService, IOptions<RequestBusSettings> settings, ILogger<DaprRequestBus> logger) : DaprApplicationBus<BaseRequest, BaseMetadata, RequestState>(
    client,
    dateTimeService,
    string.IsNullOrWhiteSpace(settings?.Value.Name) ? throw new ArgumentException($"The name of the request bus is not defined in settings ({RequestBusSettings.ConfigurationName()}.{nameof(RequestBusSettings.Name)}).", nameof(settings)) : settings.Value.Name,
    "-requests",
    logger), IRequestBus
{
}