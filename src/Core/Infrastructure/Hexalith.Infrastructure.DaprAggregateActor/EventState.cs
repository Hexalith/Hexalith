// <copyright file="EventState.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprAggregateActor;

using System;
using System.Text.Json.Serialization;

/// <summary>
/// Class EventState.
/// </summary>
public class EventState : MessageState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventState" /> class.
    /// </summary>
    [Obsolete("For serialization only", true)]
    public EventState()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventState" /> class.
    /// </summary>
    /// <param name="receivedDate">The received date.</param>
    /// <param name="idempotencyId">The idempotency identifier.</param>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="publishedDate">The published date.</param>
    [JsonConstructor]
    public EventState(
        DateTimeOffset receivedDate,
        string idempotencyId,
        string message,
        string metadata,
        DateTimeOffset? publishedDate)
        : base(receivedDate, idempotencyId, message, metadata)
    {
        PublishedDate = publishedDate;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventState"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="publishedDate">The published date.</param>
    public EventState(MessageState message, DateTimeOffset publishedDate)
        : this(
              message.ReceivedDate,
              message.IdempotencyId,
              message.Message,
              message.Metadata,
              publishedDate)
    {
        PublishedDate = publishedDate;
    }

    /// <summary>
    /// Gets the processed date.
    /// </summary>
    /// <value>The processed date.</value>
    public DateTimeOffset? PublishedDate { get; }
}
