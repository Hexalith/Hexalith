// <copyright file="RequestServiceController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Controllers;

using System.Threading;

using Hexalith.Application.Modules.Applications;
using Hexalith.Application.Requests;
using Hexalith.Application.Services;
using Hexalith.Application.States;
using Hexalith.Applications.States;
using Hexalith.PolymorphicSerializations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a controller for handling service requests and aggregate operations.
/// </summary>
/// <remarks>
/// This controller provides endpoints for:
/// <list type="bullet">
/// <item><description>Submitting and processing asynchronous requests</description></item>
/// <item><description>Retrieving aggregate identifiers with pagination support</description></item>
/// </list>
/// </remarks>
[ApiController]
[Authorize]
[Route(ServicesRoutes.RequestService)]
public partial class RequestServiceController : ControllerBase
{
    private readonly IApplication _application;
    private readonly ILogger<RequestServiceController> _logger;
    private readonly IRequestProcessor _requestProcessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestServiceController"/> class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="requestProcessor">The request bus.</param>
    /// <param name="logger">The logger.</param>
    public RequestServiceController(IApplication application, IRequestProcessor requestProcessor, ILogger<RequestServiceController> logger)
    {
        ArgumentNullException.ThrowIfNull(application);
        ArgumentNullException.ThrowIfNull(requestProcessor);
        ArgumentNullException.ThrowIfNull(logger);
        _application = application;
        _requestProcessor = requestProcessor;
        _logger = logger;
    }

    /// <summary>
    /// Gets a collection of aggregate identifiers asynchronously.
    /// </summary>
    /// <param name="collectionFactory">The factory for creating ID collection services.</param>
    /// <param name="partitionId">The partition identifier to scope the aggregate IDs.</param>
    /// <param name="aggregateName">The name of the aggregate type to retrieve IDs for.</param>
    /// <param name="skip">The number of records to skip (for pagination).</param>
    /// <param name="take">The maximum number of records to return (for pagination). Use 0 for all records.</param>
    /// <returns>A <see cref="Task{TResult}"/> containing an <see cref="ActionResult{T}"/> of <see cref="IEnumerable{String}"/> representing the collection of aggregate identifiers.</returns>
    [HttpPost("aggregate/ids")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2360:Optional parameters should not be used", Justification = "Api interface")]
    public async Task<ActionResult<IEnumerable<string>>> GetAggregateIdsRequestAsync(
        [FromServices] IIdCollectionFactory collectionFactory,
        string partitionId,
        string aggregateName,
        int skip = 0,
        int take = 0)
    {
        IIdCollectionService service = collectionFactory.CreateService(IIdCollectionFactory.GetAggregateCollectionName(aggregateName), partitionId);
        IEnumerable<string> ids = [.. await service
                .GetAsync(skip, take, CancellationToken.None)
                .ConfigureAwait(false)];
        return Ok(ids);
    }

    /// <summary>
    /// Submits a request asynchronously.
    /// </summary>
    /// <param name="request">The request to publish.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpPost(ServicesRoutes.SubmitRequest)]
    public async Task<ActionResult<MessageState>> SubmitRequestAsync(MessageState request)
    {
        if (_application is not IApiServerApplication && HttpContext.User.Identity?.IsAuthenticated != true)
        {
            return Unauthorized("User is not authenticated");
        }

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

        Polymorphic result = (await _requestProcessor
            .ProcessAsync(request.MessageObject, request.Metadata, CancellationToken.None)
            .ConfigureAwait(false)) as Polymorphic
                ?? throw new InvalidOperationException("Request processor returned a null or invalid object.");

        LogRequestSubmittedDebugInformation(
            _logger,
            request.Metadata.Message.Id,
            request.Metadata.Context.CorrelationId,
            request.Metadata.Message.Name,
            request.Metadata.DomainGlobalId);

        return Ok(new MessageState(result, request.Metadata));
    }

    /// <summary>
    /// Logs debug information when a request is successfully submitted.
    /// </summary>
    /// <param name="logger">The logger instance to use for logging.</param>
    /// <param name="messageId">The unique identifier of the message.</param>
    /// <param name="correlationId">The correlation identifier for tracking related operations.</param>
    /// <param name="messageType">The type of the message being processed.</param>
    /// <param name="partitionKey">The partition key of the aggregate.</param>
    [LoggerMessage(
      1,
      LogLevel.Debug,
      "Request {MessageType} on aggregate {PartitionKey} submitted. MessageId={MessageId}; CorrelationId={CorrelationId}.",
      EventName = "RequestSubmitted")]
    private static partial void LogRequestSubmittedDebugInformation(ILogger logger, string messageId, string correlationId, string messageType, string partitionKey);
}