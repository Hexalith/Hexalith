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
    [InlineData("123!4", "123%2104")]
    [InlineData("123 4", "123%2114")]
    [InlineData("123/4", "123%2124")]
    [InlineData("123+4", "123%2B4")]
    [InlineData("123@4", "123%404")]
    public void ToActorId_ShouldEscapeSpecialCharacters(string id, string escapedString)
    {
        // Act
        Dapr.Actors.ActorId result = id.ToActorId();

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.ToString().Should().Be(escapedString);
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

    [Theory]
    [InlineData("123")]
    [InlineData("123/4")]
    [InlineData("1 2 3")]
    [InlineData("1!2 3/4")]
    [InlineData("123@")]
    public void ToUnescapeString_ShouldWorkWithToActorId(string actorIdString)
    {
        // Arrange
        ActorId actorId = actorIdString.ToActorId();

        // Act
        string result = actorId.ToUnescapeString();

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.Should().Be(actorIdString);
    }

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
        result.Should().NotBeNull();
        result.ToString().Should().Be(escapedString);
    }

    [Theory]
    [InlineData("123!@#$%^&*()4", "123%21%40%23%24%25%5E%26%2A%28%294")]
    [InlineData("Hello World!", "Hello%20World%21")]
    [InlineData("user@example.com", "user%40example%2Ecom")]
    public void ToActorId_ShouldHandleComplexStrings(string id, string escapedString)
    {
        // Act
        ActorId result = id.ToActorId();

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().Be(escapedString);
    }

    [Fact]
    public void ToActorId_ShouldHandleMaximumLength()
    {
        // Arrange
        string id = new string('a', 1024); // Testing with a long string

        // Act
        ActorId result = id.ToActorId();

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().Be(id);
    }

    [Theory]
    [InlineData(" leading")]
    [InlineData("trailing ")]
    [InlineData(" both ")]
    public void ToActorId_ShouldHandleWhitespace(string id)
    {
        // Act
        ActorId result = id.ToActorId();

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().Be(id.Replace(" ", "%20"));
    }
}