// <copyright file="IMappableType.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Reflections;

/// <summary>
/// Interface ITypeMappable.
/// </summary>
public interface IMappableType
{
    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    /// <value>The name of the type.</value>
    string TypeMapName { get; }
}