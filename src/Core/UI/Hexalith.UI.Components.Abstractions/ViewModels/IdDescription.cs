// <copyright file="IdDescription.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.ViewModels;

/// <summary>
/// Represents an identifier-description pair.
/// </summary>
/// <remarks>
/// This record is used to store and retrieve information about an entity
/// using its identifier and description.
/// </remarks>
public record IdDescription(string Id, string Description)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdDescription"/> class.
    /// </summary>
    public IdDescription()
        : this(string.Empty, string.Empty)
    {
    }
}