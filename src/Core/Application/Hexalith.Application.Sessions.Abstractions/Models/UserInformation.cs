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
/// <param name="IdentityProviderName"></param>
/// <param name="Name">The name of the user.</param>
/// <param name="IsGlobalAdministrator">Indicates whether the user is a global administrator.</param>
/// <param name="Roles">The roles associated with the user.</param>
[DataContract]
public record UserInformation(
    [property: DataMember(Order = 1)]
    string Id,
    [property: DataMember(Order = 2)]
    string IdentityProviderName,
    [property: DataMember(Order = 2)]
    string Name,
    [property: DataMember(Order = 3)]
    bool IsGlobalAdministrator,
    [property: DataMember(Order = 4)]
    IEnumerable<string> Roles);