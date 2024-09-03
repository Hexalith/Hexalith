// ***********************************************************************
// Assembly         : Hexalith.Extensions
// Author           : Jérôme Piquot
// Created          : 05-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-22-2023
// ***********************************************************************
// <copyright file="ExampleHelper.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Extensions.Helpers;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

using Hexalith.Extensions.Common;

/// <summary>
/// Class Example.
/// </summary>
public static class ExampleHelper
{
    /// <summary>
    /// Creates the specified type.
    /// </summary>
    /// <typeparam name="T">class type.</typeparam>
    /// <returns>object?.</returns>
    public static T CreateExample<T>()
        where T : class, new()
        => (T)typeof(T).CreateExample();

    /// <summary>
    /// Creates the example.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>object.</returns>
    /// <exception cref="InvalidOperationException">$"Could not set the value of property {property.Name} of type {type.FullName}., ex.</exception>
    public static object CreateExample([NotNull] this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        object instance = Activator.CreateInstance(type)
            ?? throw new InvalidOperationException($"Could not create an instance of type {type.FullName}. The class must have a default constructor.");
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
        foreach (PropertyInfo property in properties.Where(p => p.CanWrite))
        {
            try
            {
                object? value;
                ExampleValueAttribute? attribute = property.GetCustomAttribute<ExampleValueAttribute>();
                value = attribute is not null
                    ? attribute.Value
                    : property.PropertyType switch
                    {
                        _ when property.PropertyType == typeof(string) => "string",
                        _ when property.PropertyType == typeof(int) => 101,
                        _ when property.PropertyType == typeof(long) => 101L,
                        _ when property.PropertyType == typeof(double) => 101.25,
                        _ when property.PropertyType == typeof(decimal) => 101.20M,
                        _ when property.PropertyType == typeof(DateTime) => DateTime.UtcNow,
                        _ when property.PropertyType == typeof(DateTimeOffset) => DateTimeOffset.UtcNow,
                        _ when property.PropertyType == typeof(bool) => true,
                        _ when property.PropertyType == typeof(Guid) => Guid.NewGuid(),
                        _ when property.PropertyType == typeof(byte[]) => Encoding.UTF8.GetBytes("string"),
                        _ when property.PropertyType.IsEnum => Enum.GetValues(property.PropertyType).GetValue(0),
                        _ when property.PropertyType.IsClass => CreateExample(property.PropertyType),
                        _ => null,
                    };
                property.SetValue(instance, value);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not set the value of property {property.Name} of type {type.FullName}.", ex);
            }
        }

        return instance;
    }
}