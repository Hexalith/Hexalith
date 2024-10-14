// <copyright file="ObjectDescriptionHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Hexalith.Extensions.Reflections;

using Humanizer;

/// <summary>
/// Class ObjectDescriptionHelper.
/// </summary>
public static class ObjectDescriptionHelper
{
    /// <summary>
    /// Describes the type using attributes.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>System.ValueTuple&lt;System.String, System.String, System.String&gt;.</returns>
    public static (string TypeName, string DisplayName, string Description) Describe([NotNull] this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        string? typeName = null;
        string? name = null;
        string? description = null;

        // Get the TypeMapName if the type implements IMappableType.
        if (typeof(IMappableType).IsAssignableFrom(type) && Activator.CreateInstance(type) is IMappableType mappable)
        {
            typeName = mappable.TypeMapName;
        }

        // Get the Type name if the type does not have an MapTypeName.
        typeName ??= type.Name;

        // Get the type description from the display attribute.
        DisplayAttribute? displayAttribute = type.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute != null)
        {
            name = string.IsNullOrWhiteSpace(displayAttribute.Name) ? null : displayAttribute.Name;
            description = string.IsNullOrWhiteSpace(displayAttribute.Description) ? null : displayAttribute.Description;
        }

        if (name == null)
        {
            DisplayNameAttribute? displayNameAttribute = type.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null)
            {
                name = string.IsNullOrWhiteSpace(displayNameAttribute.DisplayName) ? null : displayNameAttribute.DisplayName;
            }
        }

        name ??= typeName.Humanize(); // If name attributes are not found use the type name converted to a sentence.

        if (description == null)
        {
            DescriptionAttribute? descriptionAttribute = type.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                description = string.IsNullOrWhiteSpace(descriptionAttribute.Description) ? null : descriptionAttribute.Description;
            }
        }

        description ??= name; // If description attributes are not found, use the name.

        return (typeName, name, description);
    }

    /// <summary>
    /// Describes the specified property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>System.ValueTuple&lt;System.String, System.String, System.Nullable&lt;System.Object&gt;, System.Boolean&gt;.</returns>
    public static (string DisplayName, string Description, object? DefaultValue, bool IsRequired) Describe([NotNull] this PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(property);
        string? name = null;
        string? description = null;
        object? defaultValue = property.GetCustomAttribute<DefaultValueAttribute>()?.Value;
        bool required = property.GetCustomAttribute<RequiredAttribute>() != null;

        // Get the type description from the display attribute.
        DisplayAttribute? displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute != null)
        {
            name = string.IsNullOrWhiteSpace(displayAttribute.Name) ? null : displayAttribute.Name;
            description = string.IsNullOrWhiteSpace(displayAttribute.Description) ? null : displayAttribute.Description;
        }

        if (name == null)
        {
            DisplayNameAttribute? displayNameAttribute = property.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null)
            {
                name = string.IsNullOrWhiteSpace(displayNameAttribute.DisplayName) ? null : displayNameAttribute.DisplayName;
            }
        }

        name ??= property.Name.Humanize(); // If name attributes are not found use the type name converted to a sentence.

        if (description == null)
        {
            DescriptionAttribute? descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                description = string.IsNullOrWhiteSpace(descriptionAttribute.Description) ? null : descriptionAttribute.Description;
            }
        }

        description ??= name; // If description attributes are not found, use the name.

        return (name, description, defaultValue, required);
    }

    /// <summary>
    /// Describes the instance write properties.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>IDictionary&lt;System.String, System.ValueTuple&lt;System.String, System.String, System.Nullable&lt;System.Object&gt;, System.Boolean&gt;&gt;.</returns>
    public static IDictionary<string, (string DisplayName, string Description, object? DefaultValue, bool IsRequired)> DescribeInstanceWriteProperties([NotNull] this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        Dictionary<string, (string DisplayName, string Description, object? DefaultValue, bool IsRequired)> result = [];
        foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite))
        {
            result.Add(property.Name, property.Describe());
        }

        return result;
    }

    /// <summary>
    /// Describes the type using attributes.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>System.ValueTuple&lt;System.String, System.String, System.String&gt;.</returns>
    public static IDictionary<string, (string DisplayName, string Description, object? DefaultValue, bool IsRequired)> DescribeProperties([NotNull] this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        Dictionary<string, (string DisplayName, string Description, object? DefaultValue, bool IsRequired)> result = [];
        foreach (PropertyInfo property in type.GetProperties())
        {
            result.Add(property.Name, property.Describe());
        }

        return result;
    }
}