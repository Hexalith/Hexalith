// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 10-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-26-2023
// ***********************************************************************
// <copyright file="ExternalSystemsPubSubController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.ExternalSystems.Controllers;

using System.Runtime.CompilerServices;

using Dapr;

using Hexalith.Application;
using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.States;
using Hexalith.Infrastructure.WebApis.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Class ExternalSystemsPubSubController.
/// Implements the <see cref="EventSubmissionController" />.
/// </summary>
/// <seealso cref="EventSubmissionController" />
public class ExternalSystemsCommandsController : EventSubmissionController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemsCommandsController"/> class.
    /// </summary>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="commandProcessor">The command processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="logger">The logger.</param>
    protected ExternalSystemsCommandsController(
        IIntegrationEventProcessor eventProcessor,
        ICommandProcessor commandProcessor,
        IHostEnvironment hostEnvironment,
        ILogger logger)
        : base(eventProcessor, commandProcessor, hostEnvironment, logger)
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="eventState"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [Topic(ApplicationConstants.CommandBus, "externalsystems-commands")]
    [HttpPost("/handle-external-systems-commands")]
    public static async Task<ActionResult> HandleExternalSystemsCommandsAsync(CommandState commandState)
    {
        await HandleCommandAsync(
    }
}