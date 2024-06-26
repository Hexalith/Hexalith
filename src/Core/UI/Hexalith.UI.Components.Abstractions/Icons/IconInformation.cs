﻿// <copyright file="IconInformation.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
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
[DataContract]
public record IconInformation(
    [property: DataMember] string Name,
    [property: DataMember] int Size,
    [property: DataMember] IconStyle Style)
{
}