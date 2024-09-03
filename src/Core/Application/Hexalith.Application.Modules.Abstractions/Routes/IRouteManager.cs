// <copyright file="IRouteManager.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Routes;

/// <summary>
/// Represents a route alias manager.
/// </summary>
public interface IRouteManager
{
    /// <summary>
    /// Converts an URL path into a new one based on alias definition, tenant management or other specific route providers.
    /// </summary>
    /// <param name="path">The path of the route.</param>
    /// <returns>The route associated with the alias.</returns>
    string MapPath(string path);
}