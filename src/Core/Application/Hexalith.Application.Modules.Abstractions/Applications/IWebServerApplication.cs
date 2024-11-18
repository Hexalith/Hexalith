// <copyright file="IWebServerApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.Application.Modules.Applications;

/// <summary>
/// Represents the definition of an application.
/// </summary>
public interface IWebServerApplication : IUIApplication
{
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