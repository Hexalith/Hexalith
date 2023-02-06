// <copyright file="PolymorphicTypeResolver.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Serialization;

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using Hexalith.Extensions.Serialization;

/// <summary>
/// Default type resolver for <see cref="JsonSerializer"/>. Inherits from base class <see cref="DefaultJsonTypeInfoResolver"/>.
/// </summary>
/// <typeparam name="T">Type of the base class or interface.</typeparam>
public sealed class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
{
    /// <inheritdoc/>
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object)
        {
            TypeNamePolymorphismOptions? poly = TypeNamePolymorphismOptions.Create(jsonTypeInfo.Type);
            if (poly != null)
            {
                JsonPolymorphicBaseClassAttribute? attribute = type.GetCustomAttribute<JsonPolymorphicBaseClassAttribute>();
                if (!string.IsNullOrWhiteSpace(attribute?.DiscriminatorName))
                {
                    poly.TypeDiscriminatorPropertyName = attribute.DiscriminatorName;
                }

                jsonTypeInfo.PolymorphismOptions = poly;
            }
        }

        return jsonTypeInfo;
    }
}