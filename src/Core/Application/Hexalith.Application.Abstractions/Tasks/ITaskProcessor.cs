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
	/// Gets the task canceled date.
	/// </summary>
	DateTimeOffset? CanceledDate { get; }

	/// <summary>
	/// Gets the task completed date.
	/// </summary>
	DateTimeOffset? CompletedDate { get; }

	/// <summary>
	/// Gets the task created date.
	/// </summary>
	DateTimeOffset CreatedDate { get; }

	/// <summary>
	/// Gets a value indicating whether gets the suspension expired status.
	/// </summary>
	/// <returns>True if the suspension has expired, else false.</returns>
	bool IsSuspensionExpired { get; }

	/// <summary>
	/// Gets the task processing start date.
	/// </summary>
	DateTimeOffset? ProcessingStartDate { get; }

	/// <summary>
	/// Gets the task processor status.
	/// </summary>
	TaskProcessorStatus Status { get; }

	/// <summary>
	/// Gets the task suspended date.
	/// </summary>
	DateTimeOffset? SuspendedDate { get; }

	/// <summary>
	/// Gets the task suspended until date.
	/// </summary>
	DateTimeOffset? SuspendedUntilDate { get; }

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
	/// Start the task.
	/// </summary>
	/// <returns>New copy of the task processor with status changed.</returns>
	ITaskProcessor Start();

	/// <summary>
	/// Suspend the task.
	/// </summary>
	/// <param name="date">Suspend until date.</param>
	/// <returns>New copy of the task processor in suspended state.</returns>
	ITaskProcessor SuspendUntil(DateTimeOffset date);
}