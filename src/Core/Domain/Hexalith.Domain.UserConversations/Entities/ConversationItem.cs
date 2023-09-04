// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="ConversationItem.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.UserConversationProfiles.Entities;

/// <summary>
/// Class ConversationItem.
/// Implements the <see cref="IEquatable{ConversationItem}" />.
/// </summary>
/// <seealso cref="IEquatable{ConversationItem}" />
public record ConversationItem(DateTimeOffset Date, string Participant, string Content)
{
}