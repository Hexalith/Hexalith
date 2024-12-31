// <copyright file="DaprActorHelperTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Helpers;

using Dapr.Actors;

using FluentAssertions;

using Hexalith.Infrastructure.DaprRuntime.Helpers;

public class DaprActorHelperTest
{
    [Theory]
    [InlineData("123#4", "123%234")]
    [InlineData("123$4", "123%244")]
    [InlineData("123&4", "123%264")]
    [InlineData("123=4", "123%3D4")]
    [InlineData("123?4", "123%3F4")]
    public void ToActorId_ShouldEscapeAdditionalSpecialCharacters(string id, string escapedString)
    {
        // Act
        ActorId result = id.ToActorId();

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.ToString().Should().Be(escapedString);
    }

    [Theory]
    [InlineData("123!4", "123%2104")]
    [InlineData("123 4", "123%2114")]
    [InlineData("123/4", "123%2124")]
    [InlineData("123+4", "123%2B4")]
    [InlineData("123@4", "123%404")]
    public void ToActorId_ShouldEscapeSpecialCharacters(string id, string escapedString)
    {
        // Act
        ActorId result = id.ToActorId();

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.ToString().Should().Be(escapedString);
    }

    [Fact]
    public void ToActorId_ShouldHandleMaximumLength()
    {
        // Arrange
        string id = new('a', 1024); // Testing with a long string

        // Act
        ActorId result = id.ToActorId();

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.ToString().Should().Be(id);
    }

    [Fact]
    public void ToActorId_ShouldReturnActorId_WhenStringIsValid()
    {
        // Arrange
        string actorIdString = "123";

        // Act
        Dapr.Actors.ActorId result = actorIdString.ToActorId();

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.ToString().Should().Be(actorIdString);
    }

    [Fact]
    public void ToActorId_ShouldThrowArgumentException_WhenStringIsEmpty()
    {
        // Arrange
        string actorIdString = string.Empty;

        // Act
        Action act = () => actorIdString.ToActorId();

        // Assert
        _ = act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ToActorId_ShouldThrowArgumentNullException_WhenStringIsNull()
    {
        // Arrange
        string actorIdString = null;

        // Act
        Action act = () => actorIdString.ToActorId();

        // Assert
        _ = act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("Customer", "CustomerAggregate")]
    [InlineData("Order", "OrderAggregate")]
    [InlineData("Product", "ProductAggregate")]
    public void ToAggregateActorName_ShouldAppendAggregateSuffix(string aggregateName, string expected)
    {
        // Act
        string result = aggregateName.ToAggregateActorName();

        // Assert
        _ = result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Customer", "CustomerAggregate")]
    [InlineData("OrderAggregateExtra", "OrderAggregateExtraAggregate")]
    [InlineData("OrderAggregate", "OrderAggregateAggregate")]
    [InlineData("Aggregate", "AggregateAggregate")]
    [InlineData("AggregateOrder", "AggregateOrderAggregate")]
    public void ToAggregateActorName_ShouldHandleNonStandardNames(string aggregateName, string actorName)
    {
        // Arrange
        // Act
        string result = aggregateName.ToAggregateActorName();

        // Assert
        _ = result.Should().Be(actorName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ToAggregateActorName_ShouldThrowArgumentException_WhenNameIsEmptyOrWhitespace(string aggregateName)
    {
        // Act
        Action act = () => aggregateName.ToAggregateActorName();

        // Assert
        _ = act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ToAggregateActorName_ShouldThrowArgumentNullException_WhenNameIsNull()
    {
        // Arrange
        string aggregateName = null;

        // Act
        Action act = () => aggregateName.ToAggregateActorName();

        // Assert
        _ = act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("Customer", "CustomerAggregate")]
    [InlineData("OrderAggregateExtra", "OrderAggregateExtraAggregate")]
    [InlineData("OrderAggregate", "OrderAggregateAggregate")]
    [InlineData("Aggregate", "AggregateAggregate")]
    [InlineData("AggregateOrder", "AggregateOrderAggregate")]
    public void ToAggregateName_ShouldHandleNonStandardNames(string aggregateName, string actorName)
    {
        // Arrange
        // Act
        string result = DaprActorHelper.ToAggregateName(actorName);

        // Assert
        _ = result.Should().Be(aggregateName);
    }

    [Theory]
    [InlineData("123!4", "123%2104")]
    [InlineData("123 4", "123%2114")]
    [InlineData("123/4", "123%2124")]
    [InlineData("123+4", "123%2B4")]
    [InlineData("123@4", "123%404")]
    public void ToUnescapeString_ShouldReturnOriginalString_WhenNoSpecialCharacters(string id, string escapedString)
    {
        // Act
        string result = new ActorId(escapedString).ToUnescapeString();

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.Should().Be(id);
    }

    [Fact]
    public void ToUnescapeString_ShouldThrowArgumentException_WhenStringIsEmpty()
    {
        // Arrange
        string escapedString = string.Empty;

        // Act
        Action act = () => new ActorId(escapedString).ToUnescapeString();

        // Assert
        _ = act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ToUnescapeString_ShouldUnescapeSpecialCharacters()
    {
        // Arrange
        string escapedString = "123%2F4";

        // Act
        string result = new ActorId(escapedString).ToUnescapeString();

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.Should().Be("123/4");
    }
}