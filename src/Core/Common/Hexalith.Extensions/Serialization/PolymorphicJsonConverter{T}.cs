// <copyright file="PolymorphicJsonConverter{T}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Serialization;

using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using Hexalith.Extensions.Reflections;

/// <summary>
/// Class objectConverterInner.
/// Implements the <see cref="JsonConverter{T}" />.
/// </summary>
/// <typeparam name="T">Message type.</typeparam>
/// <seealso cref="JsonConverter{T}" />
public class PolymorphicJsonConverter<T> : JsonConverter<T>
    where T : class, IPolymorphicSerializable, new()
{
    /// <summary>
    /// Reads and converts the JSON to type <typeparamref name="T" />.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    /// <exception cref="NotSupportedException">Type name is empty or missing.</exception>
    /// <exception cref="NotSupportedException">Deserialized object is null.</exception>
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Load the JSON into a JsonDocument to enable querying the "type" property
        using JsonDocument jsonDoc = JsonDocument.ParseValue(ref reader);

        // Get the "$type" property value and create a new instance of the appropriate type
        bool hasTypeName = jsonDoc
            .RootElement
            .TryGetProperty(IPolymorphicSerializable.TypeNamePropertyName, out JsonElement typeNameElement);
        string? typeName = hasTypeName ? typeNameElement.GetString() : null;
        if (string.IsNullOrWhiteSpace(typeName))
        {
            throw new InvalidOperationException($"The type name property '{IPolymorphicSerializable.TypeNamePropertyName}' is empty or missing. JSON:\n{jsonDoc.RootElement.GetRawText()}");
        }

        bool hasMajorVersion = jsonDoc
            .RootElement
            .TryGetProperty(IPolymorphicSerializable.MajorVersionPropertyName, out JsonElement majorVersionElement);
        int majorVersion = hasMajorVersion ? majorVersionElement.GetInt32() : 0;
        bool hasMinorVersion = jsonDoc
            .RootElement
            .TryGetProperty(IPolymorphicSerializable.MinorVersionPropertyName, out JsonElement minorVersionElement);
        int minorVersion = hasMinorVersion ? minorVersionElement.GetInt32() : 0;

        // Deserialize the remaining properties
        return JsonSerializer
            .Deserialize(
                jsonDoc.RootElement.GetRawText(),
                TypeMapper.GetType<IPolymorphicSerializable>(IPolymorphicSerializable.GetTypeMapName(typeName, majorVersion, minorVersion)),
                options) as T
                ?? throw new NotSupportedException($"Deserialized object is null:\n" + jsonDoc.RootElement.GetRawText());
    }

    /// <summary>
    /// Writes a specified value as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <exception cref="NotSupportedException">Cannot create JSON object.</exception>
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value != null && value.GetType() == typeof(T))
        {
            throw new InvalidOperationException("The serialized value type can't be the same as the polymorphic base class : " + value.GetType().Name);
        }

        JsonDocument jsonDoc = JsonDocument.Parse(JsonSerializer.Serialize<object?>(value, options));
        JsonObject json = JsonObject.Create(jsonDoc.RootElement)
            ?? throw new NotSupportedException($"Cannot create JSON object from :\n" + jsonDoc.RootElement.GetRawText());
        if (value != null)
        {
            json.Add(IPolymorphicSerializable.TypeNamePropertyName, value.TypeName);
            json.Add(IPolymorphicSerializable.MajorVersionPropertyName, value.MajorVersion);
            json.Add(IPolymorphicSerializable.MinorVersionPropertyName, value.MinorVersion);
        }

        json.WriteTo(writer, options);
    }
}