// <copyright file="SessionInformation.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents session information containing user and contact details.
/// </summary>
/// <param name="Id">The unique identifier for the session.</param>
/// <param name="PartitionId">The partition identifier for data segregation.</param>
/// <param name="User">The user information associated with the session.</param>
/// <param name="Contact">The contact information associated with the session.</param>
/// <param name="CreatedOn">The date and time when the session was created.</param>
/// <param name="Expiration">The duration after which the session expires.</param>
[DataContract]
public record SessionInformation(
    [property: DataMember(Order = 1)]
    string Id,
    [property: DataMember(Order = 2)]
    string PartitionId,
    [property: DataMember(Order = 3)]
    UserInformation User,
    [property: DataMember(Order = 4)]
    ContactInformation Contact,
    [property: DataMember(Order = 5)]
    DateTimeOffset CreatedOn,
    [property: DataMember(Order = 6)]
    TimeSpan Expiration);
