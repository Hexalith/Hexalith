// <copyright file="MessageState.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprAggregateActor;

using System;
using System.Text.Json.Serialization;

/// <summary>
/// Class MessageState.
/// </summary>
public class MessageState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageState" /> class.
    /// </summary>
    [Obsolete("For serialization only", true)]
    public MessageState()
    {
        ReceivedDate = DateTimeOffset.MinValue;
        IdempotencyId = Metadata = Message = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageState" /> class.
    /// </summary>
    /// <param name="receivedDate">The received date.</param>
    /// <param name="processedDate">The processed date.</param>
    /// <param name="idempotencyId">The idempotency identifier.</param>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    [JsonConstructor]
    public MessageState(DateTimeOffset receivedDate, string idempotencyId, string message, string metadata)
    {
        ReceivedDate = receivedDate;
        IdempotencyId = idempotencyId;
        Message = message;
        Metadata = metadata;
    }

    /// <summary>
    /// Gets the idempotency identifier.
    /// </summary>
    /// <value>The idempotency identifier.</value>
    public string IdempotencyId { get; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <value>The message.</value>
    public string Message { get; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <value>The metadata.</value>
    public string Metadata { get; }

    /// <summary>
    /// Gets the received date.
    /// </summary>
    /// <value>The received date.</value>
    public DateTimeOffset ReceivedDate { get; }
}