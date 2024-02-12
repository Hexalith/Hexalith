// <copyright file="IModule.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules;

/// <summary>
/// Represents a module in the application.
/// </summary>
/// <remarks>
/// A module is a self-contained unit of functionality in the application.
/// It provides a set of actions that can be performed and has a name, description, path, and version.
/// </remarks>
public interface IApplicationModule
{
    /// <summary>
    /// Gets the description of the module.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the path of the module.
    /// </summary>
    string Path { get; }

    /// <summary>
    /// Gets the version of the module.
    /// </summary>
    string Version { get; }
}