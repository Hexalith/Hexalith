// <copyright file="UserConversationsProfile.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.UserConversationProfiles.Aggregates;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.UserConversationProfiles.Entities;
using Hexalith.Domain.UserConversationProfiles.Events;

/// <summary>
/// Represents a user's conversation profile, which is an aggregate root in the domain.
/// </summary>
/// <param name="UserId">The unique identifier of the user.</param>
/// <param name="Conversations">The collection of conversation items associated with the user.</param>
[DataContract]
public record UserConversationsProfile(string UserId, IEnumerable<ConversationItem> Conversations) : IDomainAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserConversationsProfile"/> class.
    /// </summary>
    /// <param name="startedEvent">The event that initiated the creation of this profile.</param>
    /// <exception cref="ArgumentNullException">Thrown if startedEvent is null.</exception>
    public UserConversationsProfile(UserConversationsProfileAdded startedEvent)
        : this(
              (startedEvent ?? throw new ArgumentNullException(nameof(startedEvent))).UserId,
              [])
    {
    }

    /// <inheritdoc/>
    public string AggregateId => UserConversationProfileDomainHelper.BuildUserConversationProfileAggregateId(UserId);

    /// <inheritdoc/>
    public string AggregateName => UserConversationProfileDomainHelper.UserConversationProfileAggregateName;

    /// <inheritdoc/>
    public ApplyResult Apply([NotNull] object domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        if (domainEvent is UserConversationsProfileAdded added)
        {
            return !IsInitialized()
            ? ApplyEvent(added)
            : new ApplyResult(
                this,
                [new UserConversationsProfileEventCancelled(added, $"Aggregate {UserId} already initialized")],
                true);
        }

        if (domainEvent is UserConversationsProfileEvent contactEvent)
        {
            if (contactEvent.AggregateId != AggregateId)
            {
                return new ApplyResult(this, [new UserConversationsProfileEventCancelled(contactEvent, $"Invalid aggregate identifier for {UserId} : {contactEvent.AggregateId}")], true);
            }
        }
        else
        {
            return new ApplyResult(
                this,
                [new InvalidEventApplied(
                    AggregateName,
                    AggregateId,
                    domainEvent.GetType().FullName ?? "Unknown",
                    JsonSerializer.Serialize(domainEvent),
                    $"Unexpected event applied.")],
                true);
        }

        return contactEvent switch
        {
            UserConversationAdded e => ApplyEvent(e),
            _ => new ApplyResult(
                this,
                [new UserConversationsProfileEventCancelled(contactEvent, "Event not implemented")],
                true),
        };
    }

    /// <inheritdoc/>
    public bool IsInitialized() => !string.IsNullOrWhiteSpace(UserId);

    /// <summary>
    /// Applies the UserConversationsProfileAdded event to create a new UserConversationsProfile.
    /// </summary>
    /// <param name="added">The UserConversationsProfileAdded event to apply.</param>
    /// <returns>An ApplyResult containing the new UserConversationsProfile and the applied event.</returns>
    private ApplyResult ApplyEvent(UserConversationsProfileAdded added)
        => new(new UserConversationsProfile(added), [added], false);

    /// <summary>
    /// Applies the UserConversationAdded event to add a new conversation item to the profile.
    /// </summary>
    /// <param name="added">The UserConversationAdded event to apply.</param>
    /// <returns>An ApplyResult containing the updated UserConversationsProfile and the applied event.</returns>
    private ApplyResult ApplyEvent(UserConversationAdded added)
        => new(
            this with
            {
                Conversations = Conversations.Append(new ConversationItem(added.Date, added.Participant, added.Content)),
            },
            [added],
            false);
}