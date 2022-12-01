// <copyright file="TaskProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Tasks;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// The task processor.
/// </summary>
[DataContract]
public class TaskProcessor : ITaskProcessor
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TaskProcessor"/> class.
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
	/// <param name="status">The processing status.</param>
	/// <param name="history">The processing history.</param>
	/// <param name="resiliencyPolicy">The retry policy.</param>
	/// <param name="failure">The processing failure information.</param>
	///
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
	/// Initializes a new instance of the <see cref="TaskProcessor"/> class.
	/// Initialize a processor copy.
	/// </summary>
	/// <param name="processor">The processor to duplicate.</param>
	private TaskProcessor(ITaskProcessor processor)
	{
		_ = processor ?? throw new ArgumentNullException(nameof(processor));
		Status = processor.Status;
		History = processor.History;
		Failure = processor.Failure;
		ResiliencyPolicy = processor.ResiliencyPolicy;
	}

	/// <inheritdoc/>
	public RetryStatus CanRetry =>
		ResiliencyPolicy.CanRetry(
			  History.CreatedDate,
			  Failure == null ? 0 : Failure.Count);

	/// <inheritdoc/>
	[DataMember(Order = 3)]
	[JsonPropertyOrder(3)]
	public TaskProcessingFailure? Failure { get; private set; }

	/// <inheritdoc/>
	[DataMember(Order = 2)]
	[JsonPropertyOrder(2)]
	public TaskProcessingHistory History { get; private set; }

	/// <inheritdoc/>
	[DataMember(Order = 4)]
	[JsonPropertyOrder(4)]
	public ResiliencyPolicy ResiliencyPolicy { get; private set; }

	/// <inheritdoc/>
	[DataMember(Order = 1)]
	[JsonPropertyOrder(1)]
	public TaskProcessorStatus Status { get; private set; }

	/// <inheritdoc cref="ITaskProcessor.Cancel"/>
	public TaskProcessor Cancel()
	{
		return (TaskProcessor)(Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed
			? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Canceled, "Cannot cancel a terminated task.")
			: (ITaskProcessor)new TaskProcessor(this)
			{
				History = History.Canceled(),
				Status = TaskProcessorStatus.Canceled,
			});
	}

	/// <inheritdoc cref="ITaskProcessor.Complete"/>
	public TaskProcessor Complete()
	{
		return (TaskProcessor)(Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed
			? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Completed, "Cannot complete a terminated task.")
			: (ITaskProcessor)new TaskProcessor(this)
			{
				History = History.Completed(),
				Status = TaskProcessorStatus.Completed,
			});
	}

	/// <inheritdoc cref="ITaskProcessor.Continue"/>
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
			return new TaskProcessor(this)
			{
				Status = TaskProcessorStatus.Active,
			};
		}

		// Check if the can retry from resiliency policy
		RetryStatus status = ResiliencyPolicy.CanRetry(History.ProcessingStartDate.Value, Failure.Count);
		return status switch
		{
			RetryStatus.Enabled => new TaskProcessor(this)
			{
				History = History,
				Status = TaskProcessorStatus.Active,
			},
			RetryStatus.Suspended => this,
			RetryStatus.Stopped => new TaskProcessor(this)
			{
				History = History.Canceled(),
				Status = TaskProcessorStatus.Canceled,
			},
			_ => throw new InvalidStatusChangeException(Status, TaskProcessorStatus.Active, $"Invalid retry status : {status}"),
		};
	}

	/// <inheritdoc cref="ITaskProcessor.Fail"/>
	public TaskProcessor Fail(string message)
	{
		TaskProcessingFailure newFailure = Failure == null ? new TaskProcessingFailure(1, DateTimeOffset.UtcNow, message) : Failure.Fail(message);
		return (TaskProcessor)(Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed
			? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Suspended, "Cannot fail a terminated task.")
			: (ITaskProcessor)new TaskProcessor(this)
			{
				History = History.Suspended(),
				Status = TaskProcessorStatus.Suspended,
				Failure = newFailure,
			});
	}

	/// <inheritdoc cref="ITaskProcessor.Start"/>
	public TaskProcessor Start()
	{
		return Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed or TaskProcessorStatus.Active
			? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Active, "Cannot start a terminated task.")
			: new TaskProcessor(this)
			{
				History = History.ProcessingStarted(),
				Status = TaskProcessorStatus.Active,
			};
	}

	/// <inheritdoc/>
	ITaskProcessor ITaskProcessor.Cancel()
	{
		return Cancel();
	}

	/// <inheritdoc/>
	ITaskProcessor ITaskProcessor.Complete()
	{
		return Complete();
	}

	/// <inheritdoc/>
	ITaskProcessor ITaskProcessor.Continue()
	{
		return Continue();
	}

	/// <inheritdoc/>
	ITaskProcessor ITaskProcessor.Fail(string message)
	{
		return Fail(message);
	}

	/// <inheritdoc/>
	ITaskProcessor ITaskProcessor.Start()
	{
		return Start();
	}
}