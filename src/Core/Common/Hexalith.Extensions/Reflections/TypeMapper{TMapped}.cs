// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Serialization
// Author           : Jérôme Piquot
// Created          : 03-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-12-2023
// ***********************************************************************
// <copyright file="TypeMapper{TMapped}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Extensions.Reflections;

using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Class MessageSerializationMapper.
/// </summary>
/// <typeparam name="TMapped">The type of the t mapped.</typeparam>
public static class TypeMapper<TMapped>
    where TMapped : IMappableType
{
    /// <summary>
    /// The lock.
    /// </summary>
    private static readonly object _lock = new();

    /// <summary>
    /// The map.
    /// </summary>
    private static Dictionary<string, TMapped>? _map;

    /// <summary>
    /// Gets the map.
    /// </summary>
    /// <value>The map.</value>
    private static Dictionary<string, TMapped> Map
    {
        get
        {
            if (_map == null)
            {
                lock (_lock)
                {
#pragma warning disable CA1508 // Avoid dead conditional code
                    _map ??= GetMap();
#pragma warning restore CA1508 // Avoid dead conditional code
                }
            }

            return _map;
        }
    }

    /// <summary>
    /// Gets the map.
    /// </summary>
    /// <returns>Dictionary&lt;System.String, Type&gt;.</returns>
    /// <exception cref="System.TypeInitializationException">null.</exception>
    /// <exception cref="System.InvalidOperationException">Type {type.FullName} could not be added to the serialization mapper. Name '{obj.TypeMapName}' already exists ans is associated to '{map[key].FullName}'.</exception>
#pragma warning disable CA1000 // Do not declare static members on generic types

    public static Dictionary<string, TMapped> GetMap()
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        Dictionary<string, TMapped> map =[];
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic))
        {
            try
            {
                foreach (Type? type in assembly.GetTypes())
                {
                    if (type != null && type != typeof(TMapped) && type.IsClass && !type.IsAbstract && typeof(TMapped).IsAssignableFrom(type))
                    {
                        TMapped obj;
                        try
                        {
                            obj = (TMapped)(Activator.CreateInstance(type) ?? throw new TypeInitializationException(type.AssemblyQualifiedName, null));
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException($"Type {type.FullName} could not be initialized. Verify that the class has default constructor with no parameters.", ex);
                        }

                        string key = obj.TypeMapName;
                        try
                        {
                            map.Add(key, obj);
                        }
                        catch (ArgumentException ex)
                        {
                            throw new InvalidOperationException($"Type {type.FullName} could not be added to the serialization mapper. Name '{obj.TypeMapName}' already exists ans is associated to '{map[key].GetType().FullName}'.", ex);
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException)
            {
                continue;
            }
        }

        return map;
    }

    /// <summary>
    /// Gets the object instance.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>object.</returns>
    /// <exception cref="System.InvalidOperationException">Mappable type with name '{name}' not found.</exception>
#pragma warning disable CA1000 // Do not declare static members on generic types

    public static object GetObject(string name)
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        try
        {
            return Map[name];
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidOperationException($"Mappable type with name '{name}' not found.", ex);
        }
    }

    /// <summary>
    /// Gets the type.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>Type.</returns>
    /// <exception cref="System.InvalidOperationException">Mappable type with name '{name}' not found.</exception>
#pragma warning disable CA1000 // Do not declare static members on generic types

    public static Type GetType(string name) => GetObject(name).GetType();

#pragma warning restore CA1000 // Do not declare static members on generic types
}