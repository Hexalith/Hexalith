// <copyright file="ApplicationInformation.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components;

/// <summary>
/// Represents the application information.
/// </summary>
/// <param name="Id">The unique identifier of the application.</param>
/// <param name="Name">The name of the application.</param>
/// <param name="Description">The description of the application.</param>
/// <param name="Company">The company name of the application.</param>
/// <param name="Version">The version of the application.</param>
public record ApplicationInformation(
    string Id,
    string Name,
    string Description,
    string Company,
    string Version)
{
}