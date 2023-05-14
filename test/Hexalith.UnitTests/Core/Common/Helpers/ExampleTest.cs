// <copyright file="ExampleTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Helpers;

using System;

using FluentAssertions;

using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

public class ExampleTest
{
    [Fact]
    public void Example_created_is_valid()
    {
        TestExample example = Example.Create<TestExample>();
        _ = example.StringValue.Should().Be("Hello");
        _ = example.StringDefault.Should().Be("string");
        _ = example.IntValue.Should().Be(10);
    }

    [Fact]
    public void Example_creation_should_not_throw_exceptions()
    {
        Action action = () => Example.Create<TestExample>();
        _ = action.Should().NotThrow();
    }

    [Fact]
    public void Integer_with_attribute_should_have_value()
    {
        IntegerExample example = Example.Create<IntegerExample>();
        _ = example.Value.Should().Be(129);
    }

    [Fact]
    public void Integer_with_no_attribute_should_have_value()
    {
        IntegerDefaultExample example = Example.Create<IntegerDefaultExample>();
        _ = example.Value.Should().Be(101);
    }

    [Fact]
    public void String_with_attribute_should_have_value()
    {
        StringExample example = Example.Create<StringExample>();
        _ = example.Value.Should().Be("Hello");
    }

    [Fact]
    public void String_with_no_attribute_should_have_value()
    {
        StringDefaultExample example = Example.Create<StringDefaultExample>();
        _ = example.Value.Should().Be("string");
    }

    public class IntegerDefaultExample
    {
        public int Value { get; set; }
    }

    public class IntegerExample
    {
        [ExampleValue(129)]
        public int Value { get; set; }
    }

    public class StringDefaultExample
    {
        public string Value { get; set; }
    }

    public class StringExample
    {
        [ExampleValue("Hello")]
        public string Value { get; set; }
    }

    public class TestExample
    {
        [ExampleValue(10)]
        public int IntValue { get; set; }

        public string StringDefault { get; set; }

        [ExampleValue("Hello")]
        public string StringValue { get; set; }
    }
}