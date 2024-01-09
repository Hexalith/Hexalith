// <copyright file="AggregateActorTest{submit}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using FluentAssertions;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Abstractions;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using Moq;

public partial class AggregateActorTest
{
    [Fact]
    public async Task SubmitCommandToActorWithCommandShouldStoreCommand()
    {
        DummyAggregateCommand1 command = new() { Id = "123456" };
        AggregateActorState currentState = new()
        {
            CommandCount = 2,
            EventSourceCount = 2,
            LastCommandProcessed = 2,
            LastMessagePublished = 2,
            MessageCount = 2,
            Reminder = null,
        };
        DummyTimerManager timerManager = new();
        ActorHost host = ActorHost.CreateForTest(
            typeof(AggregateActor),
            AggregateActorBase.GetAggregateActorName(command.AggregateName),
            new ActorTestOptions
            {
                ActorId = new ActorId(command.AggregateId),
                TimerManager = timerManager,
            });
        Metadata metadata = CreateMetadata(command);
        Mock<ICommandDispatcher> commandDispatcher = new(MockBehavior.Strict);
        Mock<IAggregateFactory> aggregateFactory = new(MockBehavior.Strict);
        Mock<IEventBus> eventBus = new(MockBehavior.Strict);
        Mock<INotificationBus> notificationBus = new(MockBehavior.Strict);
        Mock<ICommandBus> commandBus = new(MockBehavior.Strict);
        Mock<IRequestBus> requestBus = new(MockBehavior.Strict);
        Mock<IActorStateManager> actorStateManager = new(MockBehavior.Strict);
        Mock<IResiliencyPolicyProvider> resiliencyPolicyProvider = new(MockBehavior.Strict);
        actorStateManager
            .Setup(s => s.TryGetStateAsync<AggregateActorState>(
                        It.Is<string>(t => t == ActorConstants.AggregateStateStoreName),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<AggregateActorState>(true, currentState))
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.TryGetStateAsync<long>(
                        It.Is<string>(s => s == "CommandStream"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<long>(true, 2L))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<long>(
                        It.Is<string>(s => s == "CommandStreamId-" + metadata.Message.Id),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<long>))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s == "CommandStreamId-" + metadata.Message.Id),
                        It.Is<long>(l => l == 3L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s == "CommandStream"),
                        It.Is<long>(l => l == 3),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<CommandState>(
                        It.Is<string>(s => s.Contains('3') && s.Contains("Command")),
                        It.Is<CommandState>(s => s.Message.AggregateId == command.AggregateId),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<AggregateActorState>(
                        It.Is<string>(s => s == ActorConstants.AggregateStateStoreName),
                        It.Is<AggregateActorState>(s => s.CommandCount == 3 && s.Reminder != null),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SaveStateAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        AggregateActor actor = new(
            host,
            commandDispatcher.Object,
            aggregateFactory.Object,
            new DateTimeService(),
            eventBus.Object,
            notificationBus.Object,
            commandBus.Object,
            requestBus.Object,
            resiliencyPolicyProvider.Object,
            actorStateManager.Object);
        await actor.SubmitCommandAsync(new ActorCommandEnvelope([command], [metadata]));
        Mock.VerifyAll(actorStateManager, commandDispatcher, aggregateFactory, eventBus, notificationBus, commandBus, requestBus);
    }

    [Fact]
    public async Task SubmitCommandToActorWithIdMismatchShouldThrowException()
    {
        DummyAggregateCommand1 command = new() { Id = "123456" };
        ActorHost host = ActorHost.CreateForTest(typeof(AggregateActor), AggregateActorBase.GetAggregateActorName(command.AggregateName), new ActorTestOptions
        {
            ActorId = new ActorId("2594223"),
        });
        Mock<ICommandDispatcher> commandDispatcher = new(MockBehavior.Strict);
        Mock<IAggregateFactory> aggregateFactory = new(MockBehavior.Strict);
        Mock<IEventBus> eventBus = new(MockBehavior.Strict);
        Mock<INotificationBus> notificationBus = new(MockBehavior.Strict);
        Mock<ICommandBus> commandBus = new(MockBehavior.Strict);
        Mock<IRequestBus> requestBus = new(MockBehavior.Strict);
        Mock<IActorStateManager> actorStateManager = new(MockBehavior.Strict);
        Mock<IResiliencyPolicyProvider> resiliencyPolicyProvider = new(MockBehavior.Strict);
        AggregateActor actor = new(
            host,
            commandDispatcher.Object,
            aggregateFactory.Object,
            new DateTimeService(),
            eventBus.Object,
            notificationBus.Object,
            commandBus.Object,
            requestBus.Object,
            resiliencyPolicyProvider.Object,
            actorStateManager.Object);
        _ = await FluentActions
            .Awaiting(() => actor.SubmitCommandAsync(CreateEnvelope(command)))
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .Where(e => e.Message.Contains(command.Id) && e.Message.Contains(actor.Id.ToString()));
    }

    [Fact]
    public async Task SubmitCommandToActorWithNameMismatchShouldThrowException()
    {
        DummyAggregateCommand1 command = new() { Id = "123456" };
        ActorHost host = ActorHost.CreateForTest(typeof(AggregateActor), "TestActor", new ActorTestOptions
        {
            ActorId = new ActorId(command.AggregateId),
        });
        Mock<ICommandDispatcher> commandDispatcher = new(MockBehavior.Strict);
        Mock<IAggregateFactory> aggregateFactory = new(MockBehavior.Strict);
        Mock<IEventBus> eventBus = new(MockBehavior.Strict);
        Mock<INotificationBus> notificationBus = new(MockBehavior.Strict);
        Mock<ICommandBus> commandBus = new(MockBehavior.Strict);
        Mock<IRequestBus> requestBus = new(MockBehavior.Strict);
        Mock<IActorStateManager> actorStateManager = new(MockBehavior.Strict);
        Mock<IResiliencyPolicyProvider> resiliencyPolicyProvider = new(MockBehavior.Strict);
        AggregateActor actor = new(
            host,
            commandDispatcher.Object,
            aggregateFactory.Object,
            new DateTimeService(),
            eventBus.Object,
            notificationBus.Object,
            commandBus.Object,
            requestBus.Object,
            resiliencyPolicyProvider.Object,
            actorStateManager.Object);
        _ = await FluentActions
            .Awaiting(() => actor.SubmitCommandAsync(CreateEnvelope(command)))
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .Where(e =>
                e.Message.Contains(command.Id) &&
                e.Message.Contains(command.AggregateName) &&
                e.Message.Contains(actor.Host.ActorTypeInfo.ActorTypeName));
    }

    [Fact]
    public async Task SubmitCommandToNewActorShouldStoreCommand()
    {
        DummyAggregateCommand1 command = new() { Id = "123456" };
        DummyTimerManager timerManager = new();
        ActorHost host = ActorHost.CreateForTest(
            typeof(AggregateActor),
            AggregateActorBase.GetAggregateActorName(command.AggregateName),
            new ActorTestOptions
            {
                ActorId = new ActorId(command.AggregateId),
                TimerManager = timerManager,
            });
        Metadata metadata = CreateMetadata(command);
        Mock<ICommandDispatcher> commandDispatcher = new(MockBehavior.Strict);
        Mock<IAggregateFactory> aggregateFactory = new(MockBehavior.Strict);
        Mock<IEventBus> eventBus = new(MockBehavior.Strict);
        Mock<INotificationBus> notificationBus = new(MockBehavior.Strict);
        Mock<ICommandBus> commandBus = new(MockBehavior.Strict);
        Mock<IRequestBus> requestBus = new(MockBehavior.Strict);
        Mock<IActorStateManager> actorStateManager = new(MockBehavior.Strict);
        Mock<IResiliencyPolicyProvider> resiliencyPolicyProvider = new(MockBehavior.Strict);
        actorStateManager
            .Setup(s => s.TryGetStateAsync<AggregateActorState>(
                        It.Is<string>(t => t == ActorConstants.AggregateStateStoreName),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<AggregateActorState>))
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.TryGetStateAsync<long>(
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<long>))
            .Verifiable();
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s.Contains(metadata.Message.Id)),
                        It.Is<long>(l => l == 1),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s == "CommandStream"),
                        It.Is<long>(l => l == 1),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<CommandState>(
                        It.Is<string>(s => s.Contains('1') && s.Contains("Command")),
                        It.Is<CommandState>(s => s.Message.AggregateId == command.AggregateId),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<AggregateActorState>(
                        It.Is<string>(s => s == ActorConstants.AggregateStateStoreName),
                        It.Is<AggregateActorState>(s => s.CommandCount == 1 && s.Reminder != null),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SaveStateAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        AggregateActor actor = new(
            host,
            commandDispatcher.Object,
            aggregateFactory.Object,
            new DateTimeService(),
            eventBus.Object,
            notificationBus.Object,
            commandBus.Object,
            requestBus.Object,
            resiliencyPolicyProvider.Object,
            actorStateManager.Object);
        await actor.SubmitCommandAsync(new ActorCommandEnvelope([command], [metadata]));
        _ = timerManager.Reminders.Count.Should().Be(1);
        _ = timerManager.Timers.Count.Should().Be(1);
        Mock.VerifyAll(actorStateManager, commandDispatcher, aggregateFactory, eventBus, notificationBus, commandBus, requestBus);
    }

    private ActorCommandEnvelope CreateEnvelope(DummyAggregateCommand1 command)
                => new([command], [CreateMetadata(command)]);

    private Metadata CreateMetadata(BaseCommand command)
        => new(
                    UniqueIdHelper.GenerateUniqueStringId(),
                    command,
                    DateTimeOffset.Now,
                    new ContextMetadata(UniqueIdHelper.GenerateUniqueStringId(), "test-user", DateTimeOffset.Now, 100, "my session id"),
                    ["test", "actor"]);
}