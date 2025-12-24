// <copyright file="ExampleTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Helpers;

using System;

using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

using Shouldly;

/// <summary>
/// Class ExampleTest.
/// </summary>
public class ExampleTest
{
    // Defines the test method BasePropertyWithAttributeShouldHaveValue.
    // </summary>
    [Fact]
    public void BasePropertyWithAttributeShouldHaveValue()
    {
        BasePropertyExample example = ExampleHelper.CreateExample<BasePropertyExample>();
        example.Value.ShouldBe("Hello");
    }

    /// <summary>
    /// Defines the test method BaseReadOnlyPropertyAndPropertyWithAttributeShouldHaveValue.
    /// </summary>
    [Fact]
    public void BaseReadOnlyPropertyAndPropertyWithAttributeShouldHaveValue()
    {
        BaseReadOnlyPropertyExample example = ExampleHelper.CreateExample<BaseReadOnlyPropertyExample>();
        BaseReadOnlyProperty.ReadOnlyValue.ShouldBe("Read");
        example.Value.ShouldBe("Hello");
    }

    /// <summary>
    /// Defines the test method ExampleCreatedIsValid.
    /// </summary>
    [Fact]
    public void ExampleCreatedIsValid()
    {
        TestExample example = ExampleHelper.CreateExample<TestExample>();
        example.StringValue.ShouldBe("Hello");
        example.StringDefault.ShouldBe("string");
        example.IntValue.ShouldBe(10);
    }

    /// <summary>
    /// Defines the test method ExampleCreationShouldNotThrowExceptions.
    /// </summary>
    [Fact]
    public void ExampleCreationShouldNotThrowExceptions()
    {
        Action action = () => ExampleHelper.CreateExample<TestExample>();
        Should.NotThrow(action);
    }

    /// <summary>
    /// Defines the test method IntegerWithAttributeShouldHaveValue.
    /// </summary>
    [Fact]
    public void IntegerWithAttributeShouldHaveValue()
    {
        IntegerExample example = ExampleHelper.CreateExample<IntegerExample>();
        example.Value.ShouldBe(129);
    }

    /// <summary>
    /// Defines the test method IntegerWithNoAttributeShouldHaveValue.
    /// </summary>
    [Fact]
    public void IntegerWithNoAttributeShouldHaveValue()
    {
        IntegerDefaultExample example = ExampleHelper.CreateExample<IntegerDefaultExample>();
        example.Value.ShouldBe(101);
    }

    /// <summary>
    /// Defines the test method StringWithAttributeShouldHaveValue.
    /// </summary>
    [Fact]
    public void StringWithAttributeShouldHaveValue()
    {
        StringExample example = ExampleHelper.CreateExample<StringExample>();
        example.Value.ShouldBe("Hello");
    }

    /// <summary>
    /// Defines the test method StringWithNoAttributeShouldHaveValue.
    /// </summary>
    [Fact]
    public void StringWithNoAttributeShouldHaveValue()
    {
        StringDefaultExample example = ExampleHelper.CreateExample<StringDefaultExample>();
        example.Value.ShouldBe("string");
    }

    /// <summary>
    /// Class BaseReadOnlyProperty.
    /// </summary>
    private static class BaseReadOnlyProperty
    {
        /// <summary>
        /// The read only value.
        /// </summary>
        public const string ReadOnlyValue = "Read";
    }

    /// <summary>
    /// Class BaseProperty.
    /// </summary>
    private class BaseProperty
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [ExampleValue("Hello")]
        public string Value { get; set; }
    }

    private class BasePropertyExample : BaseProperty
    {
    }

    /// <summary>
    /// Class BaseReadOnlyPropertyExample.
    /// </summary>
    private class BaseReadOnlyPropertyExample
    {
        /// <summary>
        /// The read only value.
        /// </summary>
        public const string ReadOnlyValue = "Read";

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [ExampleValue("Hello")]
        public string Value { get; set; }
    }

    /// <summary>
    /// Class IntegerDefaultExample.
    /// </summary>
    private class IntegerDefaultExample
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value { get; set; }
    }

    /// <summary>
    /// Class IntegerExample.
    /// </summary>
    private class IntegerExample
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [ExampleValue(129)]
        public int Value { get; set; }
    }

    /// <summary>
    /// Class StringDefaultExample.
    /// </summary>
    private class StringDefaultExample
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }

    /// <summary>
    /// Class StringExample.
    /// </summary>
    private class StringExample
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [ExampleValue("Hello")]
        public string Value { get; set; }
    }

    /// <summary>
    /// Class TestExample.
    /// </summary>
    private class TestExample
    {
        /// <summary>
        /// Gets or sets the int value.
        /// </summary>
        /// <value>The int value.</value>
        [ExampleValue(10)]
        public int IntValue { get; set; }

        /// <summary>
        /// Gets or sets the string default.
        /// </summary>
        /// <value>The string default.</value>
        public string StringDefault { get; set; }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        /// <value>The string value.</value>
        [ExampleValue("Hello")]
        public string StringValue { get; set; }
    }
}