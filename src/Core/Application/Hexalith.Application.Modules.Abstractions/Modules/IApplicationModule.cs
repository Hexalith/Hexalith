// <copyright file="IApplicationModule.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Modules;

using System.Reflection;

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
    /// Gets the dependencies.
    /// </summary>
    /// <value>The dependencies.</value>
    IEnumerable<string> Dependencies { get; }

    /// <summary>
    /// Gets the description of the module.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the unique identifier of the module.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the order weight.
    /// </summary>
    /// <value>The order weight.</value>
    int OrderWeight { get; }

    /// <summary>
    /// Gets the path of the module.
    /// </summary>
    string Path { get; }

    /// <summary>
    /// Gets the module UI assemblies.
    /// </summary>
    /// <value>The UI assemblies.</value>
    IEnumerable<Assembly> PresentationAssemblies { get; }

    /// <summary>
    /// Gets the version of the module.
    /// </summary>
    string Version { get; }

    /// <summary>
    /// Setup the module in the application. For example, maps routes, etc.
    /// </summary>
    /// <param name="builder">The host application.</param>
    void UseModule(object builder);
}