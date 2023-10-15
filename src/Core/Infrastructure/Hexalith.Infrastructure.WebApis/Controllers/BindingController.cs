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

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Text.Json;

using Hexalith.Application.Errors;
using Hexalith.Application.Events;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
        ArgumentNullException.ThrowIfNull(eventProcessor);
        ArgumentNullException.ThrowIfNull(hostEnvironment);
        ArgumentNullException.ThrowIfNull(logger);
        _eventProcessor = eventProcessor;
        Logger = logger;
        _hostEnvironment = hostEnvironment;
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
                @event.TypeName,
                @event.AggregateName,
                @event.AggregateId);
            await _eventProcessor.SubmitAsync(@event, cancellationToken).ConfigureAwait(false);
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
    protected ObjectResult Problem([NotNull] ApplicationError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        string detail;
        try
        {
            detail = _hostEnvironment.IsProduction()
                ? StringHelper.FormatWithNamedPlaceholders(
                    CultureInfo.InvariantCulture,
                    error.Detail ?? string.Empty,
                    error.Arguments?.ToArray() ?? [])
                : StringHelper.FormatWithNamedPlaceholders(
                    CultureInfo.InvariantCulture,
                    error.TechnicalDetail ?? string.Empty,
                    error.TechnicalArguments?.ToArray() ?? []);
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
            error.Type?.ToString());
    }
}