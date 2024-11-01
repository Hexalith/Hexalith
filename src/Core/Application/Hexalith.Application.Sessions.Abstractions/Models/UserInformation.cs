// <copyright file="UserInformation.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents user information including identity and roles.
/// </summary>
[DataContract]
public record UserInformation(
    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    [property: DataMember(Order = 1)]
    string Id,
    /// <summary>
    /// Gets the collection of roles assigned to the user.
    /// </summary>
    [property: DataMember(Order = 2)]
    IEnumerable<string> Roles)
{
}
