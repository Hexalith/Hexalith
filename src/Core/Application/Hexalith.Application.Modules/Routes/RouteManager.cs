// <copyright file="RouteManager.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Routes;

using System.Collections.Generic;

/// <summary>
/// Manages route aliases.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RouteManager"/> class.
/// </remarks>
/// <param name="aliasProviders">The alias providers.</param>
public class RouteManager(IEnumerable<IRouteProvider> aliasProviders) : IRouteManager
{
    /// <inheritdoc/>
    public string MapPath(string path)
    {
        string newPath = path;
        foreach (IRouteProvider aliasProvider in aliasProviders)
        {
            newPath = aliasProvider.MapPath(newPath);
        }

        return newPath;
    }
}