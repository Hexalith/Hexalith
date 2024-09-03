// <copyright file="IconInformation.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Icons;

using System.Runtime.Serialization;

/// <summary>
/// Represents the information of an icon.
/// </summary>
/// <param name="Name">The icon name.</param>
/// <param name="Size">The icon size.</param>
/// <param name="Style">The icon style.</param>
/// <param name="Source">The icon source.</param>
/// <param name="Aspect">The icon aspect.</param>
/// <param name="IconLibraryName">The razor component namespace of the icon library.</param>
[DataContract]
public record IconInformation(
    [property: DataMember] string Name,
    [property: DataMember] int Size,
    [property: DataMember] IconStyle Style,
    [property: DataMember] IconSource Source,
    [property: DataMember] string? IconLibraryName
    )
{
}