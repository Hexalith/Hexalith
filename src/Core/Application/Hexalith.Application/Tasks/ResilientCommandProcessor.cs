// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-29-2023
// ***********************************************************************
// <copyright file="ResilientCommandProcessor.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Tasks;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Hexalith.Application.Commands;
using Hexalith.Application.States;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

using Microsoft.Extensions.Logging;

#pragma warning disable CA1031 // Do not catch general exception types

/// <summary>
/// Class ResilientCommandProcessor.
/// </summary>
public partial class ResilientCommandProcessor
{
    /// <summary>
    /// The command dispatcher.
    /// </summary>
    private readonly ICommandDispatcher _commandDispatcher;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// The resiliency policy.
    /// </summary>
    private readonly ResiliencyPolicy _resiliencyPolicy;

    /// <summary>
    /// The state store provider.
    /// </summary>
    private readonly IStateStoreProvider _stateStoreProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResilientCommandProcessor" /> class.
    /// </summary>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="commandDispatcher">The command dispatcher.</param>
    /// <param name="stateStoreProvider">The state store provider.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public ResilientCommandProcessor(
        ResiliencyPolicy resiliencyPolicy,
        ICommandDispatcher commandDispatcher,
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

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "An error occurred when executing command {CommandType} on {AggregateName}/{AggregateId}: {Message}")]
    public partial void LogCommandExecutionError(Exception e, string commandType, string aggregateName, string aggregateId, string message);

    /// <summary>
    /// Process as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="command">The command.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.ValueTuple&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public async Task<(TaskProcessor Processor, IEnumerable<BaseMessage> Events)> ProcessAsync(string id, [NotNull] BaseCommand command, IDomainAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        IEnumerable<BaseMessage> messages;
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
                        return (taskProcessor, Array.Empty<BaseMessage>());

                    case RetryStatus.Stopped:
                        taskProcessor = taskProcessor.Cancel();
                        return (taskProcessor, [new CommandProcessingFailed(string.Empty, command, taskProcessor)]);

                    default:
                        throw new NotSupportedException($"Task processor can retry option {taskProcessor.CanRetry} not supported.");
                }

                break;

            case TaskProcessorStatus.Active:
                break;

            case TaskProcessorStatus.Canceled:
                return (taskProcessor, [new CommandProcessingFailed(string.Empty, command, taskProcessor)]);

            case TaskProcessorStatus.Completed:
                return (taskProcessor, Array.Empty<BaseMessage>());

            default:
                throw new NotSupportedException($"Task processor status option {taskProcessor.Status} not supported.");
        }

        try
        {
            if (taskProcessor.Status == TaskProcessorStatus.Active)
            {
                messages = await _commandDispatcher.DoAsync(command, aggregate, cancellationToken).ConfigureAwait(false);
                taskProcessor = taskProcessor.Complete();
            }
            else
            {
                messages = [new CommandProcessingFailed(string.Empty, command, taskProcessor)];
            }
        }
        catch (Exception e)
        {
            taskProcessor = taskProcessor.Fail($"An error occurred when executing command {command.TypeName} on {command.AggregateName}/{command.AggregateId}: {e.Message}", e.FullMessage());
            messages = [new CommandProcessingFailed(string.Empty, command, taskProcessor)];
            LogCommandExecutionError(e, command.TypeName, command.AggregateName, command.AggregateId, e.FullMessage());
        }

        await _stateStoreProvider.SetStateAsync(nameof(TaskProcessor) + id, taskProcessor, cancellationToken).ConfigureAwait(false);

        return (taskProcessor, messages);
    }

    /// <summary>
    /// Get task processor as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;TaskProcessor&gt; representing the asynchronous operation.</returns>
    private async Task<TaskProcessor> GetTaskProcessorAsync(string id, CancellationToken cancellationToken)
    {
        ConditionalValue<TaskProcessor> result = await _stateStoreProvider
            .TryGetStateAsync<TaskProcessor>(
                nameof(TaskProcessor) + id,
                cancellationToken).ConfigureAwait(false);
        return result.HasValue ? result.Value : new TaskProcessor(DateTimeOffset.UtcNow, _resiliencyPolicy);
    }
}