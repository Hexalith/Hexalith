// <copyright file="IApiServerApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.Application.Modules.Applications;

/// <summary>
/// Represents the definition of an application.
/// </summary>
public interface IApiServerApplication : IApplication
{
    /// <summary>
    /// Gets the server modules associated with the application.
    /// </summary>
    IEnumerable<Type> ApiServerModules { get; }

    /// <summary>
    /// Gets the shared application type.
    /// Must be a type that implements <see cref="ISharedUIElementsApplication"/>.
    /// </summary>
    Type SharedUIElementsApplicationType { get; }

    /// <summary>
    /// Registers the actors associated with the application.
    /// </summary>
    /// <param name="actors">The actor collection.</param>
    void RegisterActors(object actors);
}