namespace Hexalith.Infrastructure.Abstractions.Tests
{
    using System;
    using System.Text.Json;

    using Hexalith.Infrastructure.Abstractions.Tests.Fixtures;
    using Hexalith.Infrastructure.Helpers;

    using FluentAssertions;

    using Microsoft.AspNetCore.WebUtilities;

    using Xunit;

    public class HtmlHelperTest
    {
        [Theory]
        [InlineData("L l")]
        [InlineData("/L l/")]
        [InlineData("\\/L l\\/")]
        [InlineData("àaA")]
        [InlineData("Eéèe")]
        [InlineData("M@fd1523çççç\\\\èé/+ {}")]
        [InlineData("+-*/=")]
        public void HtmlEncode_and_decode_should_be_the_same(string value)
        {
            value
                .UrlEncode()
                .UrlDecode()
                .Should()
                .Be(value);
        }

        [Theory]
        [InlineData("/")]
        [InlineData(">")]
        [InlineData("<")]
        [InlineData("\"")]
        [InlineData("\'")]
        [InlineData("&")]
        public void HtmlEncode_special_character_should_not_be_the_same(string character)
        {
            character.UrlEncode()
                .Should()
                .NotBe(character);
        }

        [Fact]
        public void ToHttpQueryString_should_contain_all_properties()
        {
            var obj = new DummyObjectWithSubObject();
            obj.SetValues1();
            obj.ASubObject.SetValues2();
            var query = obj.ToHttpQueryString();
            query.Should().Contain(nameof(obj.ADateTime));
            query.Should().Contain(nameof(obj.ADateTimeOffset));
            query.Should().Contain(nameof(obj.ADecimal));
            query.Should().Contain(nameof(obj.ADouble));
            query.Should().Contain(nameof(obj.AnInteger));
            query.Should().Contain(nameof(obj.AString));
            query.Should().Contain(nameof(obj.AStringArray));
            query.Should().Contain(nameof(obj.ASubObject));
            var parameters = QueryHelpers.ParseQuery(query);
            parameters.Should().ContainKey(nameof(obj.ADateTime));
            parameters.Should().ContainKey(nameof(obj.ADateTimeOffset));
            parameters.Should().ContainKey(nameof(obj.ADecimal));
            parameters.Should().ContainKey(nameof(obj.ADouble));
            parameters.Should().ContainKey(nameof(obj.AnInteger));
            parameters.Should().ContainKey(nameof(obj.AString));
            parameters.Should().ContainKey(nameof(obj.AStringArray));
            parameters.Should().ContainKey(nameof(obj.ASubObject));
            JsonSerializer.Deserialize<DateTime>(parameters[nameof(obj.ADateTime)]).Should().Be(obj.ADateTime);
            JsonSerializer.Deserialize<DateTimeOffset>(parameters[nameof(obj.ADateTimeOffset)]).Should().Be(obj.ADateTimeOffset);
            JsonSerializer.Deserialize<decimal>(parameters[nameof(obj.ADecimal)]).Should().Be(obj.ADecimal);
            JsonSerializer.Deserialize<double>(parameters[nameof(obj.ADouble)]).Should().Be(obj.ADouble);
            JsonSerializer.Deserialize<int>(parameters[nameof(obj.AnInteger)]).Should().Be(obj.AnInteger);
            JsonSerializer.Deserialize<string>(parameters[nameof(obj.AString)]).Should().Be(obj.AString);
            JsonSerializer.Deserialize<string[]>(parameters[nameof(obj.AStringArray)]).Should().BeEquivalentTo(obj.AStringArray);
            var subObject = JsonSerializer.Deserialize<DummyObject>(parameters[nameof(obj.ASubObject)]);
            Assert.NotNull(subObject);
            subObject?.Should().BeEquivalentTo(obj.ASubObject);
        }
    }
}