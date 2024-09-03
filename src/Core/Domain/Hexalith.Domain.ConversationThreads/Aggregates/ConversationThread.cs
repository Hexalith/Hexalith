// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="ConversationThread.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.ConversationThreads.Aggregates;

using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.ConversationThreads.Entities;
using Hexalith.Domain.ConversationThreads.Events;
using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.Messages;

/// <summary>
/// Class ConversationThread.
/// Implements the <see cref="IAggregate" />
/// Implements the <see cref="IEquatable{ConversationThread}" />.
/// </summary>
/// <seealso cref="IAggregate" />
/// <seealso cref="IEquatable{ConversationThread}" />
[DataContract]
public record ConversationThread(
    [property: DataMember] string Owner,
    [property: DataMember] DateTimeOffset StartedDate,
    [property: DataMember] DateTimeOffset? EndedDate,
    [property: DataMember] IEnumerable<ConversationItem> Items) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConversationThread" /> class.
    /// </summary>
    /// <param name="startedEvent">The started event.</param>
    public ConversationThread(ConversationThreadStarted startedEvent)
        : this(
              (startedEvent ?? throw new ArgumentNullException(nameof(startedEvent))).Owner,
              startedEvent.StartedDate,
              null,
              [])
    {
    }

    /// <summary>
    /// Applies the specified domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <returns>IAggregate.</returns>
    public override (IAggregate Aggregate, IEnumerable<BaseMessage> Messages) Apply(BaseEvent domainEvent)
    {
        return (domainEvent switch
        {
            ConversationItemAdded add => this with
            {
                Items = Items.Append(new ConversationItem(
                    add.ItemDate,
                    add.Participant,
                    add.Content)),
            },
            ConversationThreadStarted => throw new InvalidAggregateEventException(this, domainEvent, true),
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        }, []);
    }

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="startedDate">The started date.</param>
    /// <returns>System.String.</returns>
    public static string GetAggregateId(string owner, DateTimeOffset startedDate)
        => Normalize(GetAggregateName() + Separator + owner + startedDate.UtcDateTime.ToString("YYYYMMDDHHmmss", CultureInfo.InvariantCulture));

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <returns>System.String.</returns>
#pragma warning disable CA1024 // Use properties where appropriate
    public static string GetAggregateName() => nameof(ConversationThread);
#pragma warning restore CA1024 // Use properties where appropriate

    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Owner);
}