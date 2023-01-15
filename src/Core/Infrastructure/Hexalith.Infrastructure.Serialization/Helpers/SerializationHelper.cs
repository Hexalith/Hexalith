// <copyright file="SerializationHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Serialization.Helpers;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Class SerializationHelper.
/// </summary>
public static class SerializationHelper
{
    /// <summary>
    /// Converts to polymorphicjson.
    /// </summary>
    /// <typeparam name="T">The polymorphic type to serialize. The type or one of it's ascendents must have the polimorphic base class attribute <see cref="JsonPolymorphicAttribute"/>.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>The Json string.</returns>
    public static string ToPolymorphicJson<T>(this T value)
    {
        return JsonSerializer.Serialize(value, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            TypeInfoResolver = new PolymorphicTypeResolver(),
        });
    }
}