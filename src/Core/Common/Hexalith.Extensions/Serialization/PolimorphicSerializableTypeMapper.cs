// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Serialization
// Author           : Jérôme Piquot
// Created          : 03-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-10-2023
// ***********************************************************************
// <copyright file="PolimorphicSerializableTypeMapper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Hexalith.Domain.Abstractions.Converters;

namespace Hexalith.Extensions.Serialization;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class MessageSerializationMapper.
/// </summary>
public static class PolimorphicSerializableTypeMapper
{
    /// <summary>
    /// The map.
    /// </summary>
    private static Dictionary<string, Type>? _map;

    /// <summary>
    /// Gets the map.
    /// </summary>
    /// <value>The map.</value>
    private static Dictionary<string, Type> Map => _map ??= GetMap();

    /// <summary>
    /// Gets the map.
    /// </summary>
    /// <returns>Dictionary&lt;System.String, Type&gt;.</returns>
    /// <exception cref="TypeInitializationException">null.</exception>
    /// <exception cref="InvalidOperationException">Type {type.FullName} could not be added to the serialization mapper. A type with TypeName='{obj.TypeName}' and Version='{obj.MajorVersion}.{obj.MinorVersion}' already exists : {map[key].FullName}.</exception>
    public static Dictionary<string, Type> GetMap()
    {
        Dictionary<string, Type> map = new();
        IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IPolymorphicSerializable).IsAssignableFrom(t));
        foreach (Type? type in types)
        {
            IPolymorphicSerializable obj = Activator.CreateInstance(type) as IPolymorphicSerializable ?? throw new TypeInitializationException(type.AssemblyQualifiedName, null);
            string key = $"{obj.TypeName}|{obj.MajorVersion}.{obj.MinorVersion}";
            try
            {
                map.Add(key, type);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Type {type.FullName} could not be added to the serialization mapper. A type with TypeName='{obj.TypeName}' and Version='{obj.MajorVersion}.{obj.MinorVersion}' already exists : {map[key].FullName}", ex);
            }
        }

        return map;
    }

    /// <summary>
    /// Gets the type.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="majorVersion">The major version.</param>
    /// <param name="minorVersion">The minor version.</param>
    /// <returns>Type.</returns>
    /// <exception cref="InvalidOperationException">Base message class with MessageName='{name}' and Version='{majorVersion}.{minorVersion}' not found.</exception>
    /// <exception cref="TypeInitializationException">null.</exception>
    public static Type GetType(string name, int majorVersion, int minorVersion)
    {
        try
        {
            return Map[$"{name}|{majorVersion}.{minorVersion}"];
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidOperationException($"Base message class with MessageName='{name}' and Version='{majorVersion}.{minorVersion}' not found.", ex);
        }
    }
}