﻿// ***********************************************************************
// Assembly         : Hexalith.Domain.Conversations
// Author           : Jérôme Piquot
// Created          : 05-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-25-2023
// ***********************************************************************
// <copyright file="ConversationItem.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.UserConversationProfiles.Entities;

using System.Runtime.Serialization;

/// <summary>
/// Class ConversationItem.
/// Implements the <see cref="IEquatable{ConversationItem}" />.
/// </summary>
/// <seealso cref="IEquatable{ConversationItem}" />
[DataContract]
public record ConversationItem([property: DataMember(Order = 1)] DateTimeOffset Date, [property: DataMember(Order = 2)] string Participant, [property: DataMember(Order = 3)] string Content)
{
}