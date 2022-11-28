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
	[JsonConstructor]
	public TaskProcessor()
	{
		CreatedDate = DateTimeOffset.UtcNow;
		Status = TaskProcessorStatus.New;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TaskProcessor"/> class.
	/// Initialize a processor copy.
	/// </summary>
	/// <param name="processor">The processor to duplicate.</param>
	public TaskProcessor(ITaskProcessor processor)
	{
		_ = processor ?? throw new ArgumentNullException(nameof(processor));
		CanceledDate = processor.CanceledDate;
		CompletedDate = processor.CompletedDate;
		CreatedDate = processor.CreatedDate;
		ProcessingStartDate = processor.ProcessingStartDate;
		Status = processor.Status;
		SuspendedDate = processor.SuspendedDate;
		SuspendedUntilDate = processor.SuspendedUntilDate;
	}

	/// <inheritdoc/>
	public DateTimeOffset? CanceledDate { get; private set; }

	/// <inheritdoc/>
	public DateTimeOffset? CompletedDate { get; private set; }

	/// <inheritdoc/>
	public DateTimeOffset CreatedDate { get; private set; }

	/// <inheritdoc/>
	public bool IsSuspensionExpired => SuspendedUntilDate == null || SuspendedUntilDate.Value.ToUniversalTime() <= DateTimeOffset.UtcNow;

	/// <inheritdoc/>
	public DateTimeOffset? ProcessingStartDate { get; private set; }

	/// <inheritdoc/>
	public TaskProcessorStatus Status { get; private set; }

	/// <inheritdoc/>
	public DateTimeOffset? SuspendedDate { get; private set; }

	/// <inheritdoc/>
	public DateTimeOffset? SuspendedUntilDate { get; private set; }

	/// <inheritdoc/>
	public ITaskProcessor Cancel()
	{
		return Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed
			? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Canceled)
			: (ITaskProcessor)new TaskProcessor(this)
			{
				CanceledDate = DateTimeOffset.UtcNow,
				Status = TaskProcessorStatus.Canceled,
			};
	}

	/// <inheritdoc/>
	public ITaskProcessor Complete()
	{
		return Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed
			? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Completed)
			: (ITaskProcessor)new TaskProcessor(this)
			{
				CompletedDate = DateTimeOffset.UtcNow,
				Status = TaskProcessorStatus.Completed,
			};
	}

	/// <inheritdoc/>
	public ITaskProcessor Start()
	{
		return Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed or TaskProcessorStatus.Processing
			? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Processing)
			: (ITaskProcessor)new TaskProcessor(this)
			{
				ProcessingStartDate = DateTimeOffset.UtcNow,
				Status = TaskProcessorStatus.Processing,
			};
	}

	/// <inheritdoc/>
	public ITaskProcessor SuspendUntil(DateTimeOffset date)
	{
		return Status is TaskProcessorStatus.Canceled or TaskProcessorStatus.Completed
			? throw new InvalidStatusChangeException(currentStatus: Status, newStatus: TaskProcessorStatus.Completed)
			: (ITaskProcessor)new TaskProcessor(this)
			{
				SuspendedDate = DateTimeOffset.UtcNow,
				SuspendedUntilDate = date,
				Status = TaskProcessorStatus.Suspended,
			};
	}
}