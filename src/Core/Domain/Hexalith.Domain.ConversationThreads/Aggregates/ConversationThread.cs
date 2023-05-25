// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="ConversationThread.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.ConversationThreads.Aggregates;

using System.Collections.Generic;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.ConversationThreads.Entities;
using Hexalith.Domain.ConversationThreads.Events;
using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;

/// <summary>
/// Class ConversationThread.
/// Implements the <see cref="IAggregate" />
/// Implements the <see cref="IEquatable{ConversationThread}" />.
/// </summary>
/// <seealso cref="IAggregate" />
/// <seealso cref="IEquatable{ConversationThread}" />
public record ConversationThread(string Owner, DateTimeOffset StartedDate, DateTimeOffset? EndedDate, IEnumerable<ConversationItem> Items) : IAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConversationThread" /> class.
    /// </summary>
    /// <param name="startedEvent">The started event.</param>
    public ConversationThread(ConversationThreadStarted startedEvent)
        : this(startedEvent.Owner, startedEvent.StartedDate, null, Array.Empty<ConversationItem>())
    {
    }

    /// <inheritdoc/>
    public string AggregateId => GetAggregateId(Owner, StartedDate);

    /// <inheritdoc/>
    public string AggregateName => nameof(ConversationThread);

    /// <summary>
    /// Applies the specified domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <returns>IAggregate.</returns>
    public IAggregate Apply(BaseEvent domainEvent)
    {
        return domainEvent switch
        {
            ConversationItemAdded add => this with { Items = Items.Append(new ConversationItem(add.ItemDate, add.Content, add.Participant)) },
            ConversationThreadStarted => throw new InvalidAggregateEventException(this, domainEvent, true),
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        };
    }

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="startedDate">The started date.</param>
    /// <returns>System.String.</returns>
    public static string GetAggregateId(string owner, DateTimeOffset startedDate) => owner + startedDate.UtcDateTime.ToString("YYYYMMDDHHmmss");
}