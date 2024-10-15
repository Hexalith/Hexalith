// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis
// Author           : JérômePiquot
// Created          : 01-13-2023
//
// Last Modified By : JérômePiquot
// Last Modified On : 10-26-2023
// ***********************************************************************
// <copyright file="EventIntegrationController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.WebApis.Controllers;

using Hexalith.Application.Events;
using Hexalith.Application.MessageMetadatas;
using Hexalith.Application.Projections;
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
[Route("api/events")]
public class EventIntegrationController : ReceiveMessageController
{
    /// <summary>
    /// The processor.
    /// </summary>
    private readonly IIntegrationEventProcessor _eventProcessor;

    private readonly IProjectionUpdateProcessor _projectionProcessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventIntegrationController"/> class.
    /// </summary>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="projectionProcessor">The projection processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected EventIntegrationController(
        IIntegrationEventProcessor eventProcessor,
        IProjectionUpdateProcessor projectionProcessor,
        IHostEnvironment hostEnvironment,
        ILogger logger)
        : base(hostEnvironment, logger)
    {
        ArgumentNullException.ThrowIfNull(eventProcessor);
        ArgumentNullException.ThrowIfNull(projectionProcessor);

        _eventProcessor = eventProcessor;
        _projectionProcessor = projectionProcessor;
    }

    /// <summary>
    /// Handle event as an asynchronous operation.
    /// </summary>
    /// <param name="eventState">State of the event.</param>
    /// <param name="validAggregateName">Name of the valid aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    protected async Task<ActionResult> HandleEventAsync(MessageState eventState, string validAggregateName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(eventState);
        ActionResult? badRequest = MessageValidation<MessageState>(eventState, validAggregateName);
        if (badRequest != null)
        {
            return badRequest;
        }

        try
        {
            await _projectionProcessor
                .ApplyAsync(eventState.Message!, eventState.Metadata!, cancellationToken)
                .ConfigureAwait(false);
            await _eventProcessor
                .SubmitAsync(eventState.Message!, eventState.Metadata!, cancellationToken)
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