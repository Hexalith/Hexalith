// <copyright file="SequentialStringListActorTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using FluentAssertions;

using Hexalith.Infrastructure.DaprRuntime.Actors;

using Moq;

using Xunit;

public class SequentialStringListActorTests
{
    private readonly ActorHost _actorHost;
    private readonly Mock<IActorStateManager> _stateManagerMock;

    public SequentialStringListActorTests()
    {
        // Create a mock IActorStateManager
        _stateManagerMock = new Mock<IActorStateManager>(MockBehavior.Strict);

        // Create an ActorHost. Here, we can just pass null for services and use a test actor type.
        _actorHost = ActorHost.CreateForTest<SequentialStringListActor>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddValue_WhenNotExists()
    {
        // Arrange
        string valueToAdd = "testValue";

        // Initially, we have no pages in state
        _ = _stateManagerMock
            .Setup(x => x.TryGetStateAsync<IEnumerable<string>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<IEnumerable<string>>(false, null));

        // Expect that we set the state for page 0 with the new value
        _ = _stateManagerMock
            .Setup(x => x.SetStateAsync("0", It.Is<IEnumerable<string>>(s => s.Contains(valueToAdd)), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _ = _stateManagerMock
            .Setup(x => x.SaveStateAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock.Object);

        // Act
        await actor.AddAsync(valueToAdd);

        // Assert
        _stateManagerMock.VerifyAll();
    }

    [Fact]
    public async Task AddAsync_ShouldNotAddValue_WhenAlreadyExists()
    {
        // Arrange
        string valueToAdd = "existingValue";
        List<string> existingData = [valueToAdd];

        _ = _stateManagerMock
            .Setup(x => x.TryGetStateAsync<IEnumerable<string>>("0", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<IEnumerable<string>>(true, existingData));

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock.Object);

        // Act
        await actor.AddAsync(valueToAdd);

        // Assert: No state changes should occur since the value already exists.
        _stateManagerMock.Verify(x => x.SetStateAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Never);
        _stateManagerMock.Verify(x => x.SaveStateAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenValueDoesNotExist()
    {
        // Arrange
        string valueToCheck = "nonExistentValue";
        _ = _stateManagerMock
            .Setup(x => x.TryGetStateAsync<IEnumerable<string>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<IEnumerable<string>>(false, null));

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock.Object);

        // Act
        bool exists = await actor.ExistsAsync(valueToCheck);

        // Assert
        _ = exists.Should().BeFalse();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenValueExists()
    {
        // Arrange
        string valueToCheck = "existingValue";
        List<string> data = ["existingValue", "anotherValue"];

        _ = _stateManagerMock
            .Setup(x => x.TryGetStateAsync<IEnumerable<string>>("0", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<IEnumerable<string>>(true, data));

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock.Object);

        // Act
        bool exists = await actor.ExistsAsync(valueToCheck);

        // Assert
        _ = exists.Should().BeTrue();
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnNull_WhenNoSuchPage()
    {
        // Arrange
        int pageNumber = 1;
        _ = _stateManagerMock
            .Setup(x => x.TryGetStateAsync<IEnumerable<string>>(pageNumber.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<IEnumerable<string>>(false, null));

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock.Object);

        // Act
        IEnumerable<string> result = await actor.ReadAsync(pageNumber);

        // Assert
        _ = result.Should().BeNull();
    }

    [Fact]
    public async Task ReadAsync_ShouldReturnPageData_WhenPageExists()
    {
        // Arrange
        int pageNumber = 0;
        List<string> data = ["value1", "value2"];

        _ = _stateManagerMock
            .Setup(x => x.TryGetStateAsync<IEnumerable<string>>(pageNumber.ToString(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<IEnumerable<string>>(true, data));

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock.Object);

        // Act
        IEnumerable<string> result = await actor.ReadAsync(pageNumber);

        // Assert
        _ = result.Should().NotBeNull().And.BeEquivalentTo(data);
    }

    [Fact]
    public async Task RemoveAsync_ShouldDoNothing_WhenValueDoesNotExist()
    {
        // Arrange
        string valueToRemove = "missingValue";
        List<string> existingData = ["value1", "value2"];

        _ = _stateManagerMock
            .Setup(x => x.TryGetStateAsync<IEnumerable<string>>("0", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<IEnumerable<string>>(true, existingData));
        _ = _stateManagerMock
            .Setup(x => x.TryGetStateAsync<IEnumerable<string>>("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<IEnumerable<string>>(false, null));

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock.Object);

        // Act
        await actor.RemoveAsync(valueToRemove);

        // Assert: No changes since value not found
        _stateManagerMock.Verify(x => x.SetStateAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()), Times.Never);
        _stateManagerMock.Verify(x => x.SaveStateAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveValue_WhenValueExists()
    {
        // Arrange
        string valueToRemove = "valueToRemove";
        List<string> existingData = ["valueToRemove", "anotherValue"];

        _ = _stateManagerMock
            .Setup(x => x.TryGetStateAsync<IEnumerable<string>>("0", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<IEnumerable<string>>(true, existingData));

        _ = _stateManagerMock
            .Setup(x => x.SetStateAsync("0", It.Is<IEnumerable<string>>(s => !s.Contains(valueToRemove)), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _ = _stateManagerMock
            .Setup(x => x.SaveStateAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock.Object);

        // Act
        await actor.RemoveAsync(valueToRemove);

        // Assert
        _stateManagerMock.VerifyAll();
    }
}