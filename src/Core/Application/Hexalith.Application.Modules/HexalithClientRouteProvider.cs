// <copyright file="HexalithClientRouteProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules;

using Hexalith.Application.Modules.Routes;

/// <summary>
/// Represents the route provider for the HexalithClient module.
/// </summary>
public sealed class HexalithClientRouteProvider : IRouteProvider
{
    /// <inheritdoc/>
    public string MapPath(string path)
        => string.IsNullOrWhiteSpace(path) ? "/hexalith" : path;
}