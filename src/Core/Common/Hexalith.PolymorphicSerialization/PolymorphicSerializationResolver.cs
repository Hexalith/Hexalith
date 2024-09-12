// <copyright file="PolymorphicSerializationResolver.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.PolymorphicSerialization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Resolves the polymorphic serialization for JSON.
/// </summary>
public class PolymorphicSerializationResolver : DefaultJsonTypeInfoResolver
{
    private readonly Dictionary<Type, IEnumerable<IPolymorphicSerializationMapper>> _serializationMappers;

    /// <summary>
    /// Initializes a new instance of the <see cref="PolymorphicSerializationResolver"/> class.
    /// </summary>
    /// <param name="serializationMappers">The collection of serialization mappers.</param>
    public PolymorphicSerializationResolver(IEnumerable<IPolymorphicSerializationMapper> serializationMappers)
    {
        if (serializationMappers == null)
        {
            throw new ArgumentNullException(nameof(serializationMappers));
        }

        _serializationMappers = serializationMappers
            .GroupBy(m => m.Base)
            .ToDictionary(g => g.Key, g => g.AsEnumerable());
    }

    /// <inheritdoc/>
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        if (_serializationMappers.TryGetValue(jsonTypeInfo.Type, out IEnumerable<IPolymorphicSerializationMapper>? mappers))
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
            };
            foreach (IPolymorphicSerializationMapper mapper in mappers)
            {
                jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(mapper.JsonDerivedType);
            }
        }

        return jsonTypeInfo;
    }
}