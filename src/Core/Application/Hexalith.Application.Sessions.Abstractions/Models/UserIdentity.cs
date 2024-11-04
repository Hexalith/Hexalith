// <copyright file="UserIdentity.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents a user identity in the application.
/// </summary>
/// <param name="Id">The user identifier.</param>
/// <param name="Provider">The provider of the user.</param>
/// <param name="Name">The user name.</param>
/// <param name="Email">The user's email address.</param>
/// <param name="Disabled">A value indicating whether the user identity is disabled.</param>
[DataContract]
public record UserIdentity(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Provider,
    [property: DataMember(Order = 3)] string Name,
    [property: DataMember(Order = 4)] string Email,
    [property: DataMember(Order = 5)] bool Disabled);