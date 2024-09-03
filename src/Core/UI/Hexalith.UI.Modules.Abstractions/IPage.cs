// <copyright file="IPage.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Modules.Abstractions;

/// <summary>
/// Represents a page in the UI module.
/// </summary>
/// <remarks>
/// This interface defines the properties of a page, including its description, name, title, and version.
/// </remarks>
public interface IPage
{
    /// <summary>
    /// Gets the description of the page.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the name of the page.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the title of the page.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Gets the version of the page.
    /// </summary>
    string Version { get; }
}