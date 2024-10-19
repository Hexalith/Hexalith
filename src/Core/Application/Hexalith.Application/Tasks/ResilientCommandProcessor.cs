// <copyright file="ResilientCommandProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Tasks;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Aggregates;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a resilient command processor that handles command execution with retry policies and error handling.
/// This class is responsible for managing the lifecycle of command processing, including starting, suspending, and completing tasks.
/// </summary>
public partial class ResilientCommandProcessor
{
    /// <summary>
    /// The command dispatcher used to execute domain commands.
    /// </summary>
    private readonly IDomainCommandDispatcher _commandDispatcher;

    /// <summary>
    /// The logger used for logging errors and other important information.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// The resiliency policy that defines retry behavior and other resilience strategies.
    /// </summary>
    private readonly ResiliencyPolicy _resiliencyPolicy;

    /// <summary>
    /// The state store provider used to persist and retrieve task processor states.
    /// </summary>
    private readonly IStateStoreProvider _stateStoreProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResilientCommandProcessor" /> class.
    /// </summary>
    /// <param name="resiliencyPolicy">The resiliency policy defining retry behavior.</param>
    /// <param name="commandDispatcher">The command dispatcher for executing domain commands.</param>
    /// <param name="stateStoreProvider">The state store provider for persisting task states.</param>
    /// <param name="logger">The logger for error and information logging.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when any of the parameters are null.</exception>
    public ResilientCommandProcessor(
        ResiliencyPolicy resiliencyPolicy,
        IDomainCommandDispatcher commandDispatcher,
        IStateStoreProvider stateStoreProvider,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(resiliencyPolicy);
        ArgumentNullException.ThrowIfNull(commandDispatcher);
        ArgumentNullException.ThrowIfNull(stateStoreProvider);
        ArgumentNullException.ThrowIfNull(logger);
        _resiliencyPolicy = resiliencyPolicy;
        _commandDispatcher = commandDispatcher;
        _stateStoreProvider = stateStoreProvider;
        _logger = logger;
    }

    /// <summary>
    /// Logs an error that occurred during command execution.
    /// </summary>
    /// <param name="e">The exception that was thrown.</param>
    /// <param name="commandType">The type of the command being executed.</param>
    /// <param name="aggregateName">The name of the aggregate.</param>
    /// <param name="aggregateId">The ID of the aggregate.</param>
    /// <param name="message">The error message.</param>
    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "An error occurred when executing command {CommandType} on {AggregateName}/{AggregateId}: {Message}")]
    public partial void LogCommandExecutionError(Exception e, string commandType, string aggregateName, string aggregateId, string message);

    /// <summary>
    /// Processes a command asynchronously with resilience and error handling.
    /// </summary>
    /// <param name="id">The unique identifier for this processing task.</param>
    /// <param name="command">The command to be processed.</param>
    /// <param name="metadata">The metadata associated with the command.</param>
    /// <param name="aggregate">The domain aggregate, if any, associated with the command.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A tuple containing the updated TaskProcessor and the ExecuteCommandResult, if successful.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="System.NotSupportedException">Thrown when an unsupported task processor status or retry status is encountered.</exception>
    public async Task<(TaskProcessor Processor, ExecuteCommandResult? Result)> ProcessAsync(string id, [NotNull] object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ExecuteCommandResult? result = null;
        TaskProcessor taskProcessor = await GetTaskProcessorAsync(id, cancellationToken).ConfigureAwait(false);
        switch (taskProcessor.Status)
        {
            case TaskProcessorStatus.New:
                taskProcessor = taskProcessor.Start();
                break;

            case TaskProcessorStatus.Suspended:
                switch (taskProcessor.CanRetry)
                {
                    case RetryStatus.Enabled:
                        taskProcessor = taskProcessor.Continue();
                        break;

                    case RetryStatus.Suspended:
                        return (taskProcessor, null);

                    case RetryStatus.Stopped:
                        taskProcessor = taskProcessor.Cancel();
                        break;

                    default:
                        throw new NotSupportedException($"Task processor can retry option {taskProcessor.CanRetry} not supported.");
                }

                break;

            case TaskProcessorStatus.Active:
                break;

            case TaskProcessorStatus.Canceled:
                return (taskProcessor, null);

            case TaskProcessorStatus.Completed:
                return (taskProcessor, null);

            default:
                throw new NotSupportedException($"Task processor status option {taskProcessor.Status} not supported.");
        }

        if (taskProcessor.Status == TaskProcessorStatus.Active)
        {
            try
            {
                result = await _commandDispatcher.DoAsync(command, metadata, aggregate, cancellationToken).ConfigureAwait(false);
                taskProcessor = taskProcessor.Complete();
            }
            catch (Exception e)
            {
                taskProcessor = taskProcessor.Fail($"An error occurred when executing command {metadata.Message.Name} on {metadata.Message.Aggregate.Name}/{metadata.Message.Aggregate.Id}: {e.Message}", e.FullMessage());
                LogCommandExecutionError(e, metadata.Message.Name, metadata.Message.Aggregate.Name, metadata.Message.Aggregate.Id, e.FullMessage());
            }
        }

        await _stateStoreProvider.SetStateAsync(nameof(TaskProcessor) + id, taskProcessor, cancellationToken).ConfigureAwait(false);
        return (taskProcessor, result);
    }

    /// <summary>
    /// Retrieves or creates a TaskProcessor for the given ID asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier for the TaskProcessor.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>The retrieved or newly created TaskProcessor.</returns>
    private async Task<TaskProcessor> GetTaskProcessorAsync(string id, CancellationToken cancellationToken)
    {
        ConditionalValue<TaskProcessor> result = await _stateStoreProvider
            .TryGetStateAsync<TaskProcessor>(
                nameof(TaskProcessor) + id,
                cancellationToken).ConfigureAwait(false);
        return result.HasValue ? result.Value : new TaskProcessor(DateTimeOffset.UtcNow, _resiliencyPolicy);
    }
}