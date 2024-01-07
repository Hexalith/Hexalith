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
    public async Task ProcessWithFirstCommandShouldStoreEventAndMessage()
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

    [Fact]
    public async Task ProcessWithManyCommandsShouldStoreEventsAndMessages()
    {
        DummyAggregateCommand1 command8 = new() { Id = "123456", Name = "Command8" };
        DummyAggregateCommand1 command9 = new() { Id = "123456", Name = "Command9" };
        DummyAggregateCommand1 command10 = new() { Id = "123456", Name = "Command10" };
        DummyAggregateEvent1 event1 = new() { Id = "123456", Name = "Event1" };
        DummyAggregateEvent1 event2 = new() { Id = "123456", Name = "Event2" };
        AggregateActorState currentState = new()
        {
            CommandCount = 10,
            EventSourceCount = 2,
            LastCommandProcessed = 7,
            LastMessagePublished = 5,
            MessageCount = 5,
            Reminder = new TimeSpan(0, 0, 1),
        };
        DummyTimerManager timerManager = new();
        ActorHost host = ActorHost.CreateForTest(
            typeof(AggregateActor),
            AggregateActorBase.GetAggregateActorName(command8.AggregateName),
            new ActorTestOptions
            {
                ActorId = new ActorId(command8.AggregateId),
                TimerManager = timerManager,
            });
        Metadata metadata = CreateMetadata(command8);
        Mock<ICommandDispatcher> commandDispatcher = new(MockBehavior.Strict);
        Mock<IAggregateFactory> aggregateFactory = new(MockBehavior.Strict);
        Mock<IEventBus> eventBus = new(MockBehavior.Strict);
        Mock<INotificationBus> notificationBus = new(MockBehavior.Strict);
        Mock<ICommandBus> commandBus = new(MockBehavior.Strict);
        Mock<IRequestBus> requestBus = new(MockBehavior.Strict);
        Mock<IActorStateManager> actorStateManager = new(MockBehavior.Strict);
        Mock<IResiliencyPolicyProvider> resiliencyPolicyProvider = new(MockBehavior.Strict);
        commandDispatcher.Setup(s => s.DoAsync(
            It.Is<ICommand>(c => c.AggregateId == command8.AggregateId && c.TypeName == command8.TypeName && ((DummyAggregateCommand1)c).Name == command8.Name),
            It.Is<IAggregate>(a => a.AggregateName == DummyAggregate.GetAggregateName()),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync([new DummyAggregateEvent1() { Id = command8.Id, Name = command8.Name }])
            .Verifiable(Times.Once);
        commandDispatcher.Setup(s => s.DoAsync(
            It.Is<ICommand>(c => c.AggregateId == command9.AggregateId && c.TypeName == command9.TypeName && ((DummyAggregateCommand1)c).Name == command9.Name),
            It.Is<IAggregate>(a => a.AggregateName == DummyAggregate.GetAggregateName()),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync([new DummyAggregateEvent1() { Id = command9.Id, Name = command9.Name }])
            .Verifiable(Times.Once);
        commandDispatcher.Setup(s => s.DoAsync(
            It.Is<ICommand>(c => c.AggregateId == command10.AggregateId && c.TypeName == command10.TypeName && ((DummyAggregateCommand1)c).Name == command10.Name),
            It.Is<IAggregate>(a => a.AggregateName == DummyAggregate.GetAggregateName()),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync([new DummyAggregateEvent1() { Id = command10.Id, Name = command10.Name }])
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
                        It.Is<string>(s => s == "CommandStream8"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<CommandState>(true, new CommandState(DateTimeOffset.Now, command8, metadata)))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<CommandState>(
                        It.Is<string>(s => s == "CommandStream9"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<CommandState>(true, new CommandState(DateTimeOffset.Now, command9, metadata)))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<CommandState>(
                        It.Is<string>(s => s == "CommandStream10"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<CommandState>(true, new CommandState(DateTimeOffset.Now, command10, metadata)))
            .Verifiable(Times.Once);

        _ = actorStateManager
            .SetupSequence(s => s.TryGetStateAsync<long>(
                        It.Is<string>(s => s == "EventSourceStream"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<long>(true, 2L))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<long>(true, 3L))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<long>(true, 4L))
            .Throws(new InvalidOperationException());

        actorStateManager
            .Setup(s => s.TryGetStateAsync<EventState>(
                        It.Is<string>(s => s == "EventSourceStream1"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<EventState>(true, new EventState(DateTimeOffset.Now, event1, metadata)))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<EventState>(
                        It.Is<string>(s => s == "EventSourceStream2"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<EventState>(true, new EventState(DateTimeOffset.Now, event2, metadata)))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<long>(
                        It.Is<string>(s => s.StartsWith("EventSourceStreamId-")),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<long>))
            .Verifiable(Times.Exactly(3));

        _ = actorStateManager
            .SetupSequence(s => s.TryGetStateAsync<long>(
                        It.Is<string>(s => s == "MessageStream"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<long>(true, 5))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<long>(true, 6))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<long>(true, 7))
            .Throws(new InvalidOperationException());

        actorStateManager
            .Setup(s => s.TryGetStateAsync<long>(
                        It.Is<string>(s => s.StartsWith("MessageStreamId-")),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<long>))
            .Verifiable(Times.Exactly(3));

        actorStateManager
            .Setup(s => s.TryGetStateAsync<TaskProcessor>(
                        It.Is<string>(s => s == "TaskProcessor-8"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<TaskProcessor>))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<TaskProcessor>(
                        It.Is<string>(s => s == "TaskProcessor-9"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<TaskProcessor>))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.TryGetStateAsync<TaskProcessor>(
                        It.Is<string>(s => s == "TaskProcessor-10"),
                        It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Dapr.Actors.Runtime.ConditionalValue<TaskProcessor>))
            .Verifiable(Times.Once);

        actorStateManager
            .Setup(s => s.SetStateAsync<AggregateActorState>(
                        It.Is<string>(s => s == ActorConstants.AggregateStateStoreName),
                        It.Is<AggregateActorState>(s =>
                            s.CommandCount == 10 &&
                            s.EventSourceCount == 3 &&
                            s.LastCommandProcessed == 8 &&
                            s.MessageCount == 6 &&
                            s.LastMessagePublished == 5 &&
                            s.Reminder != null),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<AggregateActorState>(
                        It.Is<string>(s => s == ActorConstants.AggregateStateStoreName),
                        It.Is<AggregateActorState>(s =>
                            s.CommandCount == 10 &&
                            s.EventSourceCount == 4 &&
                            s.LastCommandProcessed == 9 &&
                            s.MessageCount == 7 &&
                            s.LastMessagePublished == 5 &&
                            s.Reminder != null),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<AggregateActorState>(
                        It.Is<string>(s => s == ActorConstants.AggregateStateStoreName),
                        It.Is<AggregateActorState>(s =>
                            s.CommandCount == 10 &&
                            s.EventSourceCount == 5 &&
                            s.LastCommandProcessed == 10 &&
                            s.MessageCount == 8 &&
                            s.LastMessagePublished == 5 &&
                            s.Reminder != null),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s.StartsWith("EventSourceStreamId-")),
                        It.Is<long>(s => s == 3L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s.StartsWith("EventSourceStreamId-")),
                        It.Is<long>(s => s == 4L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s.StartsWith("EventSourceStreamId-")),
                        It.Is<long>(s => s == 5L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<EventState>(
                        It.Is<string>(s => s == "EventSourceStream3"),
                        It.Is<EventState>(s =>
                            s.Message.AggregateId == command8.AggregateId &&
                            s.Message.AggregateName == command8.AggregateName &&
                            ((DummyAggregateEvent1)s.Message).Name == command8.Name &&
                            s.Metadata.Context.CorrelationId == metadata.Context.CorrelationId),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<EventState>(
                        It.Is<string>(s => s == "EventSourceStream4"),
                        It.Is<EventState>(s =>
                            s.Message.AggregateId == command9.AggregateId &&
                            s.Message.AggregateName == command9.AggregateName &&
                            ((DummyAggregateEvent1)s.Message).Name == command9.Name &&
                            s.Metadata.Context.CorrelationId == metadata.Context.CorrelationId),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<EventState>(
                        It.Is<string>(s => s == "EventSourceStream5"),
                        It.Is<EventState>(s =>
                            s.Message.AggregateId == command10.AggregateId &&
                            s.Message.AggregateName == command10.AggregateName &&
                            ((DummyAggregateEvent1)s.Message).Name == command10.Name &&
                            s.Metadata.Context.CorrelationId == metadata.Context.CorrelationId),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s == "EventSourceStream"),
                        3L,
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s == "EventSourceStream"),
                        4L,
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s == "EventSourceStream"),
                        5L,
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s.StartsWith("MessageStreamId-")),
                        It.Is<long>(s => s == 6L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s.StartsWith("MessageStreamId-")),
                        It.Is<long>(s => s == 7L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s.StartsWith("MessageStreamId-")),
                        It.Is<long>(s => s == 8L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s == "MessageStream"),
                        It.Is<long>(s => s == 6L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s == "MessageStream"),
                        It.Is<long>(s => s == 7L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<long>(
                        It.Is<string>(s => s == "MessageStream"),
                        It.Is<long>(s => s == 8L),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<MessageState>(
                        It.Is<string>(s => s == "MessageStream6"),
                        It.Is<MessageState>(s =>
                            s.Message.AggregateId == command8.AggregateId &&
                            s.Message.AggregateName == command8.AggregateName &&
                            ((DummyAggregateEvent1)s.Message).Name == command8.Name &&
                            s.Metadata.Context.CorrelationId == metadata.Context.CorrelationId),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<MessageState>(
                        It.Is<string>(s => s == "MessageStream7"),
                        It.Is<MessageState>(s =>
                            s.Message.AggregateId == command9.AggregateId &&
                            s.Message.AggregateName == command9.AggregateName &&
                            ((DummyAggregateEvent1)s.Message).Name == command9.Name &&
                            s.Metadata.Context.CorrelationId == metadata.Context.CorrelationId),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<MessageState>(
                        It.Is<string>(s => s == "MessageStream8"),
                        It.Is<MessageState>(s =>
                            s.Message.AggregateId == command10.AggregateId &&
                            s.Message.AggregateName == command10.AggregateName &&
                            ((DummyAggregateEvent1)s.Message).Name == command10.Name &&
                            s.Metadata.Context.CorrelationId == metadata.Context.CorrelationId),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<TaskProcessor>(
                        It.Is<string>(s => s == "TaskProcessor-8"),
                        It.Is<TaskProcessor>(s =>
                            s.Status == TaskProcessorStatus.Completed),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<TaskProcessor>(
                        It.Is<string>(s => s == "TaskProcessor-9"),
                        It.Is<TaskProcessor>(s =>
                            s.Status == TaskProcessorStatus.Completed),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SetStateAsync<TaskProcessor>(
                        It.Is<string>(s => s == "TaskProcessor-10"),
                        It.Is<TaskProcessor>(s =>
                            s.Status == TaskProcessorStatus.Completed),
                        It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Once);
        actorStateManager
            .Setup(s => s.SaveStateAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable(Times.Exactly(3));
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
        _ = await actor.ProcessNextCommandAsync();
        _ = await actor.ProcessNextCommandAsync();
        Mock.VerifyAll(actorStateManager, commandDispatcher, aggregateFactory, eventBus, notificationBus, commandBus, requestBus);
    }
}