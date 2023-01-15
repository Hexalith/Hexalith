// <copyright file="StringHelperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Helpers;

using FluentAssertions;

using Hexalith.Extensions.Helpers;

public class StringHelperTest
{
    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(10000, "10000")]
    [InlineData(-1001, "-1001")]
    public void Decimal_into_invariant_string_should_be_expected(decimal value, string expected)
    {
        _ = value.ToInvariantString().Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10000)]
    [InlineData(-1001)]
    public void Decimal_into_string_and_back_to_number_should_be_same(decimal value)
    {
        _ = value.ToInvariantString().ToDecimal().Should().Be(value);
    }

    [Theory]
    [InlineData(0d, "0")]
    [InlineData(1d, "1")]
    [InlineData(10000d, "10000")]
    [InlineData(-1001d, "-1001")]
    public void Double_into_invariant_string_should_be_expected(double value, string expected)
    {
        _ = value.ToInvariantString().Should().Be(expected);
    }

    [Theory]
    [InlineData(0d)]
    [InlineData(1d)]
    [InlineData(10000d)]
    [InlineData(-1001d)]
    public void Double_into_string_and_back_to_number_should_be_same(double value)
    {
        _ = value.ToInvariantString().ToDouble().Should().Be(value);
    }

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(10000, "10000")]
    [InlineData(-1001, "-1001")]
    public void Integer_into_invariant_string_should_be_expected(int value, string expected)
    {
        _ = value.ToInvariantString().Should().Be(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10000)]
    [InlineData(-1001)]
    public void Integer_into_string_and_back_to_number_should_be_same(int value)
    {
        _ = value.ToInvariantString().ToInteger().Should().Be(value);
    }

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(10000, "10000")]
    [InlineData(-1001, "-1001")]
    public void Invariant_string_into_integer_should_be_expected(int expected, string value)
    {
        _ = value.ToInteger().Should().Be(expected);
    }

    [Theory]
    [InlineData(0L, "0")]
    [InlineData(1L, "1")]
    [InlineData(10000L, "10000")]
    [InlineData(-1001L, "-1001")]
    public void Long_into_invariant_string_should_be_expected(long value, string expected)
    {
        _ = value.ToInvariantString().Should().Be(expected);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    [InlineData(10000L)]
    [InlineData(-1001L)]
    public void Long_into_string_and_back_to_number_should_be_same(long value)
    {
        _ = value.ToInvariantString().ToLong().Should().Be(value);
    }
}