// <copyright file="UserInformation.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components;

/// <summary>
/// Represents information about a user.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Name">The name of the user.</param>
/// <param name="Initials">The initials of the user.</param>
/// <param name="Email">The email address of the user.</param>
/// <param name="Presence">The presence status of the user.</param>
/// <param name="Company">The company the user is associated with.</param>
/// <param name="Picture">The URL of the user's picture.</param>
public record UserInformation(
    string Id,
    string Name,
    string Initials,
    string Email,
    UserPresence Presence,
    string? Company,
    string? Picture)
{
}