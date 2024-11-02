// <copyright file="UserInformation.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents user information including identity and roles.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Roles">The roles associated with the user.</param>
[DataContract]
public record UserInformation(
    [property: DataMember(Order = 1)]
    string Id,
    [property: DataMember(Order = 2)]
    IEnumerable<string> Roles);