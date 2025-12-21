// <copyright file="ConversationThreadEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ConversationThreads.Events;

using Hexalith.Domain.ConversationThreads.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a base event for conversation threads in the domain.
/// This abstract class serves as a foundation for specific conversation thread events,
/// providing common properties and behavior shared across all conversation-related events.
/// It encapsulates the core information needed to identify and process events within the context of a conversation thread.
/// </summary>
/// <param name="Owner">The owner or initiator of the conversation thread. This helps identify the specific thread associated with the event.</param>
/// <param name="StartedDate">The date and time when the conversation thread was initially started. This, along with the Owner, helps uniquely identify the thread.</param>
/// <remarks>
/// This class is marked as partial, allowing for potential extension in other partial class definitions.
/// </remarks>
/// <seealso cref="object" />
[PolymorphicSerialization]
public partial record ConversationThreadEvent(string Owner, DateTimeOffset StartedDate)
{
    /// <summary>
    /// Gets the unique identifier for the conversation thread aggregate.
    /// </summary>
    /// <remarks>
    /// The aggregate ID is derived from the Owner and StartedDate using the ConversationThread.GetAggregateId method.
    /// This ensures consistency in identifying the specific conversation thread across different parts of the system.
    /// </remarks>
    public string DomainId => ConversationThread.GetAggregateId(Owner, StartedDate);

    /// <summary>
    /// Gets the name of the aggregate associated with this event.
    /// </summary>
    /// <returns>The name of the conversation thread aggregate.</returns>
    /// <remarks>
    /// This property returns a constant value defined in ConversationDomainHelper.
    /// It provides a standardized way to identify the type of aggregate this event relates to.
    /// </remarks>
    public string DomainName => ConversationDomainHelper.ConversationThreadAggregateName;
}