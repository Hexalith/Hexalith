namespace Hexalith.Application.Projections.Tests
{
    public class IdsCollectionProjectionHandlerTests
    {
        private readonly Mock<IProjectionFactory<IdCollection>> _factoryMock;
        private readonly TestIdsCollectionProjectionHandler _handler;

        public IdsCollectionProjectionHandlerTests()
        {
            _factoryMock = new Mock<IProjectionFactory<IdCollection>>();
            _handler = new TestIdsCollectionProjectionHandler(_factoryMock.Object);
        }

        [Fact]
        public async Task ApplyAsync_ShouldAddId_WhenNotExists()
        {
            // Arrange
            var baseEvent = new TestEvent();
            var metadata = new Metadata(
                new MessageMetadata("1", "Test", 1, new AggregateMetadata("TestAggregate", "1"), DateTimeOffset.UtcNow),
                new ContextMetadata("correlationId", "userId", "partitionId", DateTimeOffset.UtcNow, 1, "sessionId", Enumerable.Empty<string>())
            );
            metadata.AggregateGlobalId = "newId";

            _factoryMock.Setup(f => f.GetStateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IdCollection?)null);

            // Act
            await _handler.ApplyAsync(baseEvent, metadata, CancellationToken.None);

            // Assert
            _factoryMock.Verify(f => f.SetStateAsync(
                It.IsAny<string>(),
                It.Is<IdCollection>(c => c.Ids.Contains("newId")),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ApplyAsync_ShouldRemoveId_WhenExists()
        {
            // Arrange
            var baseEvent = new TestEvent();
            var metadata = new Metadata(
                new MessageMetadata("1", "Test", 1, new AggregateMetadata("TestAggregate", "1"), DateTimeOffset.UtcNow),
                new ContextMetadata("correlationId", "userId", "partitionId", DateTimeOffset.UtcNow, 1, "sessionId", Enumerable.Empty<string>())
            );
            metadata.AggregateGlobalId = "existingId";

            var idCollection = new IdCollection(null, new[] { "existingId" });

            _factoryMock.Setup(f => f.GetStateAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(idCollection);

            // Act
            await _handler.ApplyAsync(baseEvent, metadata, CancellationToken.None);

            // Assert
            _factoryMock.Verify(f => f.SetStateAsync(
                It.IsAny<string>(),
                It.Is<IdCollection>(c => !c.Ids.Contains("existingId")),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ApplyAsync_ShouldNotAddId_WhenRemoveEvent()
        {
            // Arrange
            var baseEvent = new TestEvent();
            var metadata = new Metadata(
                new MessageMetadata("1", "Test", 1, new AggregateMetadata("TestAggregate", "1"), DateTimeOffset.UtcNow),
                new ContextMetadata("correlationId", "userId", "partitionId", DateTimeOffset.UtcNow, 1, "sessionId", Enumerable.Empty<string>())
            );
            metadata.AggregateGlobalId = "newId";

            _handler.SetRemoveEvent(true);

            // Act
            await _handler.ApplyAsync(baseEvent, metadata, CancellationToken.None);

            // Assert
            _factoryMock.Verify(f => f.SetStateAsync(
                It.IsAny<string>(),
                It.IsAny<IdCollection>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        private class TestIdsCollectionProjectionHandler : IdsCollectionProjectionHandler<TestEvent>
        {
            private bool _isRemoveEvent;

            public TestIdsCollectionProjectionHandler(IProjectionFactory<IdCollection> factory)
                : base(factory)
            {
            }

            public void SetRemoveEvent(bool isRemoveEvent)
            {
                _isRemoveEvent = isRemoveEvent;
            }

            protected override bool IsRemoveEvent(TestEvent baseEvent)
            {
                return _isRemoveEvent;
            }
        }

        private class TestEvent
        {
        }
    }
}