// <copyright file="ConversationItem.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ConversationThreads.Entities;

using System.Runtime.Serialization;

/// <summary>
/// Class ConversationItem.
/// Implements the <see cref="IEquatable{ConversationItem}" />.
/// </summary>
/// <seealso cref="IEquatable{ConversationItem}" />
[DataContract]
public record ConversationItem([property: DataMember] DateTimeOffset Date, [property: DataMember] string Participant, [property: DataMember] string Content)
{
}