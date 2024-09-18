// <copyright file="PolymorphicSerializationResolverBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.PolymorphicSerialization;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Resolves the polymorphic serialization for JSON.
/// </summary>
public class PolymorphicSerializationResolverBuilder : DefaultJsonTypeInfoResolver
{
    private readonly List<IPolymorphicSerializationMapper> _serializationMappers = [];

    /// <summary>
    /// Adds a serialization mapper to the builder.
    /// </summary>
    /// <param name="serializationMapper">The serialization mapper to add.</param>
    /// <returns>The current instance of the builder.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the serialization mapper is null.</exception>
    public PolymorphicSerializationResolverBuilder AddMapper(IPolymorphicSerializationMapper serializationMapper)
    {
        if (serializationMapper == null)
        {
            throw new ArgumentNullException(nameof(serializationMapper));
        }

        _serializationMappers.Add(serializationMapper);
        return this;
    }

    /// <summary>
    /// Builds the PolymorphicSerializationResolver instance.
    /// </summary>
    /// <returns>A new instance of PolymorphicSerializationResolver.</returns>
    public IEnumerable<IPolymorphicSerializationMapper> Build() => _serializationMappers;
}