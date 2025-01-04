// <copyright file="IEntityViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components;

/// <summary>
/// Represents a view model for an entity with common properties.
/// </summary>
public interface IEntityViewModel
{
    /// <summary>
    /// Gets or sets the comments for the entity.
    /// </summary>
    string? Comments { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is disabled.
    /// </summary>
    bool Disabled { get; set; }

    /// <summary>
    /// Gets a value indicating whether the entity has changes.
    /// </summary>
    bool HasChanges { get; }

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the entity.
    /// </summary>
    string Name { get; set; }
}