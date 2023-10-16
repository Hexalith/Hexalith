// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="ExampleTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.UnitTests.Core.Common.Helpers;

using System;

using FluentAssertions;

using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

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
        _ = example.Value.Should().Be("Hello");
    }

    /// <summary>
    /// Defines the test method BaseReadOnlyPropertyAndPropertyWithAttributeShouldHaveValue.
    /// </summary>
    [Fact]
    public void BaseReadOnlyPropertyAndPropertyWithAttributeShouldHaveValue()
    {
        BaseReadOnlyPropertyExample example = ExampleHelper.CreateExample<BaseReadOnlyPropertyExample>();
        _ = BaseReadOnlyProperty.ReadOnlyValue.Should().Be("Read");
        _ = example.Value.Should().Be("Hello");
    }

    /// <summary>
    /// Defines the test method ExampleCreatedIsValid.
    /// </summary>
    [Fact]
    public void ExampleCreatedIsValid()
    {
        TestExample example = ExampleHelper.CreateExample<TestExample>();
        _ = example.StringValue.Should().Be("Hello");
        _ = example.StringDefault.Should().Be("string");
        _ = example.IntValue.Should().Be(10);
    }

    /// <summary>
    /// Defines the test method ExampleCreationShouldNotThrowExceptions.
    /// </summary>
    [Fact]
    public void ExampleCreationShouldNotThrowExceptions()
    {
        Action action = () => ExampleHelper.CreateExample<TestExample>();
        _ = action.Should().NotThrow();
    }

    /// <summary>
    /// Defines the test method IntegerWithAttributeShouldHaveValue.
    /// </summary>
    [Fact]
    public void IntegerWithAttributeShouldHaveValue()
    {
        IntegerExample example = ExampleHelper.CreateExample<IntegerExample>();
        _ = example.Value.Should().Be(129);
    }

    /// <summary>
    /// Defines the test method IntegerWithNoAttributeShouldHaveValue.
    /// </summary>
    [Fact]
    public void IntegerWithNoAttributeShouldHaveValue()
    {
        IntegerDefaultExample example = ExampleHelper.CreateExample<IntegerDefaultExample>();
        _ = example.Value.Should().Be(101);
    }

    /// <summary>
    /// Defines the test method StringWithAttributeShouldHaveValue.
    /// </summary>
    [Fact]
    public void StringWithAttributeShouldHaveValue()
    {
        StringExample example = ExampleHelper.CreateExample<StringExample>();
        _ = example.Value.Should().Be("Hello");
    }

    /// <summary>
    /// Defines the test method StringWithNoAttributeShouldHaveValue.
    /// </summary>
    [Fact]
    public void StringWithNoAttributeShouldHaveValue()
    {
        StringDefaultExample example = ExampleHelper.CreateExample<StringDefaultExample>();
        _ = example.Value.Should().Be("string");
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

    /// <summary>
    /// Class BasePropertyExample.
    /// Implements the <see cref="Hexalith.UnitTests.Core.Common.Helpers.BaseProperty" />.
    /// </summary>
    /// <seealso cref="Hexalith.UnitTests.Core.Common.Helpers.BaseProperty" />
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