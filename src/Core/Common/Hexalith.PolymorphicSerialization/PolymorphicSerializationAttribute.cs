// <copyright file="PolymorphicSerializationAttribute.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerialization;

using System;

/// <summary>
/// Represents a custom attribute used to provide metadata for a message.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PolymorphicSerializationAttribute"/> class with the specified name, major version, and minor version.
/// </remarks>
/// <param name="name">The name of the message.</param>
/// <param name="version">The version of the message.</param>
/// <param name="baseType">The base type of the message.</param>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PolymorphicSerializationAttribute(string? name = null, int version = 1, Type? baseType = null) : Attribute
{
    /// <summary>
    /// Gets the base type.
    /// </summary>
    public Type? BaseType { get; } = baseType;

    /// <summary>
    /// Gets the name of the message.
    /// </summary>
    public string? Name { get; } = name;

    /// <summary>
    /// Gets the version of the message.
    /// </summary>
    public int Version { get; } = version;

    /// <summary>
    /// Gets the type name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="version">The version.</param>
    /// <returns>The type name.</returns>
    public static string GetTypeName(string name, int version)
            => (version < 2) ? name : $"{name}V{version}";

    /// <summary>
    /// Gets the polymorphic type name.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The polymorphic type name.</returns>
    /// <exception cref="ArgumentNullException">The type is null.</exception>
    public string GetTypeName(Type type)
    {
        string? name = string.IsNullOrWhiteSpace(Name) ? type.Name : Name;
        _ = name ?? throw new InvalidOperationException("The type name is null.");
        return GetTypeName(
            name,
            Version);
    }
}