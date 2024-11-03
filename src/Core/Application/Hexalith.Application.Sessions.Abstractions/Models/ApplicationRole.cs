// <copyright file="ApplicationRole.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents a role.
/// </summary>
/// <param name="Id">The unique identifier for the role.</param>
/// <param name="PartitionId">The partition identifier for the role.</param>
/// <param name="Name">The name of the role.</param>
[DataContract]
public record ApplicationRole(
    [property: DataMember] string Id,
    [property: DataMember] string PartitionId,
    [property: DataMember] string Name)
{
}