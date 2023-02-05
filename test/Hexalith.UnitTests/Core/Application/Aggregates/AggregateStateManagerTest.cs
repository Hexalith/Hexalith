// <copyright file="AggregateStateManagerTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Aggregates;

using System;
using System.Threading.Tasks;

using FluentAssertions;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Application.Abstractions.Tasks;
using Hexalith.Application.Events;
using Hexalith.Application.States;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;
using Hexalith.UnitTests.Core.Application.Commands;

public class AggregateStateManagerTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(111)]
    public async Task Add_command_should_emit_events(int commandCount)
    {
        (AggregateStateManager stateManager, MemoryStateProvider stateProvider, MemoryEventBus bus) = await GetInitializedStateManager(commandCount);

        await stateManager.ContinueAsync(
            stateProvider,
            ResiliencyPolicy.None,
            async (name, data, start, period) => await Task.CompletedTask,
            CancellationToken.None);
        _ = bus.Stream.Should().HaveCount(commandCount);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    [InlineData(111)]
    public async Task Add_command_should_persist_events(int commandCount)
    {
        (AggregateStateManager stateManager, MemoryStateProvider stateProvider, _) = await GetInitializedStateManager(commandCount);

        await stateManager.ContinueAsync(
            stateProvider,
            ResiliencyPolicy.None,
            async (name, data, start, period) => await Task.CompletedTask,
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
    public async Task Add_command_should_persit_state_with_correct_command_version(int commandCount)
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
        AggregateStateManager stateManager = new(
            new DummyCommandDispatcher(),
            eventBus,
            new DateTimeService());
        for (int i = 0; i < commandCount; i++)
        {
            (BaseCommand command, Metadata meta) = GetCommand(i);
            await stateManager.AddCommandAsync(
                provider,
                command,
                meta,
                async (name, data, start, period) => await Task.CompletedTask,
                CancellationToken.None);
        }

        return (stateManager, provider, eventBus);
    }
}