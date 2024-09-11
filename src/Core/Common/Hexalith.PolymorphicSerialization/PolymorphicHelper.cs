// <copyright file="PolymorphicHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.PolymorphicSerialization;

using System;

/// <summary>
/// Provides helper methods for polymorphic serialization.
/// </summary>
public static class PolymorphicHelper
{
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