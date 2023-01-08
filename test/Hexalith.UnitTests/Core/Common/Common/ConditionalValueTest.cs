// <copyright file="ConditionalValueTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Common;

using FluentAssertions;

using Hexalith.Extensions.Common;

using System;

using Xunit;

/// <summary>
/// Class ConditionalValueTests.
/// </summary>
public class ConditionalValueTests
{
    /// <summary>
    /// Defines the test method to check if an empty conditional value HasValue property is false.
    /// </summary>
    [Fact]
    public void Check_hasvalue_is_false_when_no_value()
    {
        // Arrange
        ConditionalValue<int> conditionalValue = new();

        // Act and Assert
        _ = conditionalValue.HasValue.Should().BeFalse();
    }

    /// <summary>
    /// Defines the test method to check that the HasValue property is true when a value is set.
    /// </summary>
    [Fact]
    public void Check_hasvalue_is_true_when_value()
    {
        // Arrange
        ConditionalValue<int> conditionalValue = new(10);

        // Act and Assert
        _ = conditionalValue.HasValue.Should().BeTrue();
    }

    /// <summary>
    /// Defines the test method to check getting the value on an empty object is an invalid operation.
    /// </summary>
    [Fact]
    public void Get_value_on_no_value_should_throw_invalid_operation_exception()
    {
        // Arrange
        ConditionalValue<int> value = new();

        // Act and Assert
        Action action = () => _ = value.Value;
        _ = action.Should().Throw<InvalidOperationException>();
    }

    /// <summary>
    /// Defines the test method to check if we retreive the correct value.
    /// </summary>
    [Fact]
    public void Get_value_should_return_the_correct_value()
    {
        // Arrange
        ConditionalValue<int> conditionalValue = new(5);

        // Assert
        _ = conditionalValue.Value.Should().Be(5);
    }
}
