// <copyright file="UserConversationsProfileEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.UserConversationProfiles.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an event related to a user's conversation profile.
/// This class serves as a base for all user conversation profile events.
/// </summary>
/// <param name="UserId">The unique identifier of the user associated with this event.</param>
[PolymorphicSerialization]
public record UserConversationsProfileEvent(string UserId)
{
    /// <summary>
    /// Gets the aggregate identifier for the user conversation profile.
    /// </summary>
    /// <returns>A string representing the aggregate identifier, constructed using the <see cref="UserId"/>.</returns>
    /// <remarks>
    /// The aggregate identifier is built using the <see cref="UserConversationProfileDomainHelper.BuildUserConversationProfileAggregateId"/> method.
    /// </remarks>
    public string AggregateId => UserConversationProfileDomainHelper.BuildUserConversationProfileAggregateId(UserId);

    /// <summary>
    /// Gets the name of the aggregate for user conversation profiles.
    /// </summary>
    /// <returns>A string representing the aggregate name.</returns>
    /// <remarks>
    /// The aggregate name is a constant value defined in <see cref="UserConversationProfileDomainHelper.UserConversationProfileAggregateName"/>.
    /// </remarks>
    public string AggregateName => UserConversationProfileDomainHelper.UserConversationProfileAggregateName;
}