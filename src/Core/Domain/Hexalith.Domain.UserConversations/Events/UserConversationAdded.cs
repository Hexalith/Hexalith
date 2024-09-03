// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="UserConversationAdded.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.UserConversationProfiles.Events;

/// <summary>
/// Class AddConversationItem.
/// Implements the <see cref="UserConversationsProfileEvent" />.
/// </summary>
/// <seealso cref="UserConversationsProfileEvent" />
/// <remarks>
/// Initializes a new instance of the <see cref="UserConversationAdded"/> class.
/// </remarks>
/// <param name="userId">The user identifier.</param>
/// <param name="startedDate">The started date.</param>
public class UserConversationAdded(string userId, DateTimeOffset startedDate) : UserConversationsProfileEvent(userId)
{
    /// <summary>
    /// Gets the started date.
    /// </summary>
    /// <value>The started date.</value>
    public DateTimeOffset StartedDate { get; } = startedDate;

    /// <summary>
    /// Get the message name.
    /// </summary>
    /// <returns>The name.</returns>
    protected override string DefaultTypeName() => nameof(UserConversationsProfileAdded);
}