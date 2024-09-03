// <copyright file="ModuleInformation.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components;

using System.Runtime.Serialization;

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
[DataContract]
public record ModuleInformation(
    [property: DataMember] string Id,
    [property: DataMember] string Name,
    [property: DataMember] string Description,
    [property: DataMember] string Version)
{
}