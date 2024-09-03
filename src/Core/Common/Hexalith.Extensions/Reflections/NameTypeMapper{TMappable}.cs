// <copyright file="NameTypeMapper{TMappable}.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Reflections;

using System.Collections.Frozen;

/// <summary>
/// Provides a mapping between string keys and a mappable type.
/// </summary>
/// <typeparam name="TMappable">The type of the mappable object.</typeparam>
internal static class NameTypeMapper<TMappable>
    where TMappable : IMappableType
{
    private static FrozenDictionary<string, TMappable>? _map;

    /// <summary>
    /// Gets the mapping between string keys and the mappable type.
    /// </summary>
    internal static FrozenDictionary<string, TMappable> Map => _map ??= TypeMapper.GetMap<TMappable>();
}