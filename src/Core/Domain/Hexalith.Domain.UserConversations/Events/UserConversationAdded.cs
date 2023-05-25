// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="UserConversationAdded.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
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
public class UserConversationAdded : UserConversationsProfileEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserConversationAdded"/> class.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="startedDate">The started date.</param>
    public UserConversationAdded(string userId, DateTimeOffset startedDate)
        : base(userId) => StartedDate = startedDate;

    /// <summary>
    /// Gets the started date.
    /// </summary>
    /// <value>The started date.</value>
    public DateTimeOffset StartedDate { get; }

    /// <summary>
    /// Get the message name.
    /// </summary>
    /// <returns>The name.</returns>
    protected override string DefaultTypeName() => nameof(UserConversationsProfileAdded);
}