// <copyright file="IModuleService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Services;

/// <summary>
/// Represents an module service.
/// </summary>
public interface IModuleService
{
    /// <summary>
    /// Gets the description of the module.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets a value indicating whether the module is an application module.
    /// </summary>
    public bool IsApplicationModule { get; }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the version of the module.
    /// </summary>
    public string Version { get; }
}