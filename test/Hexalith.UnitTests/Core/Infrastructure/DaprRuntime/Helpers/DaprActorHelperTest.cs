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
    [Fact]
    public void ToActorId_ShouldEscapeSpecialCharacters()
    {
        // Arrange
        string actorIdString = "123/4";

        // Act
        Dapr.Actors.ActorId result = actorIdString.ToActorId();

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.ToString().Should().Be("123%2F4");
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

    [Fact]
    public void ToUnescapeString_ShouldReturnOriginalString_WhenNoSpecialCharacters()
    {
        // Arrange
        string escapedString = "123";

        // Act
        string result = new ActorId(escapedString).ToUnescapeString();

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.Should().Be(escapedString);
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