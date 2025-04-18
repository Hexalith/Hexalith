// <copyright file="ConversationThreadStarted.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ConversationThreads.Events;

using System;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents an event that occurs when a new conversation thread is initiated in the system.
/// This event marks the beginning of a conversation and contains essential information to uniquely identify and track the thread.
/// </summary>
/// <param name="Owner">The owner or initiator of the conversation thread. This could be a user ID, system identifier, or any unique string that represents the entity starting the conversation.</param>
/// <param name="StartedDate">The exact date and time when the conversation thread was initiated. This timestamp, along with the Owner, forms a unique identifier for the thread and allows for chronological ordering of conversations.</param>
/// <remarks>
/// This class inherits from <see cref="ConversationThreadEvent"/>, providing a specialized event for the start of a conversation.
/// It is marked as partial, allowing for potential extension in other partial class definitions.
/// The combination of Owner and StartedDate should be unique across all conversation threads in the system.
/// </remarks>
[PolymorphicSerialization]
public partial record ConversationThreadStarted(string Owner, DateTimeOffset StartedDate)
    : ConversationThreadEvent(Owner, StartedDate);