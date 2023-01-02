// <copyright file="Dynamics365FinanceEventBindingController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Controllers;

using Ardalis.GuardClauses;

using FluentValidation;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.BusinessEvents;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Runtime.Serialization;
using System.Text.Json;

/// <summary>
/// Class Dynamics365FinanceEventBindingController.
/// Implements the <see cref="ControllerBase" />.
/// </summary>
/// <seealso cref="ControllerBase" />
public abstract class Dynamics365FinanceEventBindingController : ControllerBase
{
    /// <summary>
    /// The dispatcher.
    /// </summary>
    private readonly ICommandDispatcher _dispatcher;

    /// <summary>
    /// The event validator.
    /// </summary>
    private readonly IValidator<Dynamics365BusinessEventBase> _eventValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceEventBindingController" /> class.
    /// </summary>
    /// <param name="dispatcher">The dispatcher.</param>
    /// <param name="metadataValidator">The metadata validator.</param>
    /// <param name="logger">The logger.</param>
    protected Dynamics365FinanceEventBindingController(
        ICommandDispatcher dispatcher,
        IValidator<Dynamics365BusinessEventBase> metadataValidator,
        ILogger logger)
    {
        _dispatcher = Guard.Against.Null(dispatcher);
        _eventValidator = Guard.Against.Null(metadataValidator);
        Logger = Guard.Against.Null(logger);
    }

    protected ILogger Logger { get; }

    /// <summary>
    /// Handle business event as an asynchronous operation.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    protected async Task<ActionResult> HandleBusinessEventAsync(JsonElement message, CancellationToken cancellationToken)
    {
        try
        {
            Dynamics365BusinessEventBase businessEvent = Dynamics365BusinessEventBase.AddTypeAndDeserialize(message);

            await _eventValidator.ValidateAndThrowAsync(businessEvent);
            Logger.LogInformation(
                "Received business event {BusinessEventId} with Id='{EventId}'.",
                businessEvent.BusinessEventId,
                businessEvent.EventId);
            await _dispatcher.DoAsync(businessEvent.ToCommand(), cancellationToken);
            return Accepted();
        }
        catch (ValidationException ex)
        {
            Logger.LogError(
                ex,
                "Invalid Dynamics 365 for finance business event:\n",
                message.GetRawText());
            return BadRequest(ex.Errors);
        }
        catch (SerializationException ex)
        {
            Logger.LogError(
                ex,
                "Invalid Dynamics 365 for finance business event:\n",
                message.GetRawText());
            return BadRequest(ex.FullMessage());
        }
    }
}
