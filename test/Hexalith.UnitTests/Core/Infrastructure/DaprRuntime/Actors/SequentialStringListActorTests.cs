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

using Hexalith.Infrastructure.DaprRuntime.Actors;

using NSubstitute;

using Shouldly;

using Xunit;

public class SequentialStringListActorTests
{
    private readonly ActorHost _actorHost;
    private readonly IActorStateManager _stateManagerMock;

    public SequentialStringListActorTests()
    {
        // Create a mock IActorStateManager
        _stateManagerMock = Substitute.For<IActorStateManager>();

        // Create an ActorHost. Here, we can just pass null for services and use a test actor type.
        _actorHost = ActorHost.CreateForTest<SequentialStringListActor>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddValue_WhenNotExists()
    {
        // Arrange
        string valueToAdd = "testValue";

        // Initially, we have no pages in state
        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(false, null));

        // Expect that we set the state for page 0 with the new value
        _stateManagerMock
            .SetStateAsync("0", Arg.Is<IEnumerable<string>>(s => s.Contains(valueToAdd)), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _stateManagerMock
            .SaveStateAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock);

        // Act
        await actor.AddAsync(valueToAdd);

        // Assert
        await _stateManagerMock.Received(1).SetStateAsync("0", Arg.Is<IEnumerable<string>>(s => s.Contains(valueToAdd)), Arg.Any<CancellationToken>());
        await _stateManagerMock.Received(1).SaveStateAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddAsync_ShouldAppendValue_ToPartiallyFilledExistingPage()
    {
        // Arrange
        string existingValue = "existingValue";
        string newValue = "newValue";

        // The page is not full, so we can add more items to it.
        List<string> existingData = [existingValue];

        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>("0", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(true, existingData));
        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>("1", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(false, null));

        // We expect the updated list to contain both existingValue and newValue.
        _stateManagerMock
            .SetStateAsync("0", Arg.Is<IEnumerable<string>>(s => s.Contains(existingValue) && s.Contains(newValue)), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _stateManagerMock
            .SaveStateAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock);

        // Act
        await actor.AddAsync(newValue);

        // Assert
        await _stateManagerMock.Received(1).SetStateAsync("0", Arg.Is<IEnumerable<string>>(s => s.Contains(existingValue) && s.Contains(newValue)), Arg.Any<CancellationToken>());
        await _stateManagerMock.Received(1).SaveStateAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddAsync_ShouldCreateNewPage_WhenAllExistingPagesAreFull()
    {
        // Arrange
        // Assume page 0 is full
        List<string> fullPageData = [.. Enumerable.Range(0, 1024).Select(i => $"value{i}")];

        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>("0", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(true, fullPageData));

        // No page 1 yet
        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>("1", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(false, null));

        string newValue = "newPageValue";
        _stateManagerMock
            .SetStateAsync("1", Arg.Is<IEnumerable<string>>(s => s.Count() == 1 && s.Contains(newValue)), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _stateManagerMock
            .SaveStateAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock);

        // Act
        await actor.AddAsync(newValue);

        // Assert: The new page should be created at page 1
        await _stateManagerMock.Received(1).SetStateAsync("1", Arg.Is<IEnumerable<string>>(s => s.Count() == 1 && s.Contains(newValue)), Arg.Any<CancellationToken>());
        await _stateManagerMock.Received(1).SaveStateAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task AddAsync_ShouldNotAddValue_WhenAlreadyExists()
    {
        // Arrange
        string valueToAdd = "existingValue";
        List<string> existingData = [valueToAdd];

        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>("0", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(true, existingData));

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock);

        // Act
        await actor.AddAsync(valueToAdd);

        // Assert: No state changes should occur since the value already exists.
        await _stateManagerMock.DidNotReceive().SetStateAsync(Arg.Any<string>(), Arg.Any<IEnumerable<string>>(), Arg.Any<CancellationToken>());
        await _stateManagerMock.DidNotReceive().SaveStateAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExistsAsync_ShouldCheckInSecondPage_WhenFirstPageDoesNotContainValue()
    {
        // Arrange
        string targetValue = "valueInSecondPage";
        List<string> firstPageData = ["notTargetValue"];
        List<string> secondPageData = ["anotherValue", targetValue];

        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>("0", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(true, firstPageData));

        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>("1", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(true, secondPageData));

        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>("2", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(false, null));

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock);

        // Act
        bool exists = await actor.ExistsAsync(targetValue);

        // Assert
        exists.ShouldBeTrue();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenValueDoesNotExist()
    {
        // Arrange
        string valueToCheck = "nonExistentValue";
        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(false, null));

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock);

        // Act
        bool exists = await actor.ExistsAsync(valueToCheck);

        // Assert
        exists.ShouldBeFalse();
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenValueExists()
    {
        // Arrange
        string valueToCheck = "existingValue";
        List<string> data = ["existingValue", "anotherValue"];

        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>("0", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(true, data));

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock);

        // Act
        bool exists = await actor.ExistsAsync(valueToCheck);

        // Assert
        exists.ShouldBeTrue();
    }

    [Fact]
    public async Task RemoveAsync_ShouldDoNothing_WhenValueDoesNotExist()
    {
        // Arrange
        string valueToRemove = "missingValue";
        List<string> existingData = ["value1", "value2"];

        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>("0", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(true, existingData));
        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>("1", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(false, null));

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock);

        // Act
        await actor.RemoveAsync(valueToRemove);

        // Assert: No changes since value not found
        await _stateManagerMock.DidNotReceive().SetStateAsync(Arg.Any<string>(), Arg.Any<IEnumerable<string>>(), Arg.Any<CancellationToken>());
        await _stateManagerMock.DidNotReceive().SaveStateAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveValue_FromSecondPage()
    {
        // Arrange
        string valueToRemove = "removeMe";
        List<string> firstPageData = ["value1", "value2"];
        List<string> secondPageData = ["valueA", valueToRemove, "valueB"];

        // Use a counter to track calls
        int callCount = 0;
        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                string key = callInfo.ArgAt<string>(0);
                return key switch
                {
                    "0" when callCount++ == 0 => new ConditionalValue<IEnumerable<string>>(true, firstPageData),
                    "1" => new ConditionalValue<IEnumerable<string>>(true, secondPageData),
                    _ => new ConditionalValue<IEnumerable<string>>(false, null)
                };
            });

        // After removal, second page should not contain "removeMe"
        _stateManagerMock
            .SetStateAsync("1", Arg.Is<IEnumerable<string>>(s => !s.Contains(valueToRemove)), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _stateManagerMock
            .SaveStateAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock);

        // Act
        await actor.RemoveAsync(valueToRemove);

        // Assert
        await _stateManagerMock.Received(1).SetStateAsync("1", Arg.Is<IEnumerable<string>>(s => !s.Contains(valueToRemove)), Arg.Any<CancellationToken>());
        await _stateManagerMock.Received(1).SaveStateAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveValue_WhenValueExists()
    {
        // Arrange
        string valueToRemove = "valueToRemove";
        List<string> existingData = ["valueToRemove", "anotherValue"];

        _stateManagerMock
            .TryGetStateAsync<IEnumerable<string>>("0", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<IEnumerable<string>>(true, existingData));

        _stateManagerMock
            .SetStateAsync("0", Arg.Is<IEnumerable<string>>(s => !s.Contains(valueToRemove)), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _stateManagerMock
            .SaveStateAsync(Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        SequentialStringListActor actor = new(_actorHost, _stateManagerMock);

        // Act
        await actor.RemoveAsync(valueToRemove);

        // Assert
        await _stateManagerMock.Received(1).SetStateAsync("0", Arg.Is<IEnumerable<string>>(s => !s.Contains(valueToRemove)), Arg.Any<CancellationToken>());
        await _stateManagerMock.Received(1).SaveStateAsync(Arg.Any<CancellationToken>());
    }
}
