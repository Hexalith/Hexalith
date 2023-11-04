// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 10-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-29-2023
// ***********************************************************************
// <copyright file="ExternalSystemsIntegrationCommandsController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.ExternalSystemsCommands.Controllers;

using Hexalith.Application.States;
using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.WebApis.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Class ExternalSystemsPubSubController.
/// Implements the <see cref="CommandIntegrationController" />.
/// </summary>
/// <seealso cref="CommandIntegrationController" />
[ApiController]
public class ExternalSystemsIntegrationCommandsController : CommandSubmissionController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemsIntegrationCommandsController" /> class.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="logger">The logger.</param>
    public ExternalSystemsIntegrationCommandsController(
        ConventionNamingCommandProcessor commandProcessor,
        IHostEnvironment hostEnvironment,
        ILogger<ExternalSystemsIntegrationCommandsController> logger)
        : base(commandProcessor, hostEnvironment, logger)
    {
    }

    /// <summary>
    /// Handle external systems commands as an asynchronous operation.
    /// </summary>
    /// <param name="commandState">State of the command.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    [ExternalSystemReferenceCommandsBusTopic]
    [HttpPost("/handle-external-system-reference-commands")]
    public async Task<ActionResult> HandleExternalSystemsCommandsAsync(CommandState commandState)
    {
        return await HandleCommandAsync(
                    commandState,
                    ExternalSystemReference.GetAggregateName(),
                    CancellationToken.None)
                .ConfigureAwait(false);
    }
}