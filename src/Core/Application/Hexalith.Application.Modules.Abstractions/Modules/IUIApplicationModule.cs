// <copyright file="ISharedUIElementsApplicationModule - Copy.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Modules;

using System.Reflection;

/// <summary>
/// Represents a shared module in the application.
/// </summary>
/// <remarks>
/// A module is a self-contained unit of functionality in the application.
/// It provides a set of actions that can be performed and has a name, description, path, and version.
/// </remarks>
public interface IUIApplicationModule : IApplicationModule
{
    /// <summary>
    /// Gets the module UI assemblies.
    /// </summary>
    /// <value>The UI assemblies.</value>
    IEnumerable<Assembly> PresentationAssemblies { get; }
}