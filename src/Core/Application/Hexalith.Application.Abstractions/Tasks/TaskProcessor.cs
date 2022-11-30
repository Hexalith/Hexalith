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
		RetryPolicy = RetryPolicy.None;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TaskProcessor"/> class.
	/// </summary>
	/// <param name="status">The processing status.</param>
	/// <param name="history">The processing history.</param>
	/// <param name="retryPolicy">The retry policy.</param>
	/// <param name="failure">The processing failure information.</param>
	///
	[JsonConstructor]
	public TaskProcessor(
			TaskProcessorStatus status,
			TaskProcessingHistory history,
			RetryPolicy retryPolicy,
			TaskProcessingFailure? failure)
	{
		Status = status;
		History = history;
		Failure = failure;
		RetryPolicy = retryPolicy;
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
		RetryPolicy = processor.RetryPolicy;
	}

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
	public RetryPolicy RetryPolicy { get; private set; }

	/// <inheritdoc/>
	[DataMember(Order = 1)]
	[JsonPropertyOrder(1)]
	public TaskProcessorStatus Status { get; private set; }

	/// <inheritdoc cref="ITaskProcessor.Cancel"/>
	public TaskProcessor Cancel()
	{
		return (TaskProcessor)(Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed
			? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Canceled)
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
			? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Completed)
			: (ITaskProcessor)new TaskProcessor(this)
			{
				History = History.Completed(),
				Status = TaskProcessorStatus.Completed,
			});
	}

	/// <inheritdoc cref="ITaskProcessor.Fail"/>
	public TaskProcessor Fail(string message)
	{
		TaskProcessingFailure newFailure = Failure == null ? new TaskProcessingFailure(1, DateTimeOffset.UtcNow, message) : Failure.Fail(message);
		return (TaskProcessor)(Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed
			? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Suspended)
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
		return (TaskProcessor)(Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed or TaskProcessorStatus.Active
			? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Active)
			: (ITaskProcessor)new TaskProcessor(this)
			{
				History = History.ProcessingStarted(),
				Status = TaskProcessorStatus.Active,
			});
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