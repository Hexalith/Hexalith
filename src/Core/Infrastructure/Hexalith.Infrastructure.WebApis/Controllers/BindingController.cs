// <copyright file="BindingController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Controllers;

using Ardalis.GuardClauses;

using Hexalith.Application.Abstractions.Errors;
using Hexalith.Application.Abstractions.Events;
using Hexalith.Domain.Abstractions.Events;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using SmartFormat;

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
    private readonly IIntegrationEventProcessor _dispatcher;

    private readonly IHostEnvironment _hostEnvironment;

    /// <summary>
    /// Initializes a new instance of the <see cref="BindingController"/> class.
    /// </summary>
    /// <param name="processor">The processor.</param>
    /// <param name="metadataValidator">The metadata validator.</param>
    /// <param name="logger">The logger.</param>
    protected BindingController(
        IIntegrationEventProcessor dispatcher,
        IHostEnvironment hostEnvironment,
        ILogger logger)
    {
        _dispatcher = Guard.Against.Null(dispatcher);
        Logger = Guard.Against.Null(logger);
        _hostEnvironment = Guard.Against.Null(hostEnvironment);
    }

    protected ILogger Logger { get; }

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
            await _dispatcher.SubmitAsync(@event, cancellationToken);
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

    protected ObjectResult Problem(Error error)
    {
        string detail = _hostEnvironment.IsProduction()
            ? Smart.Format(
                CultureInfo.InvariantCulture,
                error.Detail ?? string.Empty,
                error.Arguments)
            : Smart.Format(
                CultureInfo.InvariantCulture,
                error.TechnicalDetail ?? string.Empty,
                error.TechnicalArguments);

        return Problem(
            detail,
            "https://github.com/Hexalith/Hexalith/issues/",
            (int)HttpStatusCode.BadRequest,
            error.Title,
            error.Type);
    }
}
