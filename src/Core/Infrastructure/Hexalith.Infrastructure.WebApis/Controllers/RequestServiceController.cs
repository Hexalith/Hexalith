// <copyright file="RequestServiceController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Controllers;

using Hexalith.Application.MessageMetadatas;
using Hexalith.Application.Requests;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a controller for handling requests.
/// </summary>
/// <remarks>
/// This controller is responsible for publishing requests asynchronously.
/// </remarks>
[ApiController]
[Route(ServicesRoutes.RequestService)]
public partial class RequestServiceController : ControllerBase
{
    private readonly ILogger<RequestServiceController> _logger;
    private readonly IRequestBus _requestBus;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestServiceController"/> class.
    /// </summary>
    /// <param name="requestBus">The request bus.</param>
    /// <param name="logger">The logger.</param>
    public RequestServiceController(
        IRequestBus requestBus,
        ILogger<RequestServiceController> logger)
    {
        ArgumentNullException.ThrowIfNull(requestBus);
        ArgumentNullException.ThrowIfNull(logger);
        _requestBus = requestBus;
        _logger = logger;
    }

    /// <summary>
    /// Submits a request asynchronously.
    /// </summary>
    /// <param name="request">The request to publish.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpPost(ServicesRoutes.SubmitRequest)]
    public async Task<ActionResult> SubmitRequestAsync(MessageState request)
    {
        if (request is null)
        {
            return BadRequest("Request is null");
        }

        if (request.Message is null)
        {
            return BadRequest("Request message is null");
        }

        if (request.Metadata is null)
        {
            return BadRequest("Request metadata is null");
        }

        await _requestBus
            .PublishAsync(request, CancellationToken.None)
            .ConfigureAwait(false);
        LogRequestSubmittedDebugInformation(
            _logger,
            request.Metadata.Message.Id,
            request.Metadata.Context.CorrelationId,
            request.Metadata.Message.Name,
            request.Metadata.Message.Aggregate.Name,
            request.Metadata.Message.Aggregate.Id);
        return Ok();
    }

    [LoggerMessage(
        1,
        LogLevel.Debug,
        "Request {MessageType} submitted. MessageId={MessageId}; CorrelationId={CorrelationId}; AggregateName={AggregateName}; AggregateId={AggregateId}.",
        EventName = "RequestSubmitted")]
    private static partial void LogRequestSubmittedDebugInformation(ILogger logger, string messageId, string correlationId, string messageType, string aggregateName, string aggregateId);
}