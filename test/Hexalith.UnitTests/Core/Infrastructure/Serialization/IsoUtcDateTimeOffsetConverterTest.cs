// <copyright file="IsoUtcDateTimeOffsetConverterTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Serialization;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Infrastructure.Serialization.Serialization;

public class IsoUtcDateTimeOffsetConverterTest
{
    [Fact]
    public void DeserializeDateShouldSucceed()
    {
        IsoUtcDateTimeOffsetConverter converter = new();
        string json = "\"2022-05-06T13:05:22Z\"";
        JsonSerializerOptions options = new();
        options.Converters.Add(converter);
        DateTimeOffset date = JsonSerializer.Deserialize<DateTimeOffset>(
            json,
            options);
        _ = date.Should().Be(new DateTimeOffset(
            2022,
            5,
            6,
            13,
            5,
            22,
            TimeSpan.Zero));
    }

    [Fact]
    public void SerializeDateShouldReturnIso8106String()
    {
        IsoUtcDateTimeOffsetConverter converter = new();
        DateTimeOffset date = DateTimeOffset.Now;
        JsonSerializerOptions options = new();
        options.Converters.Add(converter);
        string json = JsonSerializer.Serialize(
            date,
            options);
        DateTimeOffset d = date.ToUniversalTime();
        _ = json.Should().Be($"\"{d.Year:D4}-{d.Month:D2}-{d.Day:D2}T{d.Hour:D2}:{d.Minute:D2}:{d.Second:D2}Z\"");
    }
}