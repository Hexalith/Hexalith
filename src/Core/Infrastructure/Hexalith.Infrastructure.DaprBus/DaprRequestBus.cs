// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprBus
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="DaprRequestBus.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.Application.Abstractions.Requests;
using Hexalith.Application.Abstractions.States;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.DaprBus.Configuration;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class DaprRequestBus.
/// Implements the <see cref="Hexalith.Infrastructure.DaprBus.DaprApplicationBus{Hexalith.Domain.Abstractions.Requests.BaseRequest, Hexalith.Application.Abstractions.Metadatas.Metadata}" />
/// Implements the <see cref="IRequestBus" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.DaprBus.DaprApplicationBus{Hexalith.Domain.Abstractions.Requests.BaseRequest, Hexalith.Application.Abstractions.Metadatas.Metadata}" />
/// <seealso cref="IRequestBus" />
public class DaprRequestBus : DaprApplicationBus<BaseRequest, Metadata, RequestState>, IRequestBus
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DaprRequestBus"/> class.
    /// </summary>
    /// <param name="client">The client.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="logger">The logger.</param>
    public DaprRequestBus(DaprClient client, IDateTimeService dateTimeService, IOptions<DaprRequestBusSettings> settings, ILogger<DaprRequestBus> logger)
         : base(
        client,
        dateTimeService,
        string.IsNullOrWhiteSpace(settings.Value.Name) ? throw new ArgumentException($"The name of the request bus is not defined in settings ({DaprRequestBusSettings.ConfigurationName()}.{nameof(DaprRequestBusSettings.Name)}).", nameof(settings)) : settings.Value.Name,
        "-request",
        logger)
    {
    }
}