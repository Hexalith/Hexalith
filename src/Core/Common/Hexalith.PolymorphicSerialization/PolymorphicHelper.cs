// <copyright file="PolymorphicHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerialization;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Provides helper methods for polymorphic serialization.
/// </summary>
public static class PolymorphicHelper
{
    private static JsonSerializerOptions? _defaultJsonSerializerOptions;

    /// <summary>
    /// Gets the discriminator for polymorphic serialization.
    /// </summary>
    public static string Discriminator => "$type";

    /// <summary>
    /// Gets the default JSON serializer options.
    /// </summary>
    public static JsonSerializerOptions DefaultJsonSerializerOptions => _defaultJsonSerializerOptions ??=
        new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            TypeInfoResolver = new PolymorphicSerializationResolver(),
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };

    /// <summary>
    /// Gets the name, type name and version of a polymorphic type.
    /// </summary>
    /// <param name="type">The type to get the name, type name and version.</param>
    /// <returns>The name, type name and version of the polymorphic type.</returns>
    public static (string Name, string TypeName, int Version) GetPolymorphicTypeDiscriminator(this Type type)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        string? typeName = type.Name ?? throw new InvalidOperationException("The type name is null.");

        // Get the PolymorphicSerializationAttribute
        return Attribute.GetCustomAttribute(type, typeof(PolymorphicSerializationAttribute)) is not PolymorphicSerializationAttribute attribute
            ? (typeName, PolymorphicSerializationAttribute.GetTypeName(typeName, 1), 1)
            : (attribute.Name ?? typeName, attribute.GetTypeName(type), attribute.Version);
    }

    /// <summary>
    /// Gets the name, type name and version of a polymorphic type.
    /// </summary>
    /// <param name="instance">The instance to get the name, type name and version.</param>
    /// <returns>The name, type name and version of the polymorphic type.</returns>
    /// <exception cref="ArgumentNullException">The instance is null.</exception>
    public static (string Name, string TypeName, int Version) GetPolymorphicTypeDiscriminator(this object instance)
        => instance == null ? throw new ArgumentNullException(nameof(instance)) : instance.GetType().GetPolymorphicTypeDiscriminator();
}