// <copyright file="JsonPolymorphicTypeRegistry.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Serialization;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Provides a custom type registry for handling polymorphic types during JSON serialization and deserialization.
/// </summary>
public class JsonPolymorphicTypeRegistry : IJsonPolymorphicTypeRegistry
{
    private readonly Dictionary<Type, List<JsonDerivedType>> _derivedTypes = [];

    /// <summary>
    /// Get the polymorphic base type marked with the <see cref="JsonPolymorphicBaseTypeAttribute"/>.
    /// </summary>
    /// <typeparam name="T">The polymorphic type.</typeparam>
    /// <returns>The polymorphic base type.</returns>
    public static Type? GetPolymorphicBaseType<T>() => GetPolymorphicBaseType(typeof(T));

    /// <summary>
    /// Get the polymorphic base type marked with the <see cref="JsonPolymorphicBaseTypeAttribute"/>.
    /// </summary>
    /// <param name="type">The polymorphic type.</param>
    /// <returns>The polymorphic base type.</returns>
    /// <exception cref="InvalidOperationException">Null type argument.</exception>
    public static Type? GetPolymorphicBaseType([NotNull] Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        // Find the base type marked with the JsonPolymorphicBaseTypeAttribute
        Type? baseType = type;
        Type? polymorphicBaseType = null;
        while (baseType is not null)
        {
            if (baseType.GetCustomAttribute<JsonPolymorphicBaseTypeAttribute>() is not null)
            {
                if (polymorphicBaseType is not null)
                {
                    // Duplicate base types marked with the JsonPolymorphicBaseTypeAttribute found
                    throw new InvalidOperationException($"Multiple base types marked with the {nameof(JsonPolymorphicBaseTypeAttribute)} found for type {type.Name}.");
                }

                polymorphicBaseType = baseType;
            }

            baseType = baseType.BaseType;
        }

        return polymorphicBaseType;
    }

    /// <inheritdoc/>
    public void AddDerivedTypesToList([NotNull] Type polymorphicBaseType, [NotNull] IList<JsonDerivedType> derivedTypesList)
    {
        ArgumentNullException.ThrowIfNull(polymorphicBaseType);
        ArgumentNullException.ThrowIfNull(derivedTypesList);
        if (derivedTypesList.Count > 0)
        {
            throw new InvalidOperationException("The derived types list must be empty.");
        }

        foreach (JsonDerivedType type in GetDerivedTypes(polymorphicBaseType))
        {
            derivedTypesList.Add(type);
        }
    }

    /// <inheritdoc/>
    public IEnumerable<JsonDerivedType> GetDerivedTypes([NotNull] Type polymorphicBaseType)
    {
        ArgumentNullException.ThrowIfNull(polymorphicBaseType);

        return _derivedTypes.TryGetValue(polymorphicBaseType, out List<JsonDerivedType>? baseDerivedTypes)
            ? [.. baseDerivedTypes]
            : throw new InvalidOperationException($"Base type {polymorphicBaseType.Name} not found in the derived types registry.");
    }

    /// <summary>
    /// Registers a derived type to be included in the polymorphic type resolution.
    /// </summary>
    /// <typeparam name="T">The derived type to register.</typeparam>
    public void RegisterJsonDerivedType<T>() => RegisterJsonDerivedType(typeof(T));

    /// <summary>
    /// Registers a derived type to be included in the polymorphic type resolution.
    /// </summary>
    /// <param name="type">The derived type to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="type"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the <paramref name="type"/> is not marked with the <see cref="JsonPolymorphicDerivedTypeAttribute"/>.</exception>
    public void RegisterJsonDerivedType([NotNull] Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        Type polymorphicBaseType = GetPolymorphicBaseType(type) ?? throw new InvalidOperationException($"Type {type.Name} has no base type marked with the {nameof(JsonPolymorphicBaseTypeAttribute)}.");
        JsonPolymorphicDerivedTypeAttribute? attribute = type.GetCustomAttribute<JsonPolymorphicDerivedTypeAttribute>();

        // Call recursively register json derived type for all base types marked with the JsonPolymorphicTypeAttribute
        Type? baseType = type.BaseType;
        if (baseType is null || type == polymorphicBaseType)
        {
            return;
        }

        string typeName = attribute == null
            ? JsonPolymorphicDerivedTypeAttribute.GetDefaultTypeMapName(type)
            : attribute.GetTypeMapName(type);
        RegisterJsonDerivedType(baseType);
        JsonDerivedType jsonDerivedType = new(
            type,
            typeName);
        if (_derivedTypes.TryGetValue(polymorphicBaseType, out List<JsonDerivedType>? baseDerivedTypes))
        {
            baseDerivedTypes.Add(jsonDerivedType);
        }
        else
        {
            _derivedTypes.Add(polymorphicBaseType, [jsonDerivedType]);
        }
    }
}