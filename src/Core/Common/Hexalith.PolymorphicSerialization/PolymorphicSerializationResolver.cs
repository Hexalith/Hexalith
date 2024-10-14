// <copyright file="PolymorphicSerializationResolver.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerialization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Resolves the polymorphic serialization for JSON.
/// </summary>
public class PolymorphicSerializationResolver : DefaultJsonTypeInfoResolver
{
    private Dictionary<Type, IEnumerable<IPolymorphicSerializationMapper>>? _serializationMappers;

    /// <summary>
    /// Initializes a new instance of the <see cref="PolymorphicSerializationResolver"/> class.
    /// </summary>
    public PolymorphicSerializationResolver()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PolymorphicSerializationResolver"/> class.
    /// </summary>
    /// <param name="serializationMappers">The collection of serialization mappers.</param>
    [ActivatorUtilitiesConstructor]
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

    /// <summary>
    /// Gets or sets the default mappers.
    /// </summary>
    public static ICollection<IPolymorphicSerializationMapper> DefaultMappers { get; set; } = [];

    private Dictionary<Type, IEnumerable<IPolymorphicSerializationMapper>> SerializationMappers
                    => _serializationMappers ??= DefaultMappers
            .GroupBy(m => m.Base)
            .ToDictionary(g => g.Key, g => g.AsEnumerable());

    /// <inheritdoc/>
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        if (SerializationMappers.TryGetValue(jsonTypeInfo.Type, out IEnumerable<IPolymorphicSerializationMapper>? mappers))
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