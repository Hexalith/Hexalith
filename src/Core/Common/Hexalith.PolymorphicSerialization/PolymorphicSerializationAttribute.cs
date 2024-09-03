// <copyright file="PolymorphicSerializationAttribute.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
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
public sealed class PolymorphicSerializationAttribute(string name, int version, Type? baseType = null) : Attribute
{
    /// <summary>
    /// Gets the base type.
    /// </summary>
    public Type? BaseType { get; } = baseType;

    /// <summary>
    /// Gets the name of the message.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the type name.
    /// </summary>
    public string TypeName => GetTypeName(Name, Version);

    /// <summary>
    /// Gets the version of the message.
    /// </summary>
    public int Version { get; } = version;

    /// <summary>
    /// Gets the type name for the specified name and version.
    /// </summary>
    /// <param name="name">The name of the message.</param>
    /// <param name="version">The version of the message.</param>
    /// <returns>The type name.</returns>
    public static string GetTypeName(string name, int version) => name + "V" + version;
}