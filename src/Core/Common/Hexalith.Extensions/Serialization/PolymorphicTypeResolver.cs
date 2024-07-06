// <copyright file="PolymorphicTypeResolver.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Serialization;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Provides a custom type resolver for handling polymorphic types during JSON serialization and deserialization.
/// </summary>
public class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
{
    private static readonly Dictionary<Type, List<JsonDerivedType>> _derivedTypes = [];

    /// <summary>
    /// Registers a derived type to be included in the polymorphic type resolution.
    /// </summary>
    /// <typeparam name="T">The derived type to register.</typeparam>
    public static void RegisterJsonDerivedType<T>() => RegisterJsonDerivedType(typeof(T));

    /// <summary>
    /// Registers a derived type to be included in the polymorphic type resolution.
    /// </summary>
    /// <param name="type">The derived type to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="type"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the <paramref name="type"/> is not marked with the <see cref="JsonPolymorphicTypeAttribute"/>.</exception>
    public static void RegisterJsonDerivedType([NotNull] Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        JsonPolymorphicTypeAttribute? attribute = type.GetCustomAttribute<JsonPolymorphicTypeAttribute>()
            ?? throw new InvalidOperationException($"Type {type.Name} is not marked with the {nameof(JsonPolymorphicTypeAttribute)}.");

        // Call recursively register json derived type for all base types marked with the JsonPolymorphicTypeAttribute
        Type? baseType = type.BaseType;
        if (baseType is not null && baseType.GetCustomAttribute<JsonPolymorphicTypeAttribute>() is not null)
        {
            RegisterJsonDerivedType(baseType);
            JsonDerivedType jsonDerivedType = new(type, attribute.GetTypeMapName(type));
            if (_derivedTypes.TryGetValue(baseType, out List<JsonDerivedType>? baseDerivedTypes))
            {
                baseDerivedTypes.Add(jsonDerivedType);
            }
            else
            {
                _derivedTypes.Add(baseType, [jsonDerivedType]);
            }
        }
    }

    /// <inheritdoc/>
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);
        if (jsonTypeInfo.Type.GetCustomAttribute(typeof(JsonPolymorphicTypeAttribute)) != null)
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
            };
            foreach (JsonDerivedType derivedType in _derivedTypes[type])
            {
                jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(derivedType);
            }
        }

        return jsonTypeInfo;
    }
}