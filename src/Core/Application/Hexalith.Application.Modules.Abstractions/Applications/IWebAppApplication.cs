// <copyright file="IWebAppApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.Application.Modules.Applications;

using System.Reflection;

/// <summary>
/// Represents the definition of an application.
/// </summary>
public interface IWebAppApplication : IApplication
{
    /// <summary>
    /// Gets the shared modules associated with the application.
    /// </summary>
    IEnumerable<Type> WebAppModules { get; }

    /// <summary>
    /// Gets the presentation assemblies associated with the application.
    /// </summary>
    public IEnumerable<Assembly> PresentationAssemblies { get; }

    /// <summary>
    /// Gets the shared application type.
    /// Must be a type that implements <see cref="ISharedUIElementsApplication"/>.
    /// </summary>
    Type SharedUIElementsApplicationType { get; }
}