// <copyright file="ITheme.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Modules.Abstractions;

/// <summary>
/// Represents a theme in the Hexalith UI framework.
/// </summary>
/// <remarks>
/// This interface defines the properties that a theme must implement.
/// </remarks>
public interface ITheme
{
    /// <summary>
    /// Gets the author of the theme.
    /// </summary>
    string Author { get; }

    /// <summary>
    /// Gets the company of the theme.
    /// </summary>
    string Company { get; }

    /// <summary>
    /// Gets the description of the theme.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the layout fully qualified type name of the theme.
    /// </summary>
    string LayoutName { get; }

    /// <summary>
    /// Gets the name of the theme.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the version of the theme.
    /// </summary>
    string Version { get; }
}