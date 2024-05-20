// <copyright file="ModuleType.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Modules;

/// <summary>
/// Represents the type of a module.
/// </summary>
public enum ModuleType
{
    /// <summary>
    /// Shared module type.
    /// </summary>
    Shared = 0,

    /// <summary>
    /// Server module type.
    /// </summary>
    Server = 1,

    /// <summary>
    /// Client module type.
    /// </summary>
    Client = 2,

    /// <summary>
    /// Store app module type.
    /// </summary>
    StoreApp = 3,
}