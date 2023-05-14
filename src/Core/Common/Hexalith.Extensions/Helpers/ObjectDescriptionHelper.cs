// ***********************************************************************
// Assembly         : Hexalith.Extensions
// Author           : Jérôme Piquot
// Created          : 05-14-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-14-2023
// ***********************************************************************
// <copyright file="ObjectDescriptionHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Extensions.Helpers;

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    public static (string TypeName, string DisplayName, string Description) DescribeType(this Type type)
    {
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
}