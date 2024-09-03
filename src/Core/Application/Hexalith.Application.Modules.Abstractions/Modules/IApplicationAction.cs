// <copyright file="IApplicationAction.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Modules;
/// <summary>
/// Represents a module action.
/// </summary>
/// <remarks>
/// This interface defines the properties of a module action.
/// </remarks>
public interface IApplicationAction
{
    /// <summary>
    /// Gets the description of the module action.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the name of the module action.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the parent module for this action.
    /// </summary>
    IApplicationModule ParentModule { get; }

    /// <summary>
    /// Gets the path of the module action.
    /// </summary>
    string Path { get; }

    /// <summary>
    /// Gets the required role for this module action.
    /// </summary>
    string Role { get; }
}