// <copyright file="SessionInformation.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents session information containing user and contact details.
/// </summary>
/// <param name="UserId">The unique identifier for the user.</param>
/// <param name="SessionId">The unique identifier for the session.</param>
/// <param name="PartitionId">The partition identifier for data segregation.</param>
/// <param name="CreatedOn">The date and time the session was created.</param>
[DataContract]
public record SessionInformation(
    [property: DataMember(Order = 1)]
    string UserId,
    [property: DataMember(Order = 2)]
    string SessionId,
    [property: DataMember(Order = 3)]
    string PartitionId,
    [property: DataMember(Order = 4)]
    DateTimeOffset CreatedOn);