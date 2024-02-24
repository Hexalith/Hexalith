// ***********************************************************************
// Assembly         : Hexalith.Server.Dynamics365Finance
// Author           : Jérôme Piquot
// Created          : 12-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-15-2023
// ***********************************************************************
// <copyright file="CustomerEventsController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Server.Dynamics365Finance.Infrastructure.Controllers;

using Hexalith.Application.Events;
using Hexalith.Application.Projections;
using Hexalith.Infrastructure.WebApis.PartiesEvents.Controllers;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Class CustomerEventsController.
/// Implements the <see cref="CustomerIntegrationEventsController" />.
/// </summary>
/// <seealso cref="CustomerIntegrationEventsController" />
/// <remarks>
/// Initializes a new instance of the <see cref="CustomerEventsController" /> class.
/// </remarks>
/// <param name="eventProcessor">The event processor.</param>
/// <param name="projectionProcessor">The projection processor.</param>
/// <param name="hostEnvironment">The host environment.</param>
/// <param name="logger">The logger.</param>
[ApiController]
public class CustomerEventsController(
    IIntegrationEventProcessor eventProcessor,
    IProjectionUpdateProcessor projectionProcessor,
    IHostEnvironment hostEnvironment,
    ILogger<CustomerEventsController> logger)
    : CustomerIntegrationEventsController(
        eventProcessor,
        projectionProcessor,
        hostEnvironment,
        logger)
{
}