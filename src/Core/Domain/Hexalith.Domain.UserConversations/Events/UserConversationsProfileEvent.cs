// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="UserConversationsProfileEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.UserConversationProfiles.Events;

using Hexalith.Domain.Events;
using Hexalith.Domain.UserConversationProfiles.Aggregates;

/// <summary>
/// Class UserConversationsProfileEvent.
/// Implements the <see cref="BaseEvent" />.
/// </summary>
/// <seealso cref="BaseEvent" />
public class UserConversationsProfileEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserConversationsProfileEvent" /> class.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    public UserConversationsProfileEvent(string userId) => UserId = userId;

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    /// <value>The user identifier.</value>
    public string UserId { get; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => UserId;

    /// <summary>
    /// Get the aggregate name.
    /// </summary>
    /// <returns>The name.</returns>
    protected override string DefaultAggregateName() => nameof(UserConversationsProfile);
}