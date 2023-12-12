// <copyright file="ExternalSystemReferenceController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Server.Dynamics365Finance.Infrastructure.Controllers;

using Hexalith.Application.Events;
using Hexalith.Application.Projection;
using Hexalith.Infrastructure.WebApis.ExternalSystemsEvents.Controllers;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Class ExternalSystemReferenceController.
/// Implements the <see cref="ExternalSystemsIntegrationEventsController" />.
/// </summary>
/// <seealso cref="ExternalSystemsIntegrationEventsController" />
[ApiController]
public class ExternalSystemReferenceController : ExternalSystemsIntegrationEventsController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemReferenceController" /> class.
    /// </summary>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="projectionProcessor">The projection processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="logger">The logger.</param>
    public ExternalSystemReferenceController(
        IIntegrationEventProcessor eventProcessor,
        IProjectionUpdateProcessor projectionProcessor,
        IHostEnvironment hostEnvironment,
        ILogger<ExternalSystemReferenceController> logger)
        : base(eventProcessor, projectionProcessor, hostEnvironment, logger)
    {
    }
}