// <copyright file="ConversationThread.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ConversationThreads.Aggregates;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json;


using Hexalith.Domain.ConversationThreads.Entities;
using Hexalith.Domain.ConversationThreads.Events;
using Hexalith.Domain.Events;
using Hexalith.Domains;
using Hexalith.Domains.Results;

/// <summary>
/// Represents a conversation thread in the domain, implementing the <see cref="IDomainAggregate"/> interface.
/// This class encapsulates the state and behavior of a conversation, including its participants, timeline, and content.
/// </summary>
/// <param name="Owner">The owner or initiator of the conversation thread.</param>
/// <param name="StartedDate">The date and time when the conversation thread was initiated.</param>
/// <param name="EndedDate">The date and time when the conversation thread ended, if applicable. Null if the conversation is ongoing.</param>
/// <param name="Items">The collection of conversation items (messages or interactions) within the thread.</param>
[DataContract]
public record ConversationThread(
    [property: DataMember] string Owner,
    [property: DataMember] DateTimeOffset StartedDate,
    [property: DataMember] DateTimeOffset? EndedDate,
    [property: DataMember] IEnumerable<ConversationItem> Items) : IDomainAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConversationThread"/> class based on a conversation start event.
    /// </summary>
    /// <param name="startedEvent">The event that initiated the conversation thread.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="startedEvent"/> is null.</exception>
    public ConversationThread(ConversationThreadStarted startedEvent)
        : this(
              (startedEvent ?? throw new ArgumentNullException(nameof(startedEvent))).Owner,
              startedEvent.StartedDate,
              null,
              [])
    {
    }

    /// <summary>
    /// Gets the unique identifier for this conversation thread aggregate.
    /// </summary>
    /// <remarks>
    /// The aggregate ID is a combination of the Owner and the StartedDate, formatted as a string.
    /// </remarks>
    public string DomainId => GetAggregateId(Owner, StartedDate);

    /// <summary>
    /// Gets the name of this aggregate type.
    /// </summary>
    public string DomainName => ConversationDomainHelper.ConversationThreadAggregateName;

    /// <summary>
    /// Applies a domain event to the conversation thread, updating its state accordingly.
    /// </summary>
    /// <param name="domainEvent">The domain event to apply to the conversation thread.</param>
    /// <returns>An <see cref="ApplyResult"/> containing the updated state, any new events, and a success indicator.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="domainEvent"/> is null.</exception>
    public ApplyResult Apply([NotNull] object domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        if (domainEvent is ConversationThreadStarted started)
        {
            return !IsInitialized()
                ? ApplyEvent(started)
                : new ApplyResult(
            this,
            [new ConversationThreadEventCancelled(started, $"Aggregate {Owner}/{StartedDate} already initialized")],
            true);
        }

        if (domainEvent is ConversationThreadEvent contactEvent)
        {
            if (contactEvent.AggregateId != AggregateId)
            {
                return new ApplyResult(this, [new ConversationThreadEventCancelled(contactEvent, $"Invalid aggregate identifier for {Owner}/{StartedDate} : {contactEvent.AggregateId}")], true);
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
            ConversationItemAdded e => ApplyEvent(e),
            _ => new ApplyResult(
                this,
                [new ConversationThreadEventCancelled(contactEvent, "Event not implemented")],
                true),
        };
    }

    /// <summary>
    /// Determines whether the conversation thread has been initialized.
    /// </summary>
    /// <returns>true if the conversation thread is initialized (has an owner); otherwise, false.</returns>
    public bool IsInitialized() => !string.IsNullOrWhiteSpace(Owner);

    /// <summary>
    /// Generates the aggregate ID for a conversation thread.
    /// </summary>
    /// <param name="owner">The owner of the conversation thread.</param>
    /// <param name="startedDate">The date and time when the conversation thread started.</param>
    /// <returns>A string representing the unique aggregate ID for the conversation thread.</returns>
    public static string GetAggregateId(string owner, DateTimeOffset startedDate) => owner + startedDate.UtcDateTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);

    /// <summary>
    /// Applies a ConversationItemAdded event to the conversation thread.
    /// </summary>
    /// <param name="added">The ConversationItemAdded event to apply.</param>
    /// <returns>An ApplyResult containing the updated conversation thread and the applied event.</returns>
    private ApplyResult ApplyEvent(ConversationItemAdded added)
        => new(
            this with { Items = Items.Append(new(added.ItemDate, added.Participant, added.Content)) },
            [added],
            true);

    /// <summary>
    /// Applies a ConversationThreadStarted event to initialize a new conversation thread.
    /// </summary>
    /// <param name="started">The ConversationThreadStarted event to apply.</param>
    /// <returns>An ApplyResult containing the new conversation thread and the applied event.</returns>
    private ApplyResult ApplyEvent(ConversationThreadStarted started)
        => new(
            new ConversationThread(started),
            [started],
            true);
}