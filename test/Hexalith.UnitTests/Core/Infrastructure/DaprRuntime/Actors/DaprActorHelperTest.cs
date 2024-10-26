// <copyright file="DaprActorHelperTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using Dapr.Actors;

using FluentAssertions;

using Hexalith.Infrastructure.DaprRuntime.Helpers;

using Xunit;

/// <summary>
/// Class that contains tests for <see cref="DaprActorHelper"/>.
/// </summary>
public class DaprActorHelperTest
{
    [Theory]
    [InlineData("123", "123")]
    [InlineData(" 123", "123")]
    [InlineData("123 ", "123")]
    [InlineData(" 123 ", "123")]
    [InlineData("1 23", "1%2023")]
    public void TheSpecialCharactersShouldBeEscaped(string id, string normalized)
    {
        // Arrange
        ActorId actorId;

        // Act
        actorId = id.ToActorId();

        // Assert
        _ = actorId.ToString().Should().Be(normalized);
    }

    [Theory]
    [InlineData("123", "123")]
    [InlineData("1%2023", "1 23")]
    public void TheSpecialCharactersShouldBeUnescaped(string normalized, string id)
    {
        // Arrange
        ActorId actorId = new(normalized);

        // Act
        string idString = actorId.ToUnescapeString();

        // Assert
        _ = idString.Should().Be(id);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("1%2023")]
    [InlineData("1@23")]
    [InlineData("1/23")]
    [InlineData("1\\23")]
    [InlineData("1[]2023")]
    [InlineData("12-3")]
    [InlineData("1_23")]
    public void EscapedAndUnescapedValuesShouldBeSame(string id)
    {
        // Arrange
        ActorId actorId;

        // Act
        actorId = id.ToActorId();
        string newId = actorId.ToUnescapeString();

        // Assert
        _ = newId.Should().Be(id);
    }
}