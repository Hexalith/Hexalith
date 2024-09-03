// <copyright file="IJsonPolymorphicTypeRegistry.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Serialization;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Provides a custom type registry for handling polymorphic types during JSON serialization and deserialization.
/// </summary>
public interface IJsonPolymorphicTypeRegistry
{
    /// <summary>
    /// Adds the derived types to the given list.
    /// </summary>
    /// <param name="polymorphicBaseType">The polymorphic base type.</param>
    /// <param name="derivedTypesList">The list of derived types to add to.</param>
    void AddDerivedTypesToList(Type polymorphicBaseType, IList<JsonDerivedType> derivedTypesList);

    /// <summary>
    /// Adds the derived types to the given list.
    /// </summary>
    /// <typeparam name="TPolymorphicBase">The polymorphic base type.</typeparam>
    /// <param name="derivedTypesList">The list of derived types to add to.</param>
    void AddDerivedTypesToList<TPolymorphicBase>(IList<JsonDerivedType> derivedTypesList)
        => AddDerivedTypesToList(typeof(TPolymorphicBase), derivedTypesList);

    /// <summary>
    /// Get the derived types registered for polymorphic type resolution.
    /// </summary>
    /// <param name="polymorphicBaseType">The polymorphic base type.</param>
    /// <returns>All the the derived types of the polymorphic base type.</returns>
    IEnumerable<JsonDerivedType> GetDerivedTypes(Type polymorphicBaseType);

    /// <summary>
    /// Get the derived types registered for polymorphic type resolution.
    /// </summary>
    /// <typeparam name="TPolymorphicBase">The polymorphic base type.</typeparam>
    /// <returns>All the the derived types of the polymorphic base type.</returns>
    IEnumerable<JsonDerivedType> GetDerivedTypes<TPolymorphicBase>()
        => GetDerivedTypes(typeof(TPolymorphicBase));
}