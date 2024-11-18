// <copyright file="IWebAppApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.Application.Modules.Applications;

/// <summary>
/// Represents the definition of an application.
/// </summary>
public interface IWebAppApplication : IUIApplication
{
    /// <summary>
    /// Gets the shared modules associated with the application.
    /// </summary>
    IEnumerable<Type> WebAppModules { get; }
}