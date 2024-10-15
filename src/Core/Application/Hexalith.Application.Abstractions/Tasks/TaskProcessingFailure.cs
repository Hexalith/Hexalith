// <copyright file="TaskProcessingFailure.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Tasks;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// The task processing failure information.
/// </summary>
[DataContract]
public class TaskProcessingFailure
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaskProcessingFailure" /> class.
    /// </summary>
    /// <param name="count">The total failure count.</param>
    /// <param name="date">The last failure date.</param>
    /// <param name="message">The last failure message.</param>
    /// <param name="technicalError">The technical error.</param>
    [JsonConstructor]
    public TaskProcessingFailure(int count, DateTimeOffset date, string message, string? technicalError)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Count = count;
        Date = date;
        Message = message;
        TechnicalError = technicalError;
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskProcessingFailure"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public TaskProcessingFailure()
    {
        Date = DateTimeOffset.MinValue;
        Message = string.Empty;
    }

    /// <summary>
    /// Gets or sets the failed count.
    /// </summary>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public int Count
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Gets or sets the last failed date.
    /// </summary>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public DateTimeOffset Date
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Gets or sets the last failed message.
    /// </summary>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string Message
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Gets or sets the technical error.
    /// </summary>
    /// <value>The technical error.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public string? TechnicalError
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Fails the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="technicalError">The technical error.</param>
    /// <returns>TaskProcessingFailure.</returns>
    public TaskProcessingFailure Fail(string message, string? technicalError) => new(Count + 1, DateTimeOffset.UtcNow, message, technicalError);
}