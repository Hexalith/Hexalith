namespace Hexalith.UnitTests.Core.Infrastructure.Serialization;

using FluentAssertions;

using Hexalith.Infrastructure.Serialization.Serialization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class IsoUtcDateTimeOffsetConverterTest
{
    [Fact]
    public void Deserialize_date_should_succeed()
    {
        var converter = new IsoUtcDateTimeOffsetConverter();
        var json = "\"2022-05-06T13:05:22Z\"";
        var options = new JsonSerializerOptions();
        options.Converters.Add(converter);
        var date = JsonSerializer.Deserialize<DateTimeOffset>(
            json,
            options);
        date.Should().Be(new DateTimeOffset(
            2022,
            5,
            6,
            13,
            5,
            22,
            TimeSpan.Zero));
    }

    [Fact]
    public void Serialize_date_should_return_iso_8106_string()
    {
        var converter = new IsoUtcDateTimeOffsetConverter();
        var date = DateTimeOffset.Now;
        var options = new JsonSerializerOptions();
        options.Converters.Add(converter);
        var json = JsonSerializer.Serialize(
            date,
            options);
        var d = date.ToUniversalTime();
        json.Should().Be($"\"{d.Year:D4}-{d.Month:D2}-{d.Day:D2}T{d.Hour:D2}:{d.Minute:D2}:{d.Second:D2}Z\"");
    }
}