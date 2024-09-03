// <copyright file="IClientApplication.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.Application.Modules.Applications;

using System.Reflection;

/// <summary>
/// Represents the definition of an application.
/// </summary>
public interface IClientApplication : IApplication
{
    /// <summary>
    /// Gets the shared modules associated with the application.
    /// </summary>
    IEnumerable<Type> ClientModules { get; }

    /// <summary>
    /// Gets the presentation assemblies associated with the application.
    /// </summary>
    public IEnumerable<Assembly> PresentationAssemblies { get; }

    /// <summary>
    /// Gets the shared application type.
    /// Must be a type that implements <see cref="ISharedApplication"/>.
    /// </summary>
    Type SharedApplicationType { get; }
}