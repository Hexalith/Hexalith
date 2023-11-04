// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.Parties
// Author           : Jérôme Piquot
// Created          : 10-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="PartiesIntegrationEventsController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.PartiesEvents.Controllers;

using Hexalith.Application.Events;
using Hexalith.Application.Projection;
using Hexalith.Application.States;
using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.WebApis.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Class PartiesPubSubController.
/// Implements the <see cref="EventIntegrationController" />.
/// </summary>
/// <seealso cref="EventIntegrationController" />
[ApiController]
public class PartiesIntegrationEventsController : EventIntegrationController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartiesIntegrationEventsController"/> class.
    /// </summary>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="projectionProcessor">The projection processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="logger">The logger.</param>
    public PartiesIntegrationEventsController(
        IIntegrationEventProcessor eventProcessor,
        IProjectionUpdateProcessor projectionProcessor,
        IHostEnvironment hostEnvironment,
        ILogger<PartiesIntegrationEventsController> logger)
        : base(eventProcessor, projectionProcessor, hostEnvironment, logger)
    {
    }

    /// <summary>
    /// Handle aggregate external reference events as an asynchronous operation.
    /// </summary>
    /// <param name="eventState">State of the event.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    [CustomerEventsBusTopic]
    [HttpPost("/handle-customer-events")]
    public async Task<ActionResult> HandleCustomerEventsAsync(EventState eventState)
         => await HandleEventAsync(
                eventState,
                Customer.GetAggregateName(),
                CancellationToken.None)
             .ConfigureAwait(false);
}