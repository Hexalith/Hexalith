// <copyright file="JsonPolymorphicDerivedTypeAttribute.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Serialization;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

/// <summary>
/// Attribute to mark a class as a polymorphic type.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="JsonPolymorphicDerivedTypeAttribute"/> class.
/// </remarks>
/// <param name="typeName">The name of the type.</param>
/// <param name="version">The version number.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class JsonPolymorphicDerivedTypeAttribute(string typeName, int version) : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JsonPolymorphicDerivedTypeAttribute"/> class.
    /// </summary>
    /// <param name="typeName">The name of the type.</param>
    public JsonPolymorphicDerivedTypeAttribute(string typeName)
        : this(typeName, 1)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonPolymorphicDerivedTypeAttribute"/> class.
    /// </summary>
    public JsonPolymorphicDerivedTypeAttribute()
        : this(string.Empty, 1)
    {
    }

    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    public string TypeName { get; } = typeName;

    /// <summary>
    /// Gets the version number.
    /// </summary>
    public int Version { get; } = version;

    /// <summary>
    /// Gets the default type map name.
    /// </summary>
    /// <param name="type">Type of the derived instance.</param>
    /// <returns>The type name.</returns>
    public static string GetDefaultTypeMapName([NotNull] MemberInfo type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return $"{type.Name}|V1";
    }

    /// <summary>
    /// Gets the type map name.
    /// </summary>
    /// <typeparam name="T">The polymorphic object type parameter.</typeparam>
    /// <returns>The formatted type with the name and version.</returns>
    public string GetTypeMapName<T>() => GetTypeMapName(typeof(T));

    /// <summary>
    /// Gets the type map name.
    /// </summary>
    /// <param name="type">The polymorphic object type.</param>
    /// <returns>The formatted type map name with the name and version.</returns>
    public string GetTypeMapName([NotNull] MemberInfo type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return $"{(string.IsNullOrWhiteSpace(TypeName) ? type.Name : TypeName)}|V{Version}";
    }
}