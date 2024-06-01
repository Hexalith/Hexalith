// <copyright file="IApplicationDefinition.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.Application.Modules.Applications;

/// <summary>
/// Represents the definition of an application.
/// </summary>
public interface IApplicationDefinition
{
    /// <summary>
    /// Gets the home path of the application.
    /// </summary>
    string HomePath { get; }

    /// <summary>
    /// Gets the login path of the application.
    /// </summary>
    string LoginPath { get; }

    /// <summary>
    /// Gets the logout path of the application.
    /// </summary>
    string LogoutPath { get; }

    /// <summary>
    /// Gets the modules associated with the application.
    /// </summary>
    IEnumerable<Type> Modules { get; }

    /// <summary>
    /// Gets the shared modules associated with the application.
    /// </summary>
    IEnumerable<Type> SharedModules { get; }
}