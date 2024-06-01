// <copyright file="IClientApplication.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.Application.Modules.Applications;

/// <summary>
/// Represents the definition of an application.
/// </summary>
public interface IClientApplication : IApplicationDefinition
{
    /// <summary>
    /// Gets the shared modules associated with the application.
    /// </summary>
    IEnumerable<Type> ClientModules { get; }
}