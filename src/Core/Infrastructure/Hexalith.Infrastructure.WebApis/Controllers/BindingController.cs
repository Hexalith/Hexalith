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

using Ardalis.GuardClauses;

using Hexalith.Application.Abstractions.Errors;
using Hexalith.Application.Abstractions.Events;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Globalization;
using System.Net;
using System.Text.Json;

/// <summary>
/// Class Dynamics365FinanceEventBindingController.
/// Implements the <see cref="ControllerBase" />.
/// </summary>
/// <seealso cref="ControllerBase" />
[ApiController]
public abstract class BindingController : ControllerBase
{
    /// <summary>
    /// The processor.
    /// </summary>
    private readonly IIntegrationEventProcessor _eventProcessor;

    /// <summary>
    /// The host environment.
    /// </summary>
    private readonly IHostEnvironment _hostEnvironment;

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
    {
        _eventProcessor = Guard.Against.Null(eventProcessor);
        Logger = Guard.Against.Null(logger);
        _hostEnvironment = Guard.Against.Null(hostEnvironment);
    }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    protected ILogger Logger { get; }

    /// <summary>
    /// Deserializes the and validate.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>IEvent.</returns>
    protected abstract IEvent DeserializeAndValidate(JsonElement message);

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
            IEvent @event = DeserializeAndValidate(message);
            Logger.LogInformation(
                "Received event {EventName} {AggregateName}-{AggregateId}.",
                @event.MessageName,
                @event.AggregateName,
                @event.AggregateId);
            await _eventProcessor.SubmitAsync(@event, cancellationToken);
            return Accepted();
        }
        catch (ApplicationErrorException ex)
        {
            if (ex.Error is not null)
            {
                return Problem(ex.Error);
            }

            Logger.LogError(
                ex,
                "Event dispatch error.\n{SerializationData}",
                message.GetRawText());
            throw;
        }
    }

    /// <summary>
    /// Problems the specified error.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>ObjectResult.</returns>
    protected ObjectResult Problem(Error error)
    {
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
            "https://github.com/Hexalith/Hexalith/issues/",
            (int)HttpStatusCode.BadRequest,
            error.Title,
            error.Type);
    }
}