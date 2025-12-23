// <copyright file="ResilientProjectionEventProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Tasks;

using System;
using System.Threading;

using Hexalith.Application.Projections;
using Hexalith.Application.States;
using Hexalith.Commons.Errors;
using Hexalith.Commons.Metadatas;

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
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A <see cref="Task{TResult}" /> representing the result of the asynchronous operation.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Intentional")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S2221:\"Exception\" should not be caught", Justification = "Intentional")]
    public async Task<DateTimeOffset?> ProcessAsync(string id, object baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);
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
                        break;

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
            taskProcessor = taskProcessor.Fail(
                $"An error occurred when executing command {metadata.Message.Name} on {metadata.Message.Domain.Name}/{metadata.Message.Domain.Id}: {e.Message}", e.FullMessage());
        }

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