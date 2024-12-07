// <copyright file="IdsCollectionProjectionHandlerTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Applications;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.UnitTests.Core.Domain.Events;

using Moq;

public class IdsCollectionProjectionHandlerTests
{
    private readonly Mock<IProjectionFactory<IdCollection>> _factoryMock;
    private readonly TestIdsCollectionProjectionHandler _handler;

    public IdsCollectionProjectionHandlerTests()
    {
        _factoryMock = new Mock<IProjectionFactory<IdCollection>>(MockBehavior.Strict);
        _handler = new TestIdsCollectionProjectionHandler(_factoryMock.Object)
        {
            PageSize = 5,
        };
    }

    [Fact]
    public async Task ApplyAsync_ShouldAddId_WhenNotExists()
    {
        // Arrange
        (DummyEvent1 baseEvent, Metadata metadata) = CreateEventAndMetadata();

        _factoryMock.Setup(f => f.GetStateAsync(
                It.Is<string>(p => p == metadata.Message.Aggregate.Name),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((IdCollection)null)
            .Verifiable(Times.Once);

        _factoryMock.Setup(
                f => f.SetStateAsync(
                It.Is<string>(p => p == metadata.Message.Aggregate.Name),
                It.Is<IdCollection>(c => c.Ids.Contains(metadata.AggregateGlobalId)),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);

        // Act
        await _handler.ApplyAsync(baseEvent, metadata, CancellationToken.None);

        // Assert
        _factoryMock.Verify();
    }

    [Fact]
    public async Task ApplyAsync_ShouldAddIdInNewPage_WhenPageFull()
    {
        // Arrange
        (DummyEvent1 baseEvent, Metadata metadata) = CreateEventAndMetadata();

        IdCollection idCollection = new(null, ["1", "2", "3", "4", "5"]);
        _factoryMock.Setup(f => f.GetStateAsync(
                It.Is<string>(p => p == metadata.Message.Aggregate.Name),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(idCollection)
            .Verifiable(Times.Once);

        _factoryMock.Setup(
                f => f.SetStateAsync(
                It.Is<string>(p => p == metadata.Message.Aggregate.Name),
                It.Is<IdCollection>(c => c.Ids == idCollection.Ids && c.NextPageId == 1),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);

        _factoryMock.Setup(
                f => f.SetStateAsync(
                It.Is<string>(p => p == metadata.Message.Aggregate.Name + "1"),
                It.Is<IdCollection>(c => c.Ids.Contains(metadata.AggregateGlobalId) && c.Ids.Count() == 1),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);

        // Act
        await _handler.ApplyAsync(baseEvent, metadata, CancellationToken.None);

        // Assert
        _factoryMock.Verify();
    }

    [Fact]
    public async Task ApplyAsync_ShouldNotAddIt_WhenExists()
    {
        // Arrange
        (DummyEvent1 baseEvent, Metadata metadata) = CreateEventAndMetadata();

        IdCollection idCollection = new(null, ["1", "2", metadata.AggregateGlobalId, "3"]);

        _factoryMock.Setup(f => f.GetStateAsync(
                It.Is<string>(p => p == metadata.Message.Aggregate.Name),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(idCollection)
            .Verifiable(Times.Once);

        _handler.SetRemoveEvent(false);

        // Act
        await _handler.ApplyAsync(baseEvent, metadata, CancellationToken.None);

        // Assert
        _factoryMock.Verify();
    }

    [Fact]
    public async Task ApplyAsync_ShouldNotRemoveIt_WhenNotExists()
    {
        // Arrange
        (DummyEvent1 baseEvent, Metadata metadata) = CreateEventAndMetadata();
        _factoryMock.Setup(f => f.GetStateAsync(
                It.Is<string>(p => p == metadata.Message.Aggregate.Name),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((IdCollection)null)
            .Verifiable(Times.Once);

        _handler.SetRemoveEvent(true);

        // Act
        await _handler.ApplyAsync(baseEvent, metadata, CancellationToken.None);

        // Assert
        _factoryMock.Verify();
    }

    [Fact]
    public async Task ApplyAsync_ShouldRemoveId_WhenExists()
    {
        // Arrange
        (DummyEvent1 baseEvent, Metadata metadata) = CreateEventAndMetadata();

        IdCollection idCollection = new(null, ["1", "2", metadata.AggregateGlobalId, "3"]);

        _factoryMock.Setup(f => f.GetStateAsync(
                It.Is<string>(p => p == metadata.Message.Aggregate.Name),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(idCollection)
            .Verifiable(Times.Once);

        _factoryMock.Setup(
                f => f.SetStateAsync(
                It.Is<string>(p => p == metadata.Message.Aggregate.Name),
                It.Is<IdCollection>(c => !c.Ids
                    .Contains(metadata.AggregateGlobalId) &&
                    c.Ids.Count() == idCollection.Ids.Count() - 1),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        _handler.SetRemoveEvent(true);

        // Act
        await _handler.ApplyAsync(baseEvent, metadata, CancellationToken.None);

        // Assert
        _factoryMock.Verify();
    }

    private static (DummyEvent1 Event, Metadata Metadata) CreateEventAndMetadata()
    {
        DummyEvent1 baseEvent = new("Test", 10);
        Metadata metadata = Metadata.CreateNew(baseEvent, "user me", "part1", TimeProvider.System.GetLocalNow());
        return (baseEvent, metadata);
    }

    private class TestIdsCollectionProjectionHandler(IProjectionFactory<IdCollection> factory) : IdsCollectionProjectionHandler<DummyEvent1>(factory)
    {
        private bool _isRemoveEvent;

        public void SetRemoveEvent(bool isRemoveEvent) => _isRemoveEvent = isRemoveEvent;

        protected override bool IsRemoveEvent(DummyEvent1 baseEvent) => _isRemoveEvent;
    }
}