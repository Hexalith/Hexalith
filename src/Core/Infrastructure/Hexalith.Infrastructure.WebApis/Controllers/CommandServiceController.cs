// <copyright file="CommandServiceController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Controllers;

using FluentValidation;

using Hexalith.Application.Commands;
using Hexalith.Application.Modules.Applications;
using Hexalith.Application.States;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a controller for handling commands.
/// </summary>
/// <remarks>
/// This controller is responsible for publishing commands asynchronously.
/// </remarks>
[ApiController]
[Route(ServicesRoutes.CommandService)]
public partial class CommandServiceController : ControllerBase
{
    private readonly IApplication _application;
    private readonly IDomainCommandProcessor _commandProcessor;
    private readonly ILogger<CommandServiceController> _logger;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandServiceController"/> class.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="serviceProvider"></param>
    /// <param name="commandProcessor">The command bus.</param>
    /// <param name="logger">The logger.</param>
    public CommandServiceController(
        IApplication application,
        IServiceProvider serviceProvider,
        IDomainCommandProcessor commandProcessor,
        ILogger<CommandServiceController> logger)
    {
        ArgumentNullException.ThrowIfNull(application);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(commandProcessor);
        ArgumentNullException.ThrowIfNull(logger);
        _application = application;
        _serviceProvider = serviceProvider;
        _commandProcessor = commandProcessor;
        _logger = logger;
    }

    /// <summary>
    /// Submits a command asynchronously.
    /// </summary>
    /// <param name="command">The command to publish.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    [HttpPost(ServicesRoutes.SubmitCommand)]
    public async Task<ActionResult> PublishCommandAsync(MessageState command)
    {
        if (_application is not IApiServerApplication && HttpContext.User.Identity?.IsAuthenticated != true)
        {
            return Unauthorized("User is not authenticated");
        }

        if (command is null)
        {
            return BadRequest("Command is null");
        }

        if (string.IsNullOrWhiteSpace(command.Message))
        {
            return BadRequest("Command message is empty");
        }

        if (command.MessageObject is null)
        {
            return BadRequest("Command message is null");
        }

        if (command.Metadata is null)
        {
            return BadRequest("Command metadata is null");
        }

        // Get the IValidator<T> for the command type
        Type validatorType = typeof(IValidator<>).MakeGenericType(command.MessageObject.GetType());
        if (_serviceProvider.GetService(validatorType) is not IValidator validator)
        {
            LogMissingCommandValidatorWarning(_logger, command.MessageObject.GetType().Name);
        }
        else
        {
            FluentValidation.Results.ValidationResult validationResult = validator.Validate(new ValidationContext<object>(command.MessageObject));
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
        }

        await _commandProcessor.SubmitAsync(command.MessageObject, command.Metadata, CancellationToken.None)
            .ConfigureAwait(false);

        LogCommandSubmittedDebugInformation(
            _logger,
            command.Metadata.Message.Id,
            command.Metadata.Context.CorrelationId,
            command.Metadata.Message.Name,
            command.Metadata.AggregateGlobalId);

        return Ok();
    }

    [LoggerMessage(
      1,
      LogLevel.Debug,
      "Command {MessageType} on aggregate {PartitionKey} submitted. MessageId={MessageId}; CorrelationId={CorrelationId}.",
      EventName = "CommandSubmitted")]
    private static partial void LogCommandSubmittedDebugInformation(ILogger logger, string messageId, string correlationId, string messageType, string partitionKey);

    [LoggerMessage(
        0,
        LogLevel.Warning,
        "No validator found for command type {CommandType}.",
        EventName = "MissingCommandValidator")]
    private static partial void LogMissingCommandValidatorWarning(ILogger logger, string commandType);
}