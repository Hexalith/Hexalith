// <copyright file="ReflectionHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Reflections;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Hexalith.Extensions.Helpers;

/// <summary>
/// Reflection helper.
/// </summary>
public static class ReflectionHelper
{
    /// <summary>
    /// Get instances of all instantiable objects of a specified type.
    /// </summary>
    /// <typeparam name="TType">Base type.</typeparam>
    /// <returns>An instance of each instanciable type that inherit from the given type.</returns>
    public static IEnumerable<TType> GetInstantiableObjectsOf<TType>()
        => GetInstantiableTypesOf<TType>()
            .Select(type => (TType)Activator.CreateInstance(type)!)
            .Where(p => p is not null);

    /// <summary>
    /// Get instances of all instantiable objects of a specified type.
    /// </summary>
    /// <typeparam name="TType">Base type.</typeparam>
    /// <param name="excludedTypes">List of excluded types.</param>
    /// <returns>An instance of each instanciable type that inherit from the given type.</returns>
    public static IEnumerable<TType> GetInstantiableObjectsOf<TType>(IEnumerable<Type> excludedTypes)
        => GetInstantiableTypesOf<TType>(excludedTypes)
            .Select(type => (TType)Activator.CreateInstance(type)!)
            .Where(p => p is not null);

    /// <summary>
    /// Get all types of a specified type except interfaces and abstract classes.
    /// </summary>
    /// <typeparam name="TType">Base type.</typeparam>
    /// <returns>All types that inherit from the given type.</returns>
    public static IEnumerable<Type> GetInstantiableTypesOf<TType>() => GetInstantiableTypesOf<TType>(null);

    /// <summary>
    /// Get all types of a specified type except interfaces and abstract classes.
    /// </summary>
    /// <typeparam name="TType">Base type.</typeparam>
    /// <param name="excludedTypes">List of excluded types.</param>
    /// <returns>All types that inherit from the given type.</returns>
    public static IEnumerable<Type> GetInstantiableTypesOf<TType>(IEnumerable<Type>? excludedTypes)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        List<Type> instantiableTypes = [];
        List<Type>? excluded = excludedTypes?.ToList();
        foreach (Assembly assembly in assemblies)
        {
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                Debug.WriteLine($"Could not load types for assembly: {assembly.FullName}\nError: {e.FullMessage()}");
                continue;
            }

            foreach (Type type in types)
            {
                if (IsValidType<TType>(type, excluded))
                {
                    instantiableTypes.Add(type);
                }
            }
        }

        return instantiableTypes;
    }

    private static bool IsValidType<TType>(Type type, IEnumerable<Type>? excludedTypes)
        => IsValidType<TType>(type) &&
        (excludedTypes is null || !excludedTypes.Any(p => p.IsAssignableFrom(type)));

    private static bool IsValidType<TType>(Type type)
        => type is { IsAbstract: false, IsInterface: false } && typeof(TType).IsAssignableFrom(type);
}