// <copyright file="IRouteProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Routes;

/// <summary>
/// Represents a route provider.
/// </summary>
public interface IRouteProvider
{
    /// <summary>
    /// Maps the specified path.
    /// </summary>
    /// <param name="path">The path to map.</param>
    /// <returns>The mapped path.</returns>
    string MapPath(string path);
}