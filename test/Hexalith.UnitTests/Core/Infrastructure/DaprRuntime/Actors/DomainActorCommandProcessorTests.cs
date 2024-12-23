// <copyright file="DomainActorCommandProcessorTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.Metadatas;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

using Microsoft.Extensions.Logging;

using Moq;

public class DomainActorCommandProcessorTests
{
    private static readonly string[] _scopes = ["123"];
    private readonly Mock<IActorProxyFactory> _actorProxyFactoryMock;
    private readonly Mock<ILogger<DomainActorCommandProcessor>> _loggerMock;
    private readonly DomainActorCommandProcessor _processor;

    public DomainActorCommandProcessorTests()
    {
        _actorProxyFactoryMock = new Mock<IActorProxyFactory>();
        _loggerMock = new Mock<ILogger<DomainActorCommandProcessor>>();
        _processor = new DomainActorCommandProcessor(_actorProxyFactoryMock.Object, false, _loggerMock.Object);
    }

    [Fact]
    public async Task SubmitAsync_ShouldLogAndCallActor_WhenValidParameters()
    {
        // Arrange
        object command = new DummyAggregateCommand1("1 23", "test 123");
        Metadata metadata = new(
            MessageMetadata.Create(
                command,
                DateTimeOffset.Now),
            new ContextMetadata(
                "123COR",
                "123USR",
                "PART1",
                DateTimeOffset.UtcNow,
                123,
                "123",
                _scopes));
        string normalizedActorId = metadata.AggregateGlobalId.ToActorId().ToString();
        Mock<IDomainAggregateActor> actorMock = new(MockBehavior.Strict);
        _ = _actorProxyFactoryMock
            .Setup(
                x => x.CreateActorProxy<IDomainAggregateActor>(
                It.Is<ActorId>(p => p.ToString() == normalizedActorId),
                It.IsAny<string>(),
                It.IsAny<ActorProxyOptions>()))
            .Returns(actorMock.Object);

        _ = actorMock
            .Setup(
                x => x.SubmitCommandAsJsonAsync(
                    It.Is<string>(p => p.Contains("Metadata") && p.Contains("Message"))))
            .Returns(Task.CompletedTask);

        // Act
        await _processor.SubmitAsync(command, metadata, CancellationToken.None);

        // Assert
        actorMock.Verify(x => x.SubmitCommandAsJsonAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_ShouldThrowArgumentNullException_WhenCommandIsNull()
    {
        // Arrange
        object command = null;
        Metadata metadata = new(
            new MessageMetadata(
                string.Empty,
                string.Empty,
                0,
                new AggregateMetadata(string.Empty, string.Empty),
                DateTimeOffset.MinValue),
            new ContextMetadata(
                string.Empty,
                string.Empty,
                string.Empty,
                null,
                null,
                null,
                []));

        // Act & Assert
        _ = await Assert.ThrowsAsync<ArgumentNullException>(() => _processor.SubmitAsync(command, metadata, CancellationToken.None));
    }

    [Fact]
    public async Task SubmitAsync_ShouldThrowArgumentNullException_WhenMetadataIsNull()
    {
        // Arrange
        object command = new();
        Metadata metadata = null;

        // Act & Assert
        _ = await Assert.ThrowsAsync<ArgumentNullException>(() => _processor.SubmitAsync(command, metadata, CancellationToken.None));
    }
}