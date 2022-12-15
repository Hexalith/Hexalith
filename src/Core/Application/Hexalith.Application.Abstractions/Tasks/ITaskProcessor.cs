// <copyright file="ITaskProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Tasks;

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
    ITaskProcessor Cancel();

    /// <summary>
    /// Complete the task.
    /// </summary>
    /// <returns>New copy of the task processor with status changed.</returns>
    ITaskProcessor Complete();

    /// <summary>
    /// Changes the status from suspended to active if the retry wait time has expired.
    /// </summary>
    /// <returns>New copy of the task processor with status changed.</returns>
    ITaskProcessor Continue();

    /// <summary>
    /// Set failure state and message.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>New copy of the task processor with status changed.</returns>
    ITaskProcessor Fail(string message);

    /// <summary>
    /// Start the task.
    /// </summary>
    /// <returns>New copy of the task processor with status changed.</returns>
    ITaskProcessor Start();
}