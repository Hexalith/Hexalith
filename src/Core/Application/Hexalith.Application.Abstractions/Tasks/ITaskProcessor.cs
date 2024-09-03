// <copyright file="ITaskProcessor.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Tasks;

/// <summary>
/// The task processor.
/// </summary>
public interface ITaskProcessor
{
    /// <summary>
    /// Gets a value indicating whether we can retry.
    /// </summary>
    /// <returns>true if total wait time is expired, else false.</returns>
    RetryStatus CanRetry { get; }

    /// <summary>
    /// Gets the task processing failure.
    /// </summary>
    TaskProcessingFailure? Failure { get; }

    /// <summary>
    /// Gets the task processing history.
    /// </summary>
    TaskProcessingHistory History { get; }

    /// <summary>
    /// Gets retry policy.
    /// </summary>
    ResiliencyPolicy ResiliencyPolicy { get; }

    /// <summary>
    /// Gets the task processor status.
    /// </summary>
    TaskProcessorStatus Status { get; }

    /// <summary>
    /// Cancel the task.
    /// </summary>
    /// <returns>New copy of the task processor with status changed.</returns>
    ITaskProcessor CancelTask();

    /// <summary>
    /// Complete the task.
    /// </summary>
    /// <returns>New copy of the task processor with status changed.</returns>
    ITaskProcessor CompleteTask();

    /// <summary>
    /// Changes the status from suspended to active if the retry wait time has expired.
    /// </summary>
    /// <returns>New copy of the task processor with status changed.</returns>
    ITaskProcessor ContinueTask();

    /// <summary>
    /// Fails the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="technicalError">The technical error.</param>
    /// <returns>ITaskProcessor.</returns>
    ITaskProcessor FailTask(string message, string? technicalError);

    /// <summary>
    /// Start the task.
    /// </summary>
    /// <returns>New copy of the task processor with status changed.</returns>
    ITaskProcessor StartTask();
}