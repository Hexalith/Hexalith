// <copyright file="TaskProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Tasks;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions.Errors;
using Hexalith.Extensions.Helpers;

/// <summary>
/// The task processor.
/// </summary>
[DataContract]
public class TaskProcessor : ITaskProcessor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskProcessor" /> class.
    /// Initialize the task processor.
    /// </summary>
    public TaskProcessor()
    {
        Status = TaskProcessorStatus.New;
        History = new TaskProcessingHistory();
        ResiliencyPolicy = ResiliencyPolicy.None;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskProcessor"/> class.
    /// </summary>
    /// <param name="createdDate">The created date.</param>
    /// <param name="policy">The policy.</param>
    public TaskProcessor(DateTimeOffset createdDate, ResiliencyPolicy policy)
    {
        Status = TaskProcessorStatus.New;
        History = new TaskProcessingHistory(createdDate);
        ResiliencyPolicy = policy;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskProcessor" /> class.
    /// </summary>
    /// <param name="status">The processing status.</param>
    /// <param name="history">The processing history.</param>
    /// <param name="resiliencyPolicy">The retry policy.</param>
    /// <param name="failure">The processing failure information.</param>
    [JsonConstructor]
    public TaskProcessor(
            TaskProcessorStatus status,
            TaskProcessingHistory history,
            ResiliencyPolicy resiliencyPolicy,
            TaskProcessingFailure? failure)
    {
        Status = status;
        History = history;
        Failure = failure;
        ResiliencyPolicy = resiliencyPolicy;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskProcessor" /> class.
    /// Initialize a processor copy.
    /// </summary>
    /// <param name="processor">The processor to duplicate.</param>
    public TaskProcessor(ITaskProcessor processor)
    {
        _ = processor ?? throw new ArgumentNullException(nameof(processor));
        Status = processor.Status;
        History = processor.History;
        Failure = processor.Failure;
        ResiliencyPolicy = processor.ResiliencyPolicy;
    }

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public RetryStatus CanRetry =>
        ResiliencyPolicy.CanRetry(
              History.CreatedDate,
              Failure == null ? 0 : Failure.Count);

    /// <summary>
    /// Gets a value indicating whether gets the ended.
    /// </summary>
    /// <value>The ended.</value>
    [IgnoreDataMember]
    [JsonIgnore]
    public bool Ended
        => Status is TaskProcessorStatus.Completed or TaskProcessorStatus.Canceled;

    /// <inheritdoc/>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public TaskProcessingFailure? Failure
    {
        get;
        set;
    }

    /// <inheritdoc/>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public TaskProcessingHistory History
    {
        get;
        set;
    }

    /// <inheritdoc/>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public ResiliencyPolicy ResiliencyPolicy
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the retry date.
    /// </summary>
    /// <value>The retry wait time.</value>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    public DateTimeOffset? RetryDate
    {
        get => Ended ? null : ResiliencyPolicy.NextRetryTime(History.CreatedDate, Failure?.Count ?? 0);
        set
        {
            // Added for serialization purpose
        }
    }

    /// <summary>
    /// Gets or sets the retry wait time.
    /// </summary>
    /// <value>The retry wait time.</value>
    [DataMember(Order = 6)]
    [JsonPropertyOrder(6)]
    public TimeSpan RetryPeriod
    {
        get => ResiliencyPolicy.EvaluatePeriod(Failure?.Count ?? 0);
        set
        {
            // Added for serialization purpose
        }
    }

    /// <inheritdoc/>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TaskProcessorStatus Status
    {
        get;
        set;
    }

    /// <summary>
    /// Cancels this instance.
    /// </summary>
    /// <returns>Hexalith.Application.Abstractions.Tasks.TaskProcessor.</returns>
    /// <inheritdoc cref="ITaskProcessor.CancelTask" />
    public TaskProcessor Cancel()
    {
        return Ended
            ? this
            : new TaskProcessor(TaskProcessorStatus.Canceled, History.Canceled(), ResiliencyPolicy, Failure);
    }

    /// <inheritdoc/>
    public ITaskProcessor CancelTask() => Cancel();

    /// <summary>
    /// Completes this instance.
    /// </summary>
    /// <returns>Hexalith.Application.Abstractions.Tasks.TaskProcessor.</returns>
    /// <inheritdoc cref="ITaskProcessor.CompleteTask" />
    public TaskProcessor Complete()
    {
        return Ended
            ? this
            : new TaskProcessor(TaskProcessorStatus.Completed, History.Completed(), ResiliencyPolicy, Failure);
    }

    /// <inheritdoc/>
    public ITaskProcessor CompleteTask() => Complete();

    /// <summary>
    /// Continues this instance.
    /// </summary>
    /// <returns>Hexalith.Application.Abstractions.Tasks.TaskProcessor.</returns>
    /// <exception cref="InvalidStatusChangeException">currentStatus: Status, newStatus: TaskProcessorStatus.Active, Only suspended tasks can be continued.</exception>
    /// <exception cref="InvalidStatusChangeException">currentStatus: Status, newStatus: TaskProcessorStatus.Active, Cannot continue a task that has never been started.</exception>
    /// <inheritdoc cref="ITaskProcessor.ContinueTask" />
    public TaskProcessor Continue()
    {
        if (Status is not TaskProcessorStatus.Suspended)
        {
            throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Active, "Only suspended tasks can be continued.");
        }

        if (History.ProcessingStartDate == null)
        {
            throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Active, "Cannot continue a task that has never been started.");
        }

        if (Failure == null)
        {
            // The task has been manually suspended, so we can continue it without checking resiliency policy wait time.
            return new TaskProcessor(TaskProcessorStatus.Active, History, ResiliencyPolicy, null);
        }

        // Check if the can retry from resiliency policy
        RetryStatus status = ResiliencyPolicy.CanRetry(History.ProcessingStartDate.Value, Failure.Count);
        return status switch
        {
            RetryStatus.Enabled => new TaskProcessor(TaskProcessorStatus.Active, History, ResiliencyPolicy, Failure),
            RetryStatus.Suspended => this,
            RetryStatus.Stopped => new TaskProcessor(TaskProcessorStatus.Canceled, History.Canceled(), ResiliencyPolicy, Failure),
            _ => throw new InvalidStatusChangeException(Status, TaskProcessorStatus.Active, $"Invalid retry status : {status}"),
        };
    }

    /// <inheritdoc/>
    public ITaskProcessor ContinueTask() => Continue();

    /// <summary>
    /// Fails the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="technicalError">The technical error.</param>
    /// <returns>Hexalith.Application.Abstractions.Tasks.TaskProcessor.</returns>
    public TaskProcessor Fail(string message, string? technicalError)
    {
        TaskProcessingFailure newFailure = Failure == null ? new TaskProcessingFailure(1, DateTimeOffset.UtcNow, message, technicalError) : Failure.Fail(message, technicalError);
        bool canRetry = ResiliencyPolicy
            .CanRetry(History.ProcessingStartDate ?? History.CreatedDate, newFailure.Count) != RetryStatus.Stopped;
        return Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed
            ? this
            : canRetry
                ? new TaskProcessor(TaskProcessorStatus.Suspended, History.Suspended(), ResiliencyPolicy, newFailure)
                : new TaskProcessor(TaskProcessorStatus.Canceled, History.Canceled(), ResiliencyPolicy, newFailure);
    }

    /// <summary>
    /// Fails the specified ex.
    /// </summary>
    /// <param name="ex">The ex.</param>
    /// <returns>Hexalith.Application.Tasks.TaskProcessor.</returns>
    public TaskProcessor Fail([NotNull] ApplicationErrorException ex)
    {
        ArgumentNullException.ThrowIfNull(ex);
        string detailMessage = ex.Error == null
            ? "Application error"
            : ex.Error.GetDetailMessage(CultureInfo.InvariantCulture);
        string technicalMessage = (ex.Error == null
            ? ex.FullMessage()
            : ex.Error.GetTechnicalMessage(CultureInfo.InvariantCulture)) + "\nStack :\n" + ex.StackTrace;
        return Fail(detailMessage, technicalMessage);
    }

    /// <inheritdoc/>
    public ITaskProcessor FailTask(string message, string? technicalError) => Fail(message, technicalError);

    /// <summary>
    /// Starts this instance.
    /// </summary>
    /// <returns>Hexalith.Application.Abstractions.Tasks.TaskProcessor.</returns>
    /// <inheritdoc cref="ITaskProcessor.StartTask" />
    public TaskProcessor Start()
    {
        return Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed or TaskProcessorStatus.Active
            ? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Active, "Cannot start a terminated task.")
            : new TaskProcessor(TaskProcessorStatus.Active, History.ProcessingStarted(), ResiliencyPolicy, Failure);
    }

    /// <inheritdoc/>
    public ITaskProcessor StartTask() => Start();
}