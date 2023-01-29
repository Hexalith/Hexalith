// <copyright file="EventState.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.States;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Domain.Abstractions.Messages;
using Hexalith.Infrastructure.DaprAggregateActor;

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
        BaseMessage message,
        Metadata metadata)
        : base(receivedDate, idempotencyId, message, metadata)
    {
        if (message is not BaseEvent)
        {
            throw new ArgumentException("Message must be an event", nameof(message));
        }
    }

    /// <summary>
    /// Gets the event.
    /// </summary>
    /// <value>The event.</value>
    [IgnoreDataMember]
    [JsonIgnore]
    public BaseEvent Event => (BaseEvent)Message;
}