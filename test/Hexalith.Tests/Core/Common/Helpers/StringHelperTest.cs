// <copyright file="StringHelperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Tests.Core.Common.Helpers;

using FluentAssertions;

using Hexalith.Extensions.Helpers;

public class StringHelperTest
{
    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(10000, "10000")]
    [InlineData(-1001, "-1001")]
    public void Integer_into_invariant_string_should_be_same(int value, string expected)
    {
        _ = value.ToInvariantString().Should().Be(expected);
    }

    [Theory]
    [InlineData(0d, "0")]
    [InlineData(1d, "1")]
    [InlineData(10000d, "10000")]
    [InlineData(-1001d, "-1001")]
    public void Interger_into_invariant_string_should_be_same(double value, string expected)
    {
        _ = value.ToInvariantString().Should().Be(expected);
    }

    [Theory]
    [InlineData(0L, "0")]
    [InlineData(1L, "1")]
    [InlineData(10000L, "10000")]
    [InlineData(-1001L, "-1001")]
    public void Long_into_invariant_string_should_be_same(long value, string expected)
    {
        _ = value.ToInvariantString().Should().Be(expected);
    }
}
