// <copyright file="BindingController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Controllers;

using System.Text.Json;

using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Errors;

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
    /// <returns>System.ValueTuple&lt;object, Metadata&gt;.</returns>
    protected abstract (object Event, Metadata Metadata) DeserializeAndValidate(JsonElement message);

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

            (object @event, Metadata metadata) = DeserializeAndValidate(message);
            MessageReceivedInformation(
                Logger,
                metadata.Message.Name,
                metadata.PartitionKey,
                metadata.Message.Id,
                metadata.Context.CorrelationId);
            await _eventProcessor.SubmitAsync(@event, metadata, cancellationToken).ConfigureAwait(false);
            await PostHandleEventProcessAsync(@event, metadata, cancellationToken).ConfigureAwait(false);
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

    /// <summary>
    /// Posts the handle event process.
    /// </summary>
    /// <param name="ievent">The event.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    protected virtual Task PostHandleEventProcessAsync(object ievent, Metadata metadata, CancellationToken cancellationToken) => Task.CompletedTask;
}