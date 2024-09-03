// ***********************************************************************
// Assembly         : Hexalith.Extensions
// Author           : Jérôme Piquot
// Created          : 01-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-25-2023
// ***********************************************************************
// <copyright file="UnixEpochDateTimeOffsetConverter.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Serialization.Serialization;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

/// <summary>
/// Class UnixEpochDateTimeConverter. This class cannot be inherited.
/// Implements the <see cref="JsonConverter{DateTime}" />.
/// </summary>
/// <seealso cref="JsonConverter{DateTime}" />
public sealed partial class UnixEpochDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    /// <summary>
    /// The epoch.
    /// </summary>
    private static readonly DateTimeOffset _epoch = new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

    /// <summary>
    /// Reads and converts the JSON to type <typeparamref name="T" />.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">An object that specifies serialization options to use.</param>
    /// <returns>The converted value.</returns>
    /// <exception cref="JsonException">Could not parse epoch date.</exception>
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string formatted = reader.GetString()!;
        Match match = EpochRegex().Match(formatted);

        if (
               !match.Success
                || !long.TryParse(match.Groups[1].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out long unixTime)
                || !int.TryParse(match.Groups[3].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int hours)
                || !int.TryParse(match.Groups[4].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int minutes))
        {
            throw new JsonException("Could not parse epoch date");
        }

        int sign = match.Groups[2].Value[0] == '+' ? 1 : -1;
        TimeSpan utcOffset = new(hours * sign, minutes * sign, 0);

        return _epoch.AddMilliseconds(unixTime).ToOffset(utcOffset);
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
        long unixTime = Convert.ToInt64((value - _epoch).TotalMilliseconds);

        TimeSpan utcOffset = value.Offset;

        string formatted = FormattableString.Invariant($"/Date({unixTime}{(utcOffset >= TimeSpan.Zero ? "+" : "-")}{utcOffset:hhmm})/");
        writer.WriteStringValue(formatted);
    }

    [GeneratedRegex("^/Date\\(([+-]*\\d+)\\)/$", RegexOptions.CultureInvariant)]
    private static partial Regex EpochRegex();
}