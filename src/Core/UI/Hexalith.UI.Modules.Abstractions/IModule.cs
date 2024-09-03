// <copyright file="IModule.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Modules.Abstractions;

/// <summary>
/// Represents a module in the Hexalith UI framework.
/// </summary>
/// <remarks>
/// This interface defines the properties that a module must implement.
/// </remarks>
public interface IModule
{
    /// <summary>
    /// Gets the author of the module.
    /// </summary>
    string Author { get; }

    /// <summary>
    /// Gets the company of the module.
    /// </summary>
    string Company { get; }

    /// <summary>
    /// Gets the description of the module.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the version of the module.
    /// </summary>
    string Version { get; }
}