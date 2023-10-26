// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis
// Author           : JérômePiquot
// Created          : 01-13-2023
//
// Last Modified By : JérômePiquot
// Last Modified On : 01-15-2023
// ***********************************************************************
// <copyright file="BindingController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.WebApis.Controllers;

using System.Text.Json;

using Hexalith.Application.Errors;
using Hexalith.Application.Events;
using Hexalith.Application.Helpers;
using Hexalith.Domain.Events;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Class BindingController.
/// Implements the <see cref="ControllerBase" />.
/// </summary>
/// <seealso cref="ControllerBase" />
[ApiController]
public abstract class BindingController : ReceiveMessageController
{
    /// <summary>
    /// The processor.
    /// </summary>
    private readonly IIntegrationEventProcessor _eventProcessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="BindingController" /> class.
    /// </summary>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="logger">The logger.</param>
    protected BindingController(
        IIntegrationEventProcessor eventProcessor,
        IHostEnvironment hostEnvironment,
        ILogger logger)
        : base(hostEnvironment, logger)
    {
        ArgumentNullException.ThrowIfNull(eventProcessor);
        _eventProcessor = eventProcessor;
    }

    /// <summary>
    /// Deserializes the and validate.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>IEvent.</returns>
    protected abstract (IEvent Event, string MessageId, string CorrelationId) DeserializeAndValidate(JsonElement message);

    /// <summary>
    /// Handle business event.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    protected async Task<ActionResult> HandleEventAsync(JsonElement message, CancellationToken cancellationToken)
    {
        try
        {
            if (message.ValueKind == JsonValueKind.Null)
            {
                return BadRequest("Message received is null.");
            }

            (IEvent @event, string messageId, string correlationId) = DeserializeAndValidate(message);
            MessageReceivedInformation(
                @event.TypeName,
                @event.AggregateName,
                @event.AggregateId,
                messageId,
                correlationId);
            await _eventProcessor.SubmitAsync(@event, cancellationToken).ConfigureAwait(false);
            return Accepted();
        }
        catch (ApplicationErrorException ex)
        {
            if (ex.Error is not null)
            {
                Logger.LogError(ex);
                return Problem(ex.Error);
            }

            throw;
        }
    }
}