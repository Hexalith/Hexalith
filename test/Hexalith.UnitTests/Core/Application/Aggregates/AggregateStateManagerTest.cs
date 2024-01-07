// <copyright file="AggregateStateManagerTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Aggregates;

using System;
using System.Threading.Tasks;

using FluentAssertions;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Notifications;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;
using Hexalith.UnitTests.Core.Application.Commands;

using Microsoft.Extensions.Logging;

using Moq;

public class AggregateStateManagerTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(111)]
    public async Task AddCommandShouldPersistEvents(int commandCount)
    {
        (AggregateStateManager stateManager, MemoryStateProvider stateProvider, _) = await GetInitializedStateManager(commandCount);

        _ = await stateManager.ContinueAsync(
            stateProvider,
            new Mock<IRetryCallbackManager>().Object,
            ResiliencyPolicy.None,
            null,
            (_) => new DummyAggregate(),
            CancellationToken.None);

        _ = stateProvider.State.ContainsKey("State").Should().BeTrue();
        _ = stateProvider.State["State"].Should().BeOfType<AggregateState>();
        AggregateState state = (AggregateState)stateProvider.State["State"];
        _ = state.Should().NotBeNull();
        _ = state.LastCommandDone.Should().Be(commandCount);
        _ = state.CommandStreamVersion.Should().Be(commandCount);
        _ = state.EventStreamVersion.Should().Be(commandCount);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(111)]
    public async Task AddCommandShouldPersistStateWithCorrectCommandVersion(int commandCount)
    {
        (_, MemoryStateProvider stateProvider, _) = await GetInitializedStateManager(commandCount);

        _ = stateProvider.State.ContainsKey("State").Should().BeTrue();
        _ = stateProvider.State["State"].Should().BeOfType<AggregateState>();
        AggregateState state = (AggregateState)stateProvider.State["State"];
        _ = state.Should().NotBeNull();
        _ = state.LastCommandDone.Should().Be(0L);
        _ = state.LastEventPublished.Should().Be(0L);
        _ = state.CommandStreamVersion.Should().Be(commandCount);
        _ = state.EventStreamVersion.Should().Be(0L);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task ContinueAsyncShouldEmitEvents(int commandCount)
    {
        (AggregateStateManager stateManager, MemoryStateProvider stateProvider, MemoryEventBus bus) = await GetInitializedStateManager(commandCount);

        _ = await stateManager
            .ContinueAsync(
                stateProvider,
                new Mock<IRetryCallbackManager>().Object,
                ResiliencyPolicy.None,
                null,
                (_) => new DummyAggregate(),
                CancellationToken.None);
        _ = bus.Stream.Should().HaveCount(commandCount);
    }

    private static (BaseCommand Command, Metadata Meta) GetCommand(int id)
    {
        DummyCommand1 command = new("Test" + id.ToInvariantString(), id);
        Metadata meta = new(
                "Id-" + id.ToInvariantString(),
                command,
                DateTimeOffset.UtcNow,
                new ContextMetadata(
                    "COR" + id.ToInvariantString(),
                    "TestUser" + id.ToInvariantString(),
                    null,
                    null,
                    null),
                null);
        return (command, meta);
    }

    private static async Task<(AggregateStateManager StateManager, MemoryStateProvider StateProvider, MemoryEventBus EventBus)> GetInitializedStateManager(int commandCount)
    {
        MemoryStateProvider provider = new();
        MemoryEventBus eventBus = new(new DateTimeService());
        MemoryNotificationBus notificationBus = new(new DateTimeService());
        AggregateStateManager stateManager = new(
            new DummyCommandDispatcher(),
            eventBus,
            notificationBus,
            new DateTimeService(),
            new Mock<ILogger<AggregateStateManager>>().Object);
        for (int i = 0; i < commandCount; i++)
        {
            (BaseCommand command, Metadata meta) = GetCommand(i);
            await stateManager
                .AddCommandAsync(
                    provider,
                    new Mock<IRetryCallbackManager>().Object,
                    command.IntoArray(),
                    meta.IntoArray(),
                    CancellationToken.None)
                .ConfigureAwait(false);
        }

        return (stateManager, provider, eventBus);
    }
}

public class DummyAggregate : IAggregate
{
    public DummyAggregate()
    {
        AggregateId = "123";
        AggregateName = "Test";
    }

    public string AggregateId { get; }

    public string AggregateName { get; }

    public IAggregate Apply(BaseEvent domainEvent) => new DummyAggregate();

    public bool IsInitialized() => true;
}