// <copyright file="UserIdentity.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents a user identity in the application.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Provider">The authentication provider or identity service that manages this user.</param>
/// <param name="Name">The display name of the user.</param>
/// <param name="Email">The email address associated with the user account.</param>
/// <param name="IsGlobalAdministrator">A value indicating whether the user has global administrator privileges.</param>
/// <param name="Disabled">A value indicating whether the user account is disabled.</param>
[DataContract]
public record UserIdentity(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Provider,
    [property: DataMember(Order = 3)] string Name,
    [property: DataMember(Order = 4)] string Email,
    [property: DataMember(Order = 5)] bool IsGlobalAdministrator,
    [property: DataMember(Order = 6)] bool Disabled);