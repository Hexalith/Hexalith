// <copyright file="UserConversationAdded.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.UserConversationProfiles.Events;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents an event that occurs when a new user conversation is added.
/// </summary>
/// <param name="UserId">The unique identifier of the user associated with the conversation.</param>
/// <param name="Participant">The identifier or name of the participant in the conversation.</param>
/// <param name="Date">The date and time when the conversation entry was added.</param>
/// <param name="Content">The content of the conversation entry.</param>
[PolymorphicSerialization]
public partial record UserConversationAdded(
    string UserId,
    string Participant,
    DateTimeOffset Date,
    string Content) : UserConversationsProfileEvent(UserId)
{
}