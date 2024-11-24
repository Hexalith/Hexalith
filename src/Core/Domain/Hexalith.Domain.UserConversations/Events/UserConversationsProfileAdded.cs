// <copyright file="UserConversationsProfileAdded.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.UserConversationProfiles.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an event that occurs when a user conversations profile is added.
/// </summary>
/// <param name="UserId">The unique identifier of the user for whom the conversations profile is added.</param>
[PolymorphicSerialization]
public record UserConversationsProfileAdded(string UserId) : UserConversationsProfileEvent(UserId);