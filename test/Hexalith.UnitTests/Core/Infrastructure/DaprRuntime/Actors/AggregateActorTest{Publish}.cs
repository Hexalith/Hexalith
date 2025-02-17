﻿// <copyright file="AggregateActorTest{Publish}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

// namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

// using Dapr.Actors;
// using Dapr.Actors.Runtime;

// using FluentAssertions;

// using Hexalith.Application.Aggregates;
// using Hexalith.Application.Commands;
// using Hexalith.Application.Events;
// using Hexalith.Application.Metadatas;
// using Hexalith.Application.Requests;
// using Hexalith.Application.States;
// using Hexalith.Application.Tasks;
// using Hexalith.Infrastructure.DaprRuntime;
// using Hexalith.Infrastructure.DaprRuntime.Actors;
// using Hexalith.Infrastructure.DaprRuntime.Helpers;

// using Moq;

///// <summary>
///// Class AggregateActorTest.
///// </summary>
// public partial class AggregateActorTest
// {
//    [Fact]
//    public async Task PublishLastMessageShouldSendMessageAndRemoveCallback()
//    {
//        DummyAggregateEvent1 message = new("123456", "Hello from test");
//        AggregateActorState currentState = new()
//        {
//            CommandCount = 2,
//            EventSourceCount = 2,
//            LastCommandProcessed = 2,
//            LastMessagePublished = 1,
//            MessageCount = 2,
//            ProcessReminderDueTime = null,
//        };
//        DummyTimerManager timerManager = new();
//        ActorHost host = ActorHost.CreateForTest(
//            typeof(DomainAggregateActor),
//            message.AggregateName.ToAggregateActorName(),
//            new ActorTestOptions
//            {
//                ActorId = new ActorId(message.AggregateId),
//                TimerManager = timerManager,
//            });
//        Metadata metadata = CreateMetadata(message);
//        Mock<IDomainCommandDispatcher> commandDispatcher = new(MockBehavior.Strict);
//        Mock<IDomainAggregateFactory> aggregateFactory = new(MockBehavior.Strict);
//        Mock<IEventBus> eventBus = new(MockBehavior.Strict);
//        Mock<ICommandBus> commandBus = new(MockBehavior.Strict);
//        Mock<IRequestBus> requestBus = new(MockBehavior.Strict);
//        Mock<IActorStateManager> actorStateManager = new(MockBehavior.Strict);
//        Mock<IResiliencyPolicyProvider> resiliencyPolicyProvider = new(MockBehavior.Strict);
//        actorStateManager
//            .Setup(s => s.TryGetStateAsync<AggregateActorState>(
//                        It.Is<string>(t => t == ActorConstants.AggregateStateStoreName),
//                        It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<AggregateActorState>(true, currentState))
//            .Verifiable(Times.Once);
//        actorStateManager
//            .Setup(s => s.TryGetStateAsync<MessageState>(
//                        It.Is<string>(s => s == "MessageStream2"),
//                        It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<MessageState>(
//                true,
//                new MessageState(message, metadata)))
//            .Verifiable(Times.Once);

// eventBus
//            .Setup(s => s.PublishAsync(
//                It.IsAny<object>(),
//                It.Is<Metadata>(e =>
//                    e.Message.Aggregate.Id == message.AggregateId &&
//                    e.Message.Id == metadata.Message.Id),
//                It.IsAny<CancellationToken>()))
//            .Returns(Task.CompletedTask)
//            .Verifiable(Times.Once);

// actorStateManager
//            .Setup(s => s.SetStateAsync<AggregateActorState>(
//                        It.Is<string>(s => s == ActorConstants.AggregateStateStoreName),
//                        It.Is<AggregateActorState>(s => s.MessageCount == 2 && s.LastMessagePublished == 2 && s.ProcessReminderDueTime == null),
//                        It.IsAny<CancellationToken>()))
//            .Returns(Task.CompletedTask)
//            .Verifiable(Times.Once);
//        actorStateManager
//            .Setup(s => s.SaveStateAsync(It.IsAny<CancellationToken>()))
//            .Returns(Task.CompletedTask)
//            .Verifiable(Times.Once);
//        DomainAggregateActor actor = new(
//            host,
//            commandDispatcher.Object,
//            aggregateFactory.Object,
//            TimeProvider.System,
//            eventBus.Object,
//            commandBus.Object,
//            requestBus.Object,
//            resiliencyPolicyProvider.Object,
//            actorStateManager.Object);
//        bool result = await actor.PublishNextMessageAsync();
//        _ = result.Should().BeFalse();
//        _ = timerManager.Reminders.Should().BeEmpty();
//        _ = timerManager.Timers.Should().BeEmpty();
//        Mock.VerifyAll(actorStateManager, commandDispatcher, aggregateFactory, eventBus, commandBus, requestBus);
//    }

// [Fact]
//    public async Task PublishNextMessageFailedShouldAddMinuteCallback()
//    {
//        DummyAggregateEvent1 message = new("123456", "Hello from test");
//        AggregateActorState currentState = new()
//        {
//            CommandCount = 2,
//            EventSourceCount = 2,
//            LastCommandProcessed = 2,
//            LastMessagePublished = 1,
//            MessageCount = 3,
//            ProcessReminderDueTime = null,
//        };
//        DummyTimerManager timerManager = new();
//        ActorHost host = ActorHost.CreateForTest(
//            typeof(DomainAggregateActor),
//            message.AggregateName.ToAggregateActorName(),
//            new ActorTestOptions
//            {
//                ActorId = new ActorId(message.AggregateId),
//                TimerManager = timerManager,
//            });
//        Metadata metadata = CreateMetadata(message);
//        Mock<IDomainCommandDispatcher> commandDispatcher = new(MockBehavior.Strict);
//        Mock<IDomainAggregateFactory> aggregateFactory = new(MockBehavior.Strict);
//        Mock<IEventBus> eventBus = new(MockBehavior.Strict);
//        Mock<ICommandBus> commandBus = new(MockBehavior.Strict);
//        Mock<IRequestBus> requestBus = new(MockBehavior.Strict);
//        Mock<IActorStateManager> actorStateManager = new(MockBehavior.Strict);
//        Mock<IResiliencyPolicyProvider> resiliencyPolicyProvider = new(MockBehavior.Strict);
//        actorStateManager
//            .Setup(s => s.TryGetStateAsync<AggregateActorState>(
//                        It.Is<string>(t => t == ActorConstants.AggregateStateStoreName),
//                        It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<AggregateActorState>(true, currentState))
//            .Verifiable(Times.Once);
//        actorStateManager
//            .Setup(s => s.TryGetStateAsync<MessageState>(
//                        It.Is<string>(s => s == "MessageStream2"),
//                        It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<MessageState>(
//                true,
//                new MessageState(message, metadata)))
//            .Verifiable(Times.Once);

// eventBus
//            .Setup(s => s.PublishAsync(
//                It.IsAny<object>(),
//                It.Is<Metadata>(e =>
//                    e.Message.Aggregate.Id == message.AggregateId &&
//                    e.Message.Id == metadata.Message.Id),
//                It.IsAny<CancellationToken>()))
//            .ThrowsAsync(new InvalidOperationException("Dummy error"))
//            .Verifiable(Times.Once);

// actorStateManager
//            .Setup(s => s.SetStateAsync(
//                        It.Is<string>(s => s == ActorConstants.AggregateStateStoreName),
//                        It.Is<AggregateActorState>(s =>
//                            s.MessageCount == 3 &&
//                            s.LastMessagePublished == 1 &&
//                            s.PublishFailed &&
//                            s.PublishReminderDueTime != null),
//                        It.IsAny<CancellationToken>()))
//            .Returns(Task.CompletedTask)
//            .Verifiable(Times.Once);
//        actorStateManager
//            .Setup(s => s.SaveStateAsync(It.IsAny<CancellationToken>()))
//            .Returns(Task.CompletedTask)
//            .Verifiable(Times.Once);
//        DomainAggregateActor actor = new(
//            host,
//            commandDispatcher.Object,
//            aggregateFactory.Object,
//            TimeProvider.System,
//            eventBus.Object,
//            commandBus.Object,
//            requestBus.Object,
//            resiliencyPolicyProvider.Object,
//            actorStateManager.Object);
//        bool result = await actor.PublishNextMessageAsync();
//        _ = result.Should().BeTrue();
//        _ = timerManager.Reminders.Should().HaveCount(1);
//        _ = timerManager.Reminders[ActorConstants.PublishReminderName].Period.Should().Be(TimeSpan.FromMinutes(1));
//        _ = timerManager.Timers.Should().HaveCount(1);
//        _ = timerManager.Timers.First().Key.Should().Be(ActorConstants.PublishTimerName);
//        Mock.VerifyAll(actorStateManager, commandDispatcher, aggregateFactory, eventBus, commandBus, requestBus);
//    }

// [Fact]
//    public async Task PublishNextMessageShouldSendMessageAndAddCallback()
//    {
//        DummyAggregateEvent1 message = new("123456", "Hello from test");
//        AggregateActorState currentState = new()
//        {
//            CommandCount = 2,
//            EventSourceCount = 2,
//            LastCommandProcessed = 2,
//            LastMessagePublished = 1,
//            MessageCount = 3,
//            ProcessReminderDueTime = null,
//        };
//        DummyTimerManager timerManager = new();
//        ActorHost host = ActorHost.CreateForTest(
//            typeof(DomainAggregateActor),
//            message.AggregateName.ToAggregateActorName(),
//            new ActorTestOptions
//            {
//                ActorId = new ActorId(message.AggregateId),
//                TimerManager = timerManager,
//            });
//        Metadata metadata = CreateMetadata(message);
//        Mock<IDomainCommandDispatcher> commandDispatcher = new(MockBehavior.Strict);
//        Mock<IDomainAggregateFactory> aggregateFactory = new(MockBehavior.Strict);
//        Mock<IEventBus> eventBus = new(MockBehavior.Strict);
//        Mock<ICommandBus> commandBus = new(MockBehavior.Strict);
//        Mock<IRequestBus> requestBus = new(MockBehavior.Strict);
//        Mock<IActorStateManager> actorStateManager = new(MockBehavior.Strict);
//        Mock<IResiliencyPolicyProvider> resiliencyPolicyProvider = new(MockBehavior.Strict);
//        actorStateManager
//            .Setup(s => s.TryGetStateAsync<AggregateActorState>(
//                        It.Is<string>(t => t == ActorConstants.AggregateStateStoreName),
//                        It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<AggregateActorState>(true, currentState))
//            .Verifiable(Times.Once);
//        actorStateManager
//            .Setup(s => s.TryGetStateAsync<MessageState>(
//                        It.Is<string>(s => s == "MessageStream2"),
//                        It.IsAny<CancellationToken>()))
//            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<MessageState>(true, new MessageState(message, metadata)))
//            .Verifiable(Times.Once);

// eventBus
//            .Setup(s => s.PublishAsync(
//                It.IsAny<object>(),
//                It.Is<Metadata>(e =>
//                    e.Message.Aggregate.Id == message.AggregateId &&
//                    e.Message.Id == metadata.Message.Id),
//                It.IsAny<CancellationToken>()))
//            .Returns(Task.CompletedTask)
//            .Verifiable(Times.Once);

// actorStateManager
//            .Setup(s => s.SetStateAsync(
//                        It.Is<string>(s => s == ActorConstants.AggregateStateStoreName),
//                        It.Is<AggregateActorState>(s =>
//                            s.MessageCount == 3 &&
//                            s.LastMessagePublished == 2 &&
//                            s.PublishReminderDueTime != null),
//                        It.IsAny<CancellationToken>()))
//            .Returns(Task.CompletedTask)
//            .Verifiable(Times.Once);
//        actorStateManager
//            .Setup(s => s.SaveStateAsync(It.IsAny<CancellationToken>()))
//            .Returns(Task.CompletedTask)
//            .Verifiable(Times.Once);
//        DomainAggregateActor actor = new(
//            host,
//            commandDispatcher.Object,
//            aggregateFactory.Object,
//            TimeProvider.System,
//            eventBus.Object,
//            commandBus.Object,
//            requestBus.Object,
//            resiliencyPolicyProvider.Object,
//            actorStateManager.Object);
//        bool result = await actor.PublishNextMessageAsync();
//        _ = result.Should().BeTrue();
//        _ = timerManager.Reminders.Should().HaveCount(1);
//        _ = timerManager.Reminders[ActorConstants.PublishReminderName].Period.Should().Be(TimeSpan.FromMinutes(1));
//        _ = timerManager.Timers.Should().HaveCount(1);
//        _ = timerManager.Timers.First().Key.Should().Be(ActorConstants.PublishTimerName);
//        Mock.VerifyAll(actorStateManager, commandDispatcher, aggregateFactory, eventBus, commandBus, requestBus);
//    }
// }