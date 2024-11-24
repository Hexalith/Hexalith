// <copyright file="PolymorphicSerializationResolver.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerialization;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Resolves the polymorphic serialization for JSON by extending the DefaultJsonTypeInfoResolver.
/// This class manages the mapping between base types and their derived types for JSON serialization.
/// </summary>
public class PolymorphicSerializationResolver : DefaultJsonTypeInfoResolver
{
    /// <summary>
    /// A thread-safe dictionary to store the mapping between base types and their serialization mappers.
    /// </summary>
    private static readonly ConcurrentDictionary<Type, IEnumerable<IPolymorphicSerializationMapper>> _serializationMappers = new();

    /// <summary>
    /// Attempts to add a default mapper to the static collection of default mappers.
    /// </summary>
    /// <param name="mapper">The mapper to be added to the default mappers.</param>
    public static void TryAddDefaultMapper(IPolymorphicSerializationMapper mapper)
    {
        if (!_serializationMappers
            .SelectMany(m => m.Value)
            .Any(m => m.JsonDerivedType.DerivedType == mapper.JsonDerivedType.DerivedType))
        {
            _serializationMappers[mapper.Base] = _serializationMappers.TryGetValue(mapper.Base, out IEnumerable<IPolymorphicSerializationMapper>? mappers)
                ? mappers.Append(mapper)
                : [mapper];
        }
    }

    /// <summary>
    /// Attempts to add multiple default mappers to the static collection of default mappers.
    /// </summary>
    /// <param name="mappers">The collection of mappers to be added to the default mappers.</param>
    public static void TryAddDefaultMappers(IEnumerable<IPolymorphicSerializationMapper> mappers)
    {
        foreach (IPolymorphicSerializationMapper item in mappers)
        {
            TryAddDefaultMapper(item);
        }
    }

    /// <summary>
    /// Overrides the base GetTypeInfo method to provide custom type information for JSON serialization.
    /// </summary>
    /// <param name="type">The type for which to retrieve JSON type information.</param>
    /// <param name="options">The JSON serializer options.</param>
    /// <returns>A JsonTypeInfo object containing the type information for serialization.</returns>
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        if (_serializationMappers.TryGetValue(jsonTypeInfo.Type, out IEnumerable<IPolymorphicSerializationMapper>? mappers))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = PolymorphicHelper.Discriminator,
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
            };
            IEnumerable<JsonDerivedType> derivedTypes = mappers.Select(p => p.JsonDerivedType).Distinct();
            foreach (JsonDerivedType derivedType in derivedTypes)
            {
                jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(derivedType);
            }
        }

        return jsonTypeInfo;
    }
}