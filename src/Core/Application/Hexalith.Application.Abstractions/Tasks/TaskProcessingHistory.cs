// <copyright file="TaskProcessingHistory.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Tasks;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// The task processing history information.
/// </summary>
[DataContract]
public class TaskProcessingHistory
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TaskProcessingHistory"/> class.
	/// </summary>
	public TaskProcessingHistory()
	{
		CreatedDate = DateTimeOffset.UtcNow;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TaskProcessingHistory"/> class.
	/// </summary>
	/// <param name="createdDate">The created date.</param>
	/// <param name="suspendedDate">The last suspension date.</param>
	/// <param name="processingStartDate">The last processing date.</param>
	/// <param name="completedDate">The completed date.</param>
	/// <param name="canceledDate">The canceled date.</param>
	[JsonConstructor]
	public TaskProcessingHistory(
		DateTimeOffset createdDate,
		DateTimeOffset? suspendedDate,
		DateTimeOffset? processingStartDate,
		DateTimeOffset? completedDate,
		DateTimeOffset? canceledDate)
	{
		CreatedDate = createdDate;
		SuspendedDate = suspendedDate;
		ProcessingStartDate = processingStartDate;
		CompletedDate = completedDate;
		CanceledDate = canceledDate;
	}

	/// <summary>
	/// Gets the task canceled date.
	/// </summary>
	public DateTimeOffset? CanceledDate { get; private set; }

	/// <summary>
	/// Gets the task completed date.
	/// </summary>
	public DateTimeOffset? CompletedDate { get; private set; }

	/// <summary>
	/// Gets the task created date.
	/// </summary>
	public DateTimeOffset CreatedDate { get; private set; }

	/// <summary>
	/// Gets the task processing start date.
	/// </summary>
	public DateTimeOffset? ProcessingStartDate { get; private set; }

	/// <summary>
	/// Gets the task suspended date.
	/// </summary>
	public DateTimeOffset? SuspendedDate { get; private set; }

	/// <summary>
	/// Gets a canceled processing history.
	/// </summary>
	/// <returns>The new canceled object.</returns>
	public TaskProcessingHistory Canceled()
	{
		return CompletedDate != null || CanceledDate != null
			? throw new InvalidOperationException("Task is already completed or canceled.")
			: new TaskProcessingHistory(
			CreatedDate,
			SuspendedDate,
			ProcessingStartDate,
			CompletedDate,
			DateTimeOffset.UtcNow);
	}

	/// <summary>
	/// Gets a suspended processing history.
	/// </summary>
	/// <returns>The new canceled object.</returns>
	public TaskProcessingHistory Completed()
	{
		return CompletedDate != null || CanceledDate != null
			? throw new InvalidOperationException("Task is already completed or canceled.")
			: new TaskProcessingHistory(
			CreatedDate,
			SuspendedDate,
			ProcessingStartDate,
			DateTimeOffset.UtcNow,
			CanceledDate);
	}

	/// <summary>
	/// Gets a suspended processing history.
	/// </summary>
	/// <returns>The new canceled object.</returns>
	public TaskProcessingHistory ProcessingStarted()
	{
		return CompletedDate != null || CanceledDate != null
			? throw new InvalidOperationException("Task is already completed or canceled.")
			: new TaskProcessingHistory(
			CreatedDate,
			SuspendedDate,
			DateTimeOffset.UtcNow,
			CompletedDate,
			CanceledDate);
	}

	/// <summary>
	/// Gets a suspended processing history.
	/// </summary>
	/// <returns>The new canceled object.</returns>
	public TaskProcessingHistory Suspended()
	{
		return CompletedDate != null || CanceledDate != null
			? throw new InvalidOperationException("Task is already completed or canceled.")
			: new TaskProcessingHistory(
			CreatedDate,
			DateTimeOffset.UtcNow,
			ProcessingStartDate,
			CompletedDate,
			CanceledDate);
	}
}