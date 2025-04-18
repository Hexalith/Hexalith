// <copyright file="ConversationItemAdded.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ConversationThreads.Events;

using System;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents an event that occurs when a new item (message or interaction) is added to a conversation thread.
/// This event captures the details of the added item, including who added it, when it was added, and its content.
/// </summary>
/// <param name="Owner">The owner or initiator of the conversation thread. This helps identify the specific thread to which the item is being added.</param>
/// <param name="StartedDate">The date and time when the conversation thread was initially started. This, along with the Owner, helps uniquely identify the thread.</param>
/// <param name="ItemDate">The specific date and time when this item was added to the conversation. This allows for precise chronological ordering of items within the thread.</param>
/// <param name="Participant">The identifier of the participant who added this item to the conversation. This could be a user ID, username, or any other unique identifier for the participant.</param>
/// <param name="Content">The actual content of the added conversation item. This could be text, a reference to an attachment, or any other relevant data representing the item's content.</param>
[PolymorphicSerialization]
public partial record ConversationItemAdded(
    string Owner,
    DateTimeOffset StartedDate,
    DateTimeOffset ItemDate,
    string Participant,
    string Content)
    : ConversationThreadEvent(Owner, StartedDate);