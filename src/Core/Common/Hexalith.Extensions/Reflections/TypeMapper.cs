// <copyright file="TypeMapper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Reflections;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

/// <summary>
/// Type mapper.
/// </summary>
public static class TypeMapper
{
    /// <summary>
    /// Determines whether the specified type is mappable.
    /// </summary>
    /// <typeparam name="TMappable">The type of the mappable object.</typeparam>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool CanCreateMappableType<TMappable>([NotNull] Type type)
    {
        return IsMappableConcreteClass<TMappable>(type)
            && type.GetConstructor(Type.EmptyTypes)?.IsPublic == true;
    }

    /// <summary>
    /// Gets the map of mappable types.
    /// </summary>
    /// <typeparam name="TMappable">The type of the mappable object.</typeparam>
    /// <returns>A dictionary containing the map of mappable types.</returns>
    public static FrozenDictionary<string, TMappable> GetMap<TMappable>()
       where TMappable : IMappableType
    {
        Dictionary<string, TMappable> map = [];
        foreach (Type type in GetMappableTypes<TMappable>())
        {
            TMappable obj;
            try
            {
                obj = (TMappable)(Activator.CreateInstance(type) ?? throw new TypeInitializationException(type.AssemblyQualifiedName, null));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Type {type.FullName} could not be initialized. Verify that the class has a default constructor with no parameters.", ex);
            }

            string key = obj.TypeMapName;
            try
            {
                if (map.TryGetValue(key, out TMappable? value))
                {
                    Debug.WriteLine($"Type {type.AssemblyQualifiedName} could not be added to the serialization mapper."
                        + $" Name '{obj.TypeMapName}' already exists and is associated with '{value.GetType().AssemblyQualifiedName}'.");
                    continue;
                }

                map.Add(key, obj);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException(
                    $"Type {type.FullName} could not be added to the serialization mapper. Name '{obj.TypeMapName}' already exists and is associated with '{map[key].GetType().FullName}'.", ex);
            }
        }

        return map.ToFrozenDictionary();
    }

    /// <summary>
    /// Gets the mappable types.
    /// </summary>
    /// <typeparam name="TMappable">The type of the mappable object.</typeparam>
    /// <returns>System.Collections.Generic.IEnumerable&lt;System.Type&gt;.</returns>
    public static IEnumerable<Type> GetMappableTypes<TMappable>()
    {
        List<Type> map = [];
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic))
        {
            try
            {
                foreach (Type? type in assembly.GetTypes().Where(CanCreateMappableType<TMappable>))
                {
                    map.Add(type);
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
    /// Gets the mappable object by name.
    /// </summary>
    /// <typeparam name="TMappable">The type of the mappable object.</typeparam>
    /// <param name="name">The name of the mappable object.</param>
    /// <returns>The mappable object.</returns>
    public static TMappable GetObject<TMappable>(string name)
        where TMappable : IMappableType
    {
        try
        {
            return NameTypeMapper<TMappable>.Map[name];
        }
        catch (KeyNotFoundException ex)
        {
            throw new InvalidOperationException($"Mappable type with name '{name}' not found.", ex);
        }
    }

    /// <summary>
    /// Gets the type of the mappable object by name.
    /// </summary>
    /// <typeparam name="TMappable">The type of the mappable tree parent.</typeparam>
    /// <param name="name">The name of the mappable object.</param>
    /// <returns>The type of the mappable object.</returns>
    public static Type GetType<TMappable>(string name)
        where TMappable : IMappableType
        => GetObject<TMappable>(name).GetType();

    /// <summary>
    /// Determines whether the specified type is a mappable concrete class.
    /// </summary>
    /// <typeparam name="TMappable">The type of the mappable object.</typeparam>
    /// <param name="type">The type.</param>
    /// <returns>True if the specified type is a mappable concrete class; otherwise, false.</returns>
    public static bool IsMappableConcreteClass<TMappable>([NotNull] Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return type != typeof(TMappable)
            && type.IsClass
            && !type.IsAbstract
            && typeof(TMappable).IsAssignableFrom(type);
    }
}