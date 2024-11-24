// <copyright file="UnixEpochDateTimeConverter.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Serialization.Serialization;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

/// <summary>
/// Converts DateTime to and from Unix Epoch format in JSON.
/// </summary>
public sealed partial class UnixEpochDateTimeConverter : JsonConverter<DateTime>
{
    /// <summary>
    /// The epoch starting point (January 1, 1970, 00:00:00 UTC).
    /// </summary>
    private static readonly DateTime _epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Reads and converts the JSON to DateTime.
    /// </summary>
    /// <param name="reader">The reader to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>The converted DateTime value.</returns>
    /// <exception cref="System.Text.Json.JsonException">Thrown when the JSON cannot be parsed to a valid DateTime.</exception>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string formatted = reader.GetString()!;
        Match match = EpochRegex().Match(formatted);

        return !match.Success
                || !long.TryParse(match.Groups[1].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out long unixTime)
            ? throw new JsonException("Could not parse epoch date")
            : _epoch.AddMilliseconds(unixTime);
    }

    /// <summary>
    /// Writes a DateTime value as JSON.
    /// </summary>
    /// <param name="writer">The writer to write to.</param>
    /// <param name="value">The DateTime value to convert to JSON.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    public override void Write([NotNull] Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        long unixTime = Convert.ToInt64((value.ToUniversalTime() - _epoch).TotalMilliseconds);

        string formatted = FormattableString.Invariant($"/Date({unixTime})/");
        writer.WriteStringValue(formatted);
    }

    /// <summary>
    /// Gets the regular expression for matching Unix Epoch date strings.
    /// </summary>
    /// <returns>A Regex object for matching Unix Epoch date strings.</returns>
    [GeneratedRegex("^/Date\\(([+-]*\\d+)\\)/$", RegexOptions.CultureInvariant)]
    private static partial Regex EpochRegex();
}