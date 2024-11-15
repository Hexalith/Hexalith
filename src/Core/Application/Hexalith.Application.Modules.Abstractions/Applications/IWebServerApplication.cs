﻿// <copyright file="IWebServerApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.Application.Modules.Applications;

using System.Reflection;

/// <summary>
/// Represents the definition of an application.
/// </summary>
public interface IWebServerApplication : IApplication
{
    /// <summary>
    /// Gets the presentation assemblies associated with the application.
    /// </summary>
    public IEnumerable<Assembly> PresentationAssemblies { get; }

    /// <summary>
    /// Gets the shared application type.
    /// Must be a type that implements <see cref="ISharedAssetsApplication"/>.
    /// </summary>
    Type SharedAssetsApplicationType { get; }

    /// <summary>
    /// Gets the client application type.
    /// Must be a type that implements <see cref="IWebAppApplication"/>.
    /// </summary>
    Type WebAppApplicationType { get; }

    /// <summary>
    /// Gets the server modules associated with the application.
    /// </summary>
    IEnumerable<Type> WebServerModules { get; }
}