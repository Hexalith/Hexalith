// <copyright file="StringHelperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Helpers;

using System.Globalization;

using FluentAssertions;

using Hexalith.Extensions.Helpers;

public class StringHelperTest
{
    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(10000, "10000")]
    [InlineData(-1001, "-1001")]
    public void DecimalIntoInvariantStringShouldBeExpected(decimal value, string expected) => _ = value.ToInvariantString().Should().Be(expected);

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10000)]
    [InlineData(-1001)]
    public void DecimalIntoStringAndBackToNumberShouldBeSame(decimal value) => _ = value.ToInvariantString().ToDecimal().Should().Be(value);

    [Theory]
    [InlineData(0d, "0")]
    [InlineData(1d, "1")]
    [InlineData(10000d, "10000")]
    [InlineData(-1001d, "-1001")]
    public void DoubleIntoInvariantStringShouldBeExpected(double value, string expected) => _ = value.ToInvariantString().Should().Be(expected);

    [Theory]
    [InlineData(0d)]
    [InlineData(1d)]
    [InlineData(10000d)]
    [InlineData(-1001d)]
    public void DoubleIntoStringAndBackToNumberShouldBeSame(double value) => _ = value.ToInvariantString().ToDouble().Should().Be(value);

    [Fact]
    public void FormatStringWithNamedPlaceholdersShouldReturnExpected()
    {
        _ = StringHelper
            .FormatWithNamedPlaceholders(CultureInfo.InvariantCulture, "Say {Hello} {Number} times", ["hello world", 11])
            .Should()
            .Be("Say hello world 11 times");
    }

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(10000, "10000")]
    [InlineData(-1001, "-1001")]
    public void IntegerIntoInvariantStringShouldBeExpected(int value, string expected) => _ = value.ToInvariantString().Should().Be(expected);

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10000)]
    [InlineData(-1001)]
    public void IntegerIntoStringAndBackToNumberShouldBeSame(int value) => _ = value.ToInvariantString().ToInteger().Should().Be(value);

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(10000, "10000")]
    [InlineData(-1001, "-1001")]
    public void InvariantStringIntoIntegerShouldBeExpected(int expected, string value) => _ = value.ToInteger().Should().Be(expected);

    [Theory]
    [InlineData(0L, "0")]
    [InlineData(1L, "1")]
    [InlineData(10000L, "10000")]
    [InlineData(-1001L, "-1001")]
    public void LongIntoInvariantStringShouldBeExpected(long value, string expected) => _ = value.ToInvariantString().Should().Be(expected);

    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    [InlineData(10000L)]
    [InlineData(-1001L)]
    public void LongIntoStringAndBackToNumberShouldBeSame(long value) => _ = value.ToInvariantString().ToLong().Should().Be(value);

    [Theory]
    [InlineData("{hello}", "{0}")]
    [InlineData("{4}", "{0}")]
    [InlineData("{{double}} {test}", "{{0}} {1}")]
    [InlineData("hello from {me} and {him}", "hello from {0} and {1}")]
    public void ReplaceNamedPlaceholdersShouldReturnExpectedStringWithIndices(string value, string expected) => _ = StringHelper.ReplacePlaceholderNamesByIndex(value).Should().Be(expected);
}