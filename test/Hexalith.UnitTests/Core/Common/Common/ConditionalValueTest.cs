// <copyright file="ConditionalValueTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Common;

using System;

using FluentAssertions;

using Hexalith.Extensions.Common;

using Xunit;

/// <summary>
/// Class ConditionalValueTests.
/// </summary>
public class ConditionalValueTest
{
    /// <summary>
    /// Defines the test method to check if an empty conditional value HasValue property is false.
    /// </summary>
    [Fact]
    public void CheckHasValueIsFalseWhenNoValue()
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
    public void CheckHasValueIsTrueWhenValue()
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
    public void GetValueOnNoValueShouldThrowInvalidOperationException()
    {
        // Arrange
        ConditionalValue<int> value = new();

        // Act and Assert
        Action action = () => _ = value.Value;
        _ = action.Should().Throw<InvalidOperationException>();
    }

    /// <summary>
    /// Defines the test method to check if we retrieve the correct value.
    /// </summary>
    [Fact]
    public void GetValueShouldReturnTheCorrectValue()
    {
        // Arrange
        ConditionalValue<int> conditionalValue = new(5);

        // Assert
        _ = conditionalValue.Value.Should().Be(5);
    }
}