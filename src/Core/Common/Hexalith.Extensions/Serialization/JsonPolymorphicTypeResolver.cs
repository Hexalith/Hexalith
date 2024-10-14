// <copyright file="JsonPolymorphicTypeResolver.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Serialization;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Provides a custom type resolver for handling polymorphic types during JSON serialization and deserialization.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="JsonPolymorphicTypeResolver"/> class.
/// </remarks>
public class JsonPolymorphicTypeResolver(IJsonPolymorphicTypeRegistry registry) : DefaultJsonTypeInfoResolver
{
    /// <inheritdoc/>
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(type);
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);
        Type? polymorphicBaseType = JsonPolymorphicTypeRegistry.GetPolymorphicBaseType(type);
        if (polymorphicBaseType is not null)
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
            };
            registry.AddDerivedTypesToList(polymorphicBaseType, jsonTypeInfo.PolymorphismOptions.DerivedTypes);
        }

        return jsonTypeInfo;
    }
}