// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Serialization
// Author           : Jérôme Piquot
// Created          : 03-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-10-2023
// ***********************************************************************
// <copyright file="BusinessEventTypeMapper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Serialization;

using System;
using System.Collections.Generic;
using System.Linq;

using Hexalith.Extensions.Common;
using Hexalith.Extensions.Errors;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;

/// <summary>
/// Class MessageSerializationMapper.
/// </summary>
public static class BusinessEventTypeMapper
{
    private static readonly object _syncMap = new();

    /// <summary>
    /// The map.
    /// </summary>
    private static volatile Dictionary<string, Type>? _map;

    /// <summary>
    /// Gets the map.
    /// </summary>
    /// <value>The map.</value>
    private static Dictionary<string, Type> Map
    {
        get
        {
            lock (_syncMap)
            {
                _map ??= GetMap();
            }

            return _map;
        }
    }

    /// <summary>
    /// Gets the map.
    /// </summary>
    /// <returns>Dictionary&lt;System.String, Type&gt;.</returns>
    /// <exception cref="System.TypeInitializationException">null.</exception>
    /// <exception cref="System.InvalidOperationException">Type {type.FullName} could not be added to the serialization mapper. A type with TypeName='{obj.TypeName}' and Version='{obj.MajorVersion}.{obj.MinorVersion}' already exists : {map[key].FullName}.</exception>
    public static Dictionary<string, Type> GetMap()
    {
        Dictionary<string, Type> map = [];
        System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic).ToArray();
        Type[] types = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Dynamics365BusinessEventBase)))
            .ToArray();
        foreach (Type type in types)
        {
            Dynamics365BusinessEventBase obj = Activator.CreateInstance(type) as Dynamics365BusinessEventBase ?? throw new TypeInitializationException(type.AssemblyQualifiedName, null);
            string key = $"{obj.BusinessEventId}|{obj.MajorVersion}.{obj.MinorVersion}";
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
    /// <exception cref="System.InvalidOperationException">Business event class with MessageName='{name}' and Version='{majorVersion}.{minorVersion}' not found.</exception>
    /// <exception cref="TypeInitializationException">Base message class with MessageName='{name}' and Version='{majorVersion}.{minorVersion}' not found.</exception>
    public static Type GetType(string name, int majorVersion, int minorVersion)
    {
        try
        {
            return Map[$"{name}|{majorVersion}.{minorVersion}"];
        }
        catch (KeyNotFoundException ex)
        {
            ApplicationError error = new()
            {
                Category = ErrorCategory.Technical,
                Arguments = [name, majorVersion, minorVersion],
                Detail = "The business event '{name}' v'{majorVersion}.{minorVersion}' not supported.",
                Title = "Business event not supported",
                Type = "BusinessEventNotSupported",
                TechnicalArguments = [name, majorVersion, minorVersion],
                TechnicalDetail = $"Business event class with MessageName='{name}' and Version='{majorVersion}.{minorVersion}' not found.",
            };
            throw new ApplicationErrorException(error, ex);
        }
    }
}