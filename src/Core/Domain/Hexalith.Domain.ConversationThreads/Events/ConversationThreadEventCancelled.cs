// <copyright file="ConversationThreadEventCancelled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ConversationThreads.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an event that indicates the cancellation of a conversation thread event.
/// </summary>
/// <param name="Event">The original conversation thread event that is being cancelled.</param>
/// <param name="Reason">The reason for the cancellation of the conversation thread event.</param>
/// <remarks>
/// This record inherits from <see cref="ConversationThreadEvent"/> and adds additional context specific to the cancellation event.
/// </remarks>
[PolymorphicSerialization]
public partial record ConversationThreadEventCancelled(
    ConversationThreadEvent Event,
    string Reason)
    : ConversationThreadEvent(Event.Owner, Event.StartedDate);