// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Serialization
// Author           : Jérôme Piquot
// Created          : 03-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-12-2023
// ***********************************************************************
// <copyright file="IPolymorphicSerializable.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Extensions.Serialization;

using Hexalith.Extensions.Reflections;

/// <summary>
/// Interface IPolymorphicSerializable.
/// </summary>
public interface IPolymorphicSerializable : IMappableType
{
    /// <summary>
    /// Gets the major version property name.
    /// </summary>
    public static string MajorVersionPropertyName => "$version_major";

    /// <summary>
    /// Gets the minor version property name.
    /// </summary>
    public static string MinorVersionPropertyName => "$version_minor";

    /// <summary>
    /// Gets the type name property name.
    /// </summary>
    public static string TypeNamePropertyName => "$type_name";

    /// <summary>
    /// Gets the major version.
    /// </summary>
    /// <value>The major version.</value>
    int MajorVersion { get; }

    /// <summary>
    /// Gets the minor version.
    /// </summary>
    /// <value>The minor version.</value>
    int MinorVersion { get; }

    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    /// <value>The name of the type.</value>
    string TypeName { get; }

    /// <summary>
    /// Gets the name of the type map.
    /// </summary>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="majorVersion">The major version.</param>
    /// <param name="minorVersion">The minor version.</param>
    /// <returns>System.String.</returns>
    public static string GetTypeMapName(string typeName, int majorVersion, int minorVersion) => $"{typeName}|{majorVersion}.{minorVersion}";
}