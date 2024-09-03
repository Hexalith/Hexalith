// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-01-2023
// ***********************************************************************
// <copyright file="ResilientProjectionEventProcessor.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Tasks;

using System;
using System.Threading;

using Hexalith.Application.Projections;
using Hexalith.Application.States;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class ResilientEventProcessor.
/// </summary>
public class ResilientProjectionEventProcessor
{
    /// <summary>
    /// The projection.
    /// </summary>
    private readonly IProjection _projection;

    /// <summary>
    /// The resiliency policy.
    /// </summary>
    private readonly ResiliencyPolicy _resiliencyPolicy;

    /// <summary>
    /// The state store provider.
    /// </summary>
    private readonly IStateStoreProvider _stateStoreProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResilientProjectionEventProcessor" /> class.
    /// </summary>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="projection">The projection.</param>
    /// <param name="stateStoreProvider">The state store provider.</param>
    public ResilientProjectionEventProcessor(
        ResiliencyPolicy resiliencyPolicy,
        IProjection projection,
        IStateStoreProvider stateStoreProvider)
    {
        ArgumentNullException.ThrowIfNull(resiliencyPolicy);
        ArgumentNullException.ThrowIfNull(projection);
        ArgumentNullException.ThrowIfNull(stateStoreProvider);
        _resiliencyPolicy = resiliencyPolicy;
        _projection = projection;
        _stateStoreProvider = stateStoreProvider;
    }

    /// <summary>
    /// Process as an asynchronous operation.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="baseEvent">The command.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A <see cref="Task{TResult}" /> representing the result of the asynchronous operation.</returns>
    public async Task<DateTimeOffset?> ProcessAsync(string id, BaseEvent baseEvent, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
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
                        return taskProcessor.RetryDate;

                    case RetryStatus.Stopped:
                        taskProcessor = taskProcessor.Cancel();
                        return null;

                    default:
                        throw new NotSupportedException($"Task processor can retry option {taskProcessor.CanRetry} not supported.");
                }

                break;

            case TaskProcessorStatus.Active:
                break;

            case TaskProcessorStatus.Canceled:
            case TaskProcessorStatus.Completed:
                return null;

            default:
                throw new NotSupportedException($"Task processor status option {taskProcessor.Status} not supported.");
        }

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            if (taskProcessor.Status == TaskProcessorStatus.Active)
            {
                await _projection.HandleAsync(baseEvent, cancellationToken).ConfigureAwait(false);
                taskProcessor = taskProcessor.Complete();
            }
        }
        catch (Exception e)
        {
            taskProcessor = taskProcessor.Fail($"An error occurred when executing command {baseEvent.TypeName} on {baseEvent.AggregateName}/{baseEvent.AggregateId}: {e.Message}", e.FullMessage());
        }
#pragma warning restore CA1031 // Do not catch general exception types

        await _stateStoreProvider.SetStateAsync(nameof(TaskProcessor) + id, taskProcessor, cancellationToken).ConfigureAwait(false);
        return (taskProcessor.Status is TaskProcessorStatus.Completed or TaskProcessorStatus.Canceled) ? null : taskProcessor.RetryDate;
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