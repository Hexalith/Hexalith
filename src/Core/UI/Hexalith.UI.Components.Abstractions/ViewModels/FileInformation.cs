// <copyright file="FileInformation.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.ViewModels;

using System.Runtime.Serialization;

/// <summary>
/// Represents information about a file in the system.
/// </summary>
/// <param name="Id">The unique identifier of the file.</param>
/// <param name="Name">The name of the file.</param>
/// <param name="OriginalName">The original name of the file before any modifications.</param>
/// <param name="ContentType">The MIME content type of the file.</param>
public record FileInformation(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string OriginalName,
    [property: DataMember(Order = 4)] string ContentType);