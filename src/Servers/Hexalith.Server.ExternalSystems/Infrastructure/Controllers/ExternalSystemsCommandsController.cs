// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 10-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-26-2023
// ***********************************************************************
// <copyright file="ExternalSystemsCommandsController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Server.ExternalSystems.Infrastructure.Controllers;

using Dapr;

using Hexalith.Application;
using Hexalith.Application.Commands;
using Hexalith.Application.States;
using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.WebApis.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Class ExternalSystemsPubSubController.
/// Implements the <see cref="EventIntegrationController" />.
/// </summary>
/// <seealso cref="EventIntegrationController" />
public class ExternalSystemsCommandsController : CommandSubmissionController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemsCommandsController"/> class.
    /// </summary>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="commandProcessor">The command processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="logger">The logger.</param>
    protected ExternalSystemsCommandsController(
        ICommandProcessor commandProcessor,
        IHostEnvironment hostEnvironment,
        ILogger logger)
        : base(commandProcessor, hostEnvironment, logger)
    {
    }

    /// <summary>
    /// Handle external systems commands as an asynchronous operation.
    /// </summary>
    /// <param name="commandState">State of the command.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    [Topic(ApplicationConstants.CommandBus, "externalsystemreference-commands")]
    [HttpPost("/handle-external-system-reference-commands")]
    public async Task<ActionResult> HandleExternalSystemsCommandsAsync(CommandState commandState)
        => await HandleCommandAsync(commandState, ExternalSystemReference.GetAggregateName(), CancellationToken.None)
            .ConfigureAwait(false);
}