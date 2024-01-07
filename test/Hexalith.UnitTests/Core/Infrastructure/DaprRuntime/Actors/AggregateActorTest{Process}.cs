// <copyright file="AggregateActorTest{Process}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using Dapr.Actors;
using Dapr.Actors.Runtime;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Application.Metadatas;
using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Aggregates;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.DaprRuntime.Abstractions;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using Moq;

public partial class AggregateActorTest
{
    [Fact]
    public async Task ProcessCommandToActorWithFirstCommandShouldStoreCommand()
    {
        DummyAggregateCommand1 command = new() { Id = "123456" };
        AggregateActorState currentState = new()
        {
            CommandCount = 1,
            EventSourceCount = 0,
            LastCommandProcessed = 0,
            LastMessagePublished = 0,
            MessageCount = 0,
            Reminder = new TimeSpan(0, 0, 1),
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
        DummyAggregateEvent1 ev = new() { Id = command.Id };
        Mock<ICommandDispatcher> commandDispatcher = new(MockBehavior.Strict);
        Mock<IAggregateFactory> aggregateFactory = new(MockBehavior.Strict);
        Mock<IEventBus> eventBus = new(MockBehavior.Strict);
        Mock<INotificationBus> notificationBus = new(MockBehavior.Strict);
        Mock<ICommandBus> commandBus = new(MockBehavior.Strict);
        Mock<IRequestBus> requestBus = new(MockBehavior.Strict);
        Mock<IActorStateManager> actorStateManager = new(MockBehavior.Strict);
        Mock<IResiliencyPolicyProvider> resiliencyPolicyProvider = new(MockBehavior.Strict);
        commandDispatcher.Setup(s => s.DoAsync(
            It.Is<ICommand>(c => c.AggregateId == command.AggregateId && c.TypeName == command.TypeName),
            It.Is<IAggregate>(a => a.AggregateName == DummyAggregate.GetAggregateName()),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync([ev])
            .Verifiable(Times.Once);

        resiliencyPolicyProvider
            .Setup(p => p.GetPolicy(It.Is<string>(s => s == AggregateActorBase.GetAggregateActorName(DummyAggregate.GetAggregateName()))))
            .Returns(ResiliencyPolicy.None)
            .Verifiable(Times.Once);

        aggregateFactory
            .Setup(c => c.Create(It.Is<string>(s => s == DummyAggregate.GetAggregateName())))
            .Returns(new DummyAggregate())
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<AggregateActorState>(
                        It.Is<string>(t => t == ActorConstants.AggregateStateStoreName),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<AggregateActorState>(true, currentState))
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.TryGetStateAsync<CommandState>(
                        It.Is<string>(s => s == "CommandStream1"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<CommandState>(true, new CommandState(DateTimeOffset.Now, command, metadata)))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<long>(
                        It.Is<string>(s => s == "EventSourceStream"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<long>))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<long>(
                        It.Is<string>(s => s.StartsWith("EventSourceStreamId-")),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<long>))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<long>(
                        It.Is<string>(s => s == "MessageStream"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<long>))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<long>(
                        It.Is<string>(s => s.StartsWith("MessageStreamId-")),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<long>))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<TaskProcessor>(
                        It.Is<string>(s => s == "TaskProcessor-1"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<TaskProcessor>))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.SetStateAsync<AggregateActorState>(
                        It.Is<string>(s => s == ActorConstants.AggregateStateStoreName),
                        It.Is<AggregateActorState>(s =>
                            s.CommandCount == 1 &&
                            s.LastCommandProcessed == 1 &&
                            s.MessageCount == 1 &&
                            s.LastMessagePublished == 0 &&
                            s.Reminder != null),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s.StartsWith("EventSourceStreamId-")),
                        It.Is<long>(s => s == 1L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s == "EventSourceStream"),
                        It.Is<long>(s => s == 1L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<EventState>(
                        It.Is<string>(s => s == "EventSourceStream1"),
                        It.Is<EventState>(s =>
                            s.Message.AggregateId == command.AggregateId &&
                            s.Message.AggregateName == command.AggregateName &&
                            s.Metadata.Context.CorrelationId == metadata.Context.CorrelationId),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s.StartsWith("MessageStreamId-")),
                        It.Is<long>(s => s == 1L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s == "MessageStream"),
                        It.Is<long>(s => s == 1L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<MessageState>(
                        It.Is<string>(s => s == "MessageStream1"),
                        It.Is<MessageState>(s =>
                            s.Message.AggregateId == command.AggregateId &&
                            s.Message.AggregateName == command.AggregateName &&
                            s.Metadata.Context.CorrelationId == metadata.Context.CorrelationId),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<TaskProcessor>(
                        It.Is<string>(s => s == "TaskProcessor-1"),
                        It.Is<TaskProcessor>(s =>
                            s.Status == TaskProcessorStatus.Completed),
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
        _ = await actor.ProcessNextCommandAsync();
        Mock.VerifyAll(actorStateManager, commandDispatcher, aggregateFactory, eventBus, notificationBus, commandBus, requestBus);
    }
}