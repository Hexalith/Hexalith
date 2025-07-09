// <copyright file="IsoUtcDateTimeOffsetConverter.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Serialization.Serialization;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Class UnixEpochDateTimeConverter. This class cannot be inherited.
/// Implements the <see cref="JsonConverter{DateTime}" />.
/// </summary>
/// <seealso cref="JsonConverter{DateTime}" />
public sealed class IsoUtcDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    /// <summary>
    /// Reads and converts the JSON to type />.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    /// <exception cref="JsonException">Could not parse epoch date.</exception>
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string formatted = reader.GetString()!;
        return DateTimeOffset.TryParse(
            formatted,
            CultureInfo.InvariantCulture,
            out DateTimeOffset result) ? result : throw new JsonException("Could not parse date : " + formatted);
    }

    /// <summary>
    /// Writes a specified value as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The value to convert to JSON.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    public override void Write([NotNull] Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        writer.WriteStringValue(
            value
            .ToUniversalTime()
            .ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture));
    }
}