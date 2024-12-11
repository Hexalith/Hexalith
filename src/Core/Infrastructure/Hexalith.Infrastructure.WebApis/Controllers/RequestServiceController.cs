// <copyright file="RequestServiceController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Controllers;

using Hexalith.Application.Requests;
using Hexalith.Application.States;
using Hexalith.PolymorphicSerialization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a controller for handling requests.
/// </summary>
/// <remarks>
/// This controller is responsible for publishing requests asynchronously.
/// </remarks>
[ApiController]
[Authorize]
[Route(ServicesRoutes.RequestService)]
public partial class RequestServiceController : ControllerBase
{
    private readonly ILogger<RequestServiceController> _logger;
    private readonly IRequestProcessor _requestProcessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestServiceController"/> class.
    /// </summary>
    /// <param name="requestProcessor">The request bus.</param>
    /// <param name="logger">The logger.</param>
    public RequestServiceController(IRequestProcessor requestProcessor, ILogger<RequestServiceController> logger)
    {
        ArgumentNullException.ThrowIfNull(requestProcessor);
        ArgumentNullException.ThrowIfNull(logger);
        _requestProcessor = requestProcessor;
        _logger = logger;
    }

    /// <summary>
    /// Submits a request asynchronously.
    /// </summary>
    /// <param name="request">The request to publish.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpPost(ServicesRoutes.SubmitRequest)]
    public async Task<ActionResult<MessageState>> PublishRequestAsync(MessageState request)
    {
        if (request is null)
        {
            return BadRequest("Request is null");
        }

        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("Request message is empty");
        }

        if (request.MessageObject is null)
        {
            return BadRequest("Request message is null");
        }

        if (request.Metadata is null)
        {
            return BadRequest("Request metadata is null");
        }

        PolymorphicRecordBase result = (await _requestProcessor
            .ProcessAsync(request.MessageObject, request.Metadata, CancellationToken.None)
            .ConfigureAwait(false)) as PolymorphicRecordBase
                ?? throw new InvalidOperationException("Request processor returned a null or invalid object.");

        LogRequestSubmittedDebugInformation(
            _logger,
            request.Metadata.Message.Id,
            request.Metadata.Context.CorrelationId,
            request.Metadata.Message.Name,
            request.Metadata.AggregateGlobalId);

        return Ok(new MessageState(result, request.Metadata));
    }

    [LoggerMessage(
      1,
      LogLevel.Debug,
      "Request {MessageType} on aggregate {PartitionKey} submitted. MessageId={MessageId}; CorrelationId={CorrelationId}.",
      EventName = "RequestSubmitted")]
    private static partial void LogRequestSubmittedDebugInformation(ILogger logger, string messageId, string correlationId, string messageType, string partitionKey);
}