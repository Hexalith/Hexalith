// <copyright file="CommandServiceController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Controllers;

using Hexalith.Application.Commands;
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
    private readonly ICommandBus _commandBus;
    private readonly ILogger<CommandServiceController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandServiceController"/> class.
    /// </summary>
    /// <param name="commandBus">The command bus.</param>
    /// <param name="logger">The logger.</param>
    public CommandServiceController(ICommandBus commandBus, ILogger<CommandServiceController> logger)
    {
        ArgumentNullException.ThrowIfNull(commandBus);
        ArgumentNullException.ThrowIfNull(logger);
        _commandBus = commandBus;
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
        if (command is null)
        {
            return BadRequest("Command is null");
        }

        if (command.Message is null)
        {
            return BadRequest("Command message is null");
        }

        if (command.Metadata is null)
        {
            return BadRequest("Command metadata is null");
        }

        await _commandBus
            .PublishAsync(command.Message, command.Metadata, CancellationToken.None)
            .ConfigureAwait(false);

        LogCommandSubmittedDebugInformation(
            _logger,
            command.Metadata.Message.Id,
            command.Metadata.Context.CorrelationId,
            command.Metadata.Message.Name,
            command.Metadata.PartitionKey);

        return Ok();
    }

    [LoggerMessage(
      1,
      LogLevel.Debug,
      "Command {MessageType} on aggregate {PartitionKey} submitted. MessageId={MessageId}; CorrelationId={CorrelationId}.",
      EventName = "CommandSubmitted")]
    private static partial void LogCommandSubmittedDebugInformation(ILogger logger, string messageId, string correlationId, string messageType, string partitionKey);
}