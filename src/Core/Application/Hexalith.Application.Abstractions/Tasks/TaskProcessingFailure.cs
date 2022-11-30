// <copyright file="TaskProcessingFailure.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Tasks;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// The task processing failure information.
/// </summary>
[DataContract]
public class TaskProcessingFailure
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TaskProcessingFailure"/> class.
	/// </summary>
	/// <param name="count">The total failure count.</param>
	/// <param name="date">The last failure date.</param>
	/// <param name="message">The last failure message.</param>
	[JsonConstructor]
	public TaskProcessingFailure(int count, DateTimeOffset date, string message)
	{
		Count = count;
		Date = date;
		Message = message;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TaskProcessingFailure"/> class.
	/// </summary>
	[Obsolete("This constructor is only for serialization purposes.", true)]
	public TaskProcessingFailure()
	{
		Date = DateTimeOffset.MinValue;
		Message = string.Empty;
	}

	/// <summary>
	/// Gets the failed count.
	/// </summary>
	public int Count { get; private set; }

	/// <summary>
	/// Gets the last failed date.
	/// </summary>
	public DateTimeOffset Date { get; private set; }

	/// <summary>
	/// Gets the last failed message.
	/// </summary>
	public string Message { get; private set; }

	/// <summary>
	/// Fail.
	/// </summary>
	/// <param name="message">The error message.</param>
	/// <returns>A new failure with the fail count incremented.</returns>
	public TaskProcessingFailure Fail(string message)
	{
		return new TaskProcessingFailure(Count + 1, DateTimeOffset.UtcNow, message);
	}
}