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
[Serializable]
public class TaskProcessingHistory
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskProcessingHistory"/> class.
    /// </summary>
    [Obsolete]
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
#pragma warning disable CS0618 // Type or member is obsolete
        CreatedDate = createdDate;
        SuspendedDate = suspendedDate;
        ProcessingStartDate = processingStartDate;
        CompletedDate = completedDate;
        CanceledDate = canceledDate;
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Gets or sets the task canceled date.
    /// </summary>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    public DateTimeOffset? CanceledDate
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Gets or sets the task completed date.
    /// </summary>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public DateTimeOffset? CompletedDate
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Gets or sets the task created date.
    /// </summary>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public DateTimeOffset CreatedDate
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Gets or sets the task processing start date.
    /// </summary>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public DateTimeOffset? ProcessingStartDate
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Gets or sets the task suspended date.
    /// </summary>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public DateTimeOffset? SuspendedDate
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

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