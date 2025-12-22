// <copyright file="UserConversationProfileDomainHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.UserConversationProfiles;

using Hexalith.Domain.UserConversationProfiles.Aggregates;

/// <summary>
/// Provides helper methods for working with UserConversationProfile domain objects.
/// </summary>
public static class UserConversationProfileDomainHelper
{
    /// <summary>
    /// Gets the name of the UserConversationsProfile aggregate.
    /// </summary>
    /// <value>The name of the UserConversationsProfile aggregate.</value>
    public static string UserConversationProfileDomainName => nameof(UserConversationsProfile);

    /// <summary>
    /// Builds the aggregate ID for a UserConversationProfile.
    /// </summary>
    /// <param name="userId">The unique identifier for the user associated with the UserConversationProfile.</param>
    /// <returns>The aggregate ID for the UserConversationProfile.</returns>
    public static string BuildUserConversationProfileDomainId(string userId) => userId;
}