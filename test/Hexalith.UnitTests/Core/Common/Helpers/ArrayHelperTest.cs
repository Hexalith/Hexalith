// <copyright file="ArrayHelperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Helpers;

using FluentAssertions;

using Hexalith.Extensions.Helpers;

public class ArrayHelperTest
{
    [Fact]
    public void IntegerIntoArrayShouldContainValue()
    {
        List<string>[] array = new List<string> { "Hello once", "Hello again " }.IntoArray();
        _ = array.Should().ContainSingle();
        _ = array[0][0].Should().Be("Hello once");
        _ = array[0][1].Should().Be("Hello again ");
    }

    [Fact]
    public void ListIntoArrayShouldContainIntegerValue()
    {
        int[] array = 1254.IntoArray();
        _ = array.Should().ContainSingle();
        _ = array[0].Should().Be(1254);
    }

    [Fact]
    public void ObjectIntoArrayShouldContainValue()
    {
        var array = new { Hello = "Hello", Count = 100 }.IntoArray();
        _ = array.Should().ContainSingle();
        _ = array[0].Hello.Should().Be("Hello");
        _ = array[0].Count.Should().Be(100);
    }

    [Fact]
    public void StringIntoArrayShouldContainValue()
    {
        string[] array = "Hello".IntoArray();
        _ = array.Should().ContainSingle();
        _ = array[0].Should().Be("Hello");
    }
}