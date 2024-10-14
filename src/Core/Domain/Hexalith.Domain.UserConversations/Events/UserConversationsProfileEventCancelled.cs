// <copyright file="UserConversationsProfileEventCancelled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.UserConversationProfiles.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an event indicating that a user conversation profile event has been cancelled.
/// </summary>
/// <param name="Event">The original user conversations profile event that was cancelled.</param>
/// <param name="Reason">The reason for cancelling the event.</param>
[PolymorphicSerialization]
public partial record UserConversationsProfileEventCancelled(
    UserConversationsProfileEvent Event,
    string Reason) : UserConversationsProfileEvent(Event.UserId);