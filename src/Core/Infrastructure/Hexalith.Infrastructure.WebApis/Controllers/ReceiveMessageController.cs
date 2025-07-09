// <copyright file="ReceiveMessageController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Controllers;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;

using Hexalith.Application.States;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Class ApplicationController.
/// Implements the <see cref="ControllerBase" />.
/// </summary>
/// <seealso cref="ControllerBase" />
[ApiController]
public abstract partial class ReceiveMessageController : ControllerBase
{
    /// <summary>
    /// The host environment.
    /// </summary>
    private readonly IHostEnvironment _hostEnvironment;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReceiveMessageController" /> class.
    /// </summary>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="logger">The logger.</param>
    protected ReceiveMessageController(
        IHostEnvironment hostEnvironment,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(hostEnvironment);
        ArgumentNullException.ThrowIfNull(logger);
        Logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    protected ILogger Logger { get; }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Received message {MessageName} with Id={MessageId}, AggregateKey={AggregateKey} and CorrelationId={CorrelationId}.")]
    public static partial void MessageReceivedInformation(
        ILogger logger,
        string? messageName,
        string? aggregateKey,
        string? messageId,
        string? correlationId);

    /// <summary>
    /// Messages the validation errors.
    /// </summary>
    /// <typeparam name="TMessageType">The type of the message type.</typeparam>
    /// <param name="messageState">State of the message.</param>
    /// <param name="aggregateType">Type of the aggregate.</param>
    /// <returns>System.Nullable&lt;ActionResult&gt;.</returns>
    protected BadRequestObjectResult? MessageValidation<TMessageType>(MessageState messageState, string aggregateType)
        where TMessageType : MessageState
    {
        BadRequestObjectResult? badRequest = messageState.Metadata == null
            ? messageState == null ? BadRequest("Invalid data : Message state received is null.") : BadRequest($"Invalid data : Message metadata received is null. CorrelationId={messageState.IdempotencyId}.")
            : messageState == null ? BadRequest("Invalid data : Message state received is null.") : string.IsNullOrWhiteSpace(messageState.Message)
            ? BadRequest($"Invalid data : Message received is empty. MessageName={messageState.Metadata.Message.Name}; MessageId={messageState.Metadata.Message.Id}; CorrelationId={messageState.IdempotencyId}.")
            : messageState.MessageObject == null
            ? BadRequest($"Invalid data : Message received is invalid. MessageName={messageState.Metadata.Message.Name}; MessageId={messageState.Metadata.Message.Id}; CorrelationId={messageState.IdempotencyId} : {messageState.Message}")
            : messageState.Metadata.Message.Aggregate.Name != aggregateType
            ? BadRequest($"Invalid data : The message aggregate is {messageState.Metadata.Message.Aggregate.Name}, but {aggregateType} was expected. MessageName={messageState.Metadata.Message.Name}; MessageId={messageState.Metadata.Message.Id}; CorrelationId={messageState.IdempotencyId}.")
            : messageState is not TMessageType
            ? BadRequest($"Invalid data : The message state received type is {messageState.GetType().Name}, but {typeof(TMessageType).Name} was expected. MessageName={messageState.Metadata.Message.Name}; MessageId={messageState.Metadata.Message.Id}; CorrelationId={messageState.IdempotencyId}.")
            : null;
        MessageReceivedInformation(
            Logger,
            messageState?.Metadata?.Message.Name ?? "Unknown",
            messageState?.Metadata?.AggregateGlobalId,
            messageState?.Metadata?.Message.Id,
            messageState?.IdempotencyId);
        return badRequest;
    }

    /// <summary>
    /// Problems the specified error.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <param name="exception">The exception.</param>
    /// <returns>ObjectResult.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    /// <exception cref="System.InvalidOperationException">Could not format the error message:\n" + error.Detail + "\n" + error.TechnicalDetail.</exception>
    protected ObjectResult Problem([NotNull] ApplicationError error, Exception? exception)
    {
        ArgumentNullException.ThrowIfNull(error);
        error.LogApplicationErrorDetails(Logger, exception);
        string detail;
        try
        {
            detail = _hostEnvironment.IsProduction()
                ? StringHelper.FormatWithNamedPlaceholders(
                    CultureInfo.InvariantCulture,
                    error.Detail ?? string.Empty,
                    error.Arguments)
                : StringHelper.FormatWithNamedPlaceholders(
                    CultureInfo.InvariantCulture,
                    error.TechnicalDetail ?? string.Empty,
                    error.TechnicalArguments);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Could not format the error message:\n" + error.Detail + "\n" + error.TechnicalDetail, ex);
        }

        return Problem(
            detail,
            _hostEnvironment.EnvironmentName,
            (int)HttpStatusCode.BadRequest,
            error.Title,
            error.Type?.ToString());
    }
}