// <copyright file="Session.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Models;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Represents a session with its details.
/// </summary>
/// <param name="Id">The unique identifier of the session.</param>
/// <param name="UserId">The unique identifier of the user associated with the session.</param>
/// <param name="PartitionId">The partition identifier associated with the session.</param>
/// <param name="CreatedAt">The date and time when the session was created.</param>
/// <param name="Expiration">The duration after which the session will expire.</param>
/// <param name="LastActivity">The date and time of the last activity in the session.</param>
/// <param name="Disabled">Indicates whether the session is disabled.</param>
[DataContract]
public record Session(
    [property: DataMember(Order = 1)]
    string Id,
    [property: DataMember(Order = 2)]
    string UserId,
    [property: DataMember(Order = 3)]
    string PartitionId,
    [property: DataMember(Order = 4)]
    DateTimeOffset CreatedAt,
    [property: DataMember(Order = 5)]
    TimeSpan Expiration,
    [property: DataMember(Order = 6)]
    DateTimeOffset LastActivity,
    [property: DataMember(Order = 7)]
    bool Disabled);