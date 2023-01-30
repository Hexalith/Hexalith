// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-30-2023
// ***********************************************************************
// <copyright file="ResilientCommandProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Abstractions.Tasks;

using System;
using System.Threading;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.States;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class ResilientCommandProcessor.
/// </summary>
public class ResilientCommandProcessor
{
    /// <summary>
    /// The command dispatcher.
    /// </summary>
    private readonly ICommandDispatcher _commandDispatcher;

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
    public ResilientCommandProcessor(
        ResiliencyPolicy resiliencyPolicy,
        ICommandDispatcher commandDispatcher,
        IStateStoreProvider stateStoreProvider)
    {
        _resiliencyPolicy = resiliencyPolicy;
        _commandDispatcher = commandDispatcher;
        _stateStoreProvider = stateStoreProvider;
    }

    /// <summary>
    /// Process as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A <see cref="Task{TResult}" /> representing the result of the asynchronous operation.</returns>
    public async Task<(bool Completed, IEnumerable<BaseEvent> Events)> ProcessAsync(string id, BaseCommand command, CancellationToken cancellationToken)
    {
        IEnumerable<BaseEvent> events;
        TaskProcessor taskProcessor = await GetTaskProcessorAsync(id, cancellationToken);
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
                        return (false, Array.Empty<BaseEvent>());

                    case RetryStatus.Stopped:
                        taskProcessor = taskProcessor.Cancel();
                        break;
                }

                break;

            case TaskProcessorStatus.Active:
                break;

            case TaskProcessorStatus.Canceled:
            case TaskProcessorStatus.Completed:
                return (true, Array.Empty<BaseEvent>());
        }

        try
        {
            if (taskProcessor.Status == TaskProcessorStatus.Active)
            {
                events = await _commandDispatcher.DoAsync(command, cancellationToken);
                taskProcessor = taskProcessor.Complete();
            }
            else
            {
                events = new CommandProcessingFailed(command, taskProcessor).IntoArray();
            }
        }
        catch (Exception e)
        {
            taskProcessor = taskProcessor.Fail(e.FullMessage());
            events = new CommandProcessingFailed(command, taskProcessor).IntoArray();
        }

        await _stateStoreProvider.SetStateAsync(nameof(TaskProcessor) + id, taskProcessor, cancellationToken);
        return (taskProcessor.Status is TaskProcessorStatus.Completed or TaskProcessorStatus.Canceled, events);
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
                cancellationToken);
        return result.HasValue ? result.Value : new TaskProcessor(_resiliencyPolicy);
    }
}