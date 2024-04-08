// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Serialization
// Author           : Jérôme Piquot
// Created          : 03-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-10-2023
// ***********************************************************************
// <copyright file="BusinessEventJsonConverter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Serialization;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;

/// <summary>
/// Class BaseMessageConverterInner.
/// Implements the <see cref="JsonConverter{T}" />.
/// </summary>
/// <typeparam name="T">Message type.</typeparam>
/// <seealso cref="JsonConverter{T}" />
public class BusinessEventJsonConverter : JsonConverter<Dynamics365BusinessEventBase>
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
    public override Dynamics365BusinessEventBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Load the JSON into a JsonDocument to enable querying the "type" property
        using JsonDocument jsonDoc = JsonDocument.ParseValue(ref reader);

        // Get the "$type" property value and create a new instance of the appropriate type
        bool hasTypeName = jsonDoc
            .RootElement
            .TryGetProperty(nameof(Dynamics365BusinessEventBase.BusinessEventId), out JsonElement typeNameElement);
        string? typeName = hasTypeName ? typeNameElement.GetString() : null;
        if (string.IsNullOrWhiteSpace(typeName))
        {
            throw new InvalidOperationException($"The business event type name property '{nameof(Dynamics365BusinessEventBase.BusinessEventId)}' is empty or missing. JSON:\n{jsonDoc.RootElement.GetRawText()}");
        }

        bool hasMajorVersion = jsonDoc
            .RootElement
            .TryGetProperty(nameof(Dynamics365BusinessEventBase.MajorVersion), out JsonElement majorVersionElement);
        int majorVersion = hasMajorVersion ? majorVersionElement.GetInt32() : 0;
        bool hasMinorVersion = jsonDoc
            .RootElement
            .TryGetProperty(nameof(Dynamics365BusinessEventBase.MinorVersion), out JsonElement minorVersionElement);
        int minorVersion = hasMinorVersion ? minorVersionElement.GetInt32() : 0;

        // Deserialize the remaining properties
        return JsonSerializer
            .Deserialize(
                jsonDoc.RootElement.GetRawText(),
                BusinessEventTypeMapper.GetType(typeName, majorVersion, minorVersion),
                options) as Dynamics365BusinessEventBase
                ?? throw new NotSupportedException($"Deserialized business event is null:\n" + jsonDoc.RootElement.GetRawText());
    }

    /// <summary>
    /// Writes a specified value as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <exception cref="NotSupportedException">Cannot create JSON object.</exception>
    public override void Write(Utf8JsonWriter writer, Dynamics365BusinessEventBase value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(writer);

        if (value.GetType() == typeof(Dynamics365BusinessEventBase))
        {
            throw new InvalidOperationException("The serialized value type can't be the same as the polymorphic base class : " + value.GetType().Name);
        }

        JsonSerializer.Serialize<object>(writer, value, options);
    }
}