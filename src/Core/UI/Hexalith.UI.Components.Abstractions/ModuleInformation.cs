// <copyright file="ModuleInformation.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components;

/// <summary>
/// Represents the information of a module.
/// </summary>
/// <remarks>
/// This class contains properties to store the module's ID, name, description, and version.
/// </remarks>
/// <param name="Id">The unique identifier of the module.</param>
/// <param name="Name">The name of the module.</param>
/// <param name="Description">The description of the module.</param>
/// <param name="Version">The version of the module.</param>

public record ModuleInformation(
    string Id,
    string Name,
    string Description,
    string Version)
{
}