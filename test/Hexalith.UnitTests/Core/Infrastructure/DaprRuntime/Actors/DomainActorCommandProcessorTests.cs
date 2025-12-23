// <copyright file="DomainActorCommandProcessorTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Commons.Metadatas;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

using Microsoft.Extensions.Logging;

using NSubstitute;

public class DomainActorCommandProcessorTests
{
    private static readonly string[] _scopes = ["123"];
    private readonly IActorProxyFactory _actorProxyFactoryMock;
    private readonly ILogger<DomainActorCommandProcessor> _loggerMock;
    private readonly DomainActorCommandProcessor _processor;

    public DomainActorCommandProcessorTests()
    {
        _actorProxyFactoryMock = Substitute.For<IActorProxyFactory>();
        _loggerMock = Substitute.For<ILogger<DomainActorCommandProcessor>>();
        _processor = new DomainActorCommandProcessor(_actorProxyFactoryMock, false, _loggerMock);
    }

    [Fact]
    public async Task SubmitAsync_ShouldLogAndCallActor_WhenValidParameters()
    {
        // Arrange
        object command = new DummyAggregateCommand1("1 23", "test 123");
        Metadata metadata = new(
                command.CreateMessageMetadata(DateTimeOffset.Now),
            new ContextMetadata(
                "123COR",
                "123USR",
                "PART1",
                DateTimeOffset.UtcNow,
                TimeSpan.FromSeconds(102),
                123,
                "ETH-TEST",
                "123",
                _scopes));
        string normalizedActorId = metadata.DomainGlobalId.ToActorId().ToString();
        IDomainAggregateActor actorMock = Substitute.For<IDomainAggregateActor>();
        _actorProxyFactoryMock
            .CreateActorProxy<IDomainAggregateActor>(
                Arg.Is<ActorId>(p => p.ToString() == normalizedActorId),
                Arg.Any<string>(),
                Arg.Any<ActorProxyOptions>())
            .Returns(actorMock);

        actorMock
            .SubmitCommandAsJsonAsync(Arg.Is<string>(p => p.Contains("Metadata") && p.Contains("Message")))
            .Returns(Task.CompletedTask);

        // Act
        await _processor.SubmitAsync(command, metadata, CancellationToken.None);

        // Assert
        await actorMock.Received(1).SubmitCommandAsJsonAsync(Arg.Any<string>());
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
                new DomainMetadata(string.Empty, string.Empty),
                DateTimeOffset.MinValue),
            new ContextMetadata(
                string.Empty,
                string.Empty,
                string.Empty,
                null,
                null,
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
