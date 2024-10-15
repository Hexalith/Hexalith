// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis
// Author           : JérômePiquot
// Created          : 01-13-2023
//
// Last Modified By : JérômePiquot
// Last Modified On : 10-26-2023
// ***********************************************************************
// <copyright file="CommandSubmissionController.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.WebApis.Controllers;

using Hexalith.Application.Commands;
using Hexalith.Application.States;
using Hexalith.Extensions.Errors;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Class PubSubController.
/// Implements the <see cref="ControllerBase" />.
/// </summary>
/// <seealso cref="ControllerBase" />
[ApiController]
[Route("api/commands")]
public class CommandSubmissionController : ReceiveMessageController
{
    /// <summary>
    /// The command processor.
    /// </summary>
    private readonly ICommandProcessor _commandProcessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandSubmissionController"/> class.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected CommandSubmissionController(
        ICommandProcessor commandProcessor,
        IHostEnvironment hostEnvironment,
        ILogger logger)
        : base(hostEnvironment, logger)
    {
        ArgumentNullException.ThrowIfNull(commandProcessor);

        _commandProcessor = commandProcessor;
    }

    /// <summary>
    /// Handle command as an asynchronous operation.
    /// </summary>
    /// <param name="commandState">State of the command.</param>
    /// <param name="validAggregateName">Name of the valid aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected async Task<ActionResult> HandleCommandAsync(MessageState commandState, string validAggregateName, CancellationToken cancellationToken)
    {
        ActionResult? badRequest = MessageValidation<MessageState>(commandState, validAggregateName);
        if (badRequest is not null)
        {
            return badRequest;
        }

        try
        {
            await _commandProcessor
                .SubmitAsync(commandState.Message!, commandState.Metadata!, cancellationToken)
                .ConfigureAwait(false);
            return Ok();
        }
        catch (ApplicationErrorException ex)
        {
            if (ex.Error is not null)
            {
                return Problem(ex.Error, ex);
            }

            throw;
        }
    }
}