// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="UserConversationsProfile.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.UserConversationProfiles.Aggregates;

using System.Collections.Generic;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.UserConversationProfiles.Events;

/// <summary>
/// Class UserConversations.
/// Implements the <see cref="IAggregate" />
/// Implements the <see cref="IEquatable{UserConversations}" />.
/// </summary>
/// <seealso cref="IAggregate" />
/// <seealso cref="IEquatable{UserConversations}" />
public record UserConversationsProfile(string UserId, IEnumerable<DateTimeOffset> Conversations) : IAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserConversationsProfile"/> class.
    /// </summary>
    /// <param name="startedEvent">The started event.</param>
    public UserConversationsProfile(UserConversationsProfileAdded startedEvent)
        : this(
              (startedEvent ?? throw new ArgumentNullException(nameof(startedEvent))).UserId,
              Array.Empty<DateTimeOffset>())
    {
    }

    /// <inheritdoc/>
    public string AggregateId => UserId;

    /// <inheritdoc/>
    public string AggregateName => nameof(UserConversationsProfile);

    /// <summary>
    /// Applies the specified domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <returns>IAggregate.</returns>
    public IAggregate Apply(BaseEvent domainEvent)
    {
        return domainEvent switch
        {
            UserConversationAdded add => this with { Conversations = Conversations.Append(add.StartedDate) },
            UserConversationsProfileAdded => throw new InvalidAggregateEventException(this, domainEvent, true),
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        };
    }
}