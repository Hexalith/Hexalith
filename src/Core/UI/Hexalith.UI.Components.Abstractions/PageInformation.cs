// <copyright file="PageInformation.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components;

/// <summary>
/// Represents the information of a page.
/// </summary>
/// <param name="Id">The unique identifier of the page.</param>
/// <param name="Name">The name of the page.</param>
/// <param name="Description">The description of the page.</param>
public record PageInformation(
    string Id,
    string Name,
    string Description)
{
}