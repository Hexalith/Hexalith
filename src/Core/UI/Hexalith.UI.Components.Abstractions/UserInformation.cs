// <copyright file="UserInformation.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components;

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