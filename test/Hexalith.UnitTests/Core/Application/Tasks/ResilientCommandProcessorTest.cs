// <copyright file="ResilientCommandProcessorTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Tasks;

using System.Threading.Tasks;

using FluentAssertions;

using Hexalith.Application.Commands;
using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Messages;
using Hexalith.UnitTests.Core.Application.Commands;
using Hexalith.UnitTests.Core.Domain.Events;

using Microsoft.Extensions.Logging;

using Moq;

public class ResilientCommandProcessorTest
{
    [Fact]
    public async Task CompletedStateShouldBeValid()
    {
        Mock<ICommandDispatcher> dispatcher = new();
        _ = dispatcher
            .Setup(p => p.DoAsync(It.IsAny<ICommand>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync([new DummyEvent1("My test response", 123)]);
        MemoryStateProvider stateProvider = new();
        ResilientCommandProcessor processor = new(
            ResiliencyPolicy.None,
            dispatcher.Object,
            stateProvider,
            new Mock<ILogger<ResilientCommandProcessor>>().Object);
        const string key = "test1";
        string stateName = nameof(TaskProcessor) + key;
        DummyCommand1 command = new("My test 1", 1);
        (TaskProcessor taskProcessor, IEnumerable<BaseMessage> events) = await processor.ProcessAsync(key, command, null, CancellationToken.None);
        _ = taskProcessor.Should().NotBeNull();
        _ = taskProcessor.Ended.Should().BeTrue();
        _ = taskProcessor.Status.Should().Be(TaskProcessorStatus.Completed);
        _ = taskProcessor.Failure.Should().BeNull();

        _ = events.Should().HaveCount(1);
        _ = events.First().Should().BeOfType<DummyEvent1>();
        _ = stateProvider.State.Should().BeEmpty();
        _ = stateProvider.UncommittedState.Should().NotBeEmpty();
        _ = stateProvider.UncommittedState.Should().ContainKey(stateName);
        _ = stateProvider.UncommittedState[stateName].Should().BeOfType<TaskProcessor>();
    }

    [Fact]
    public async Task TaskStatusShouldBeCancelledWhenCommandDispatcherFailsWithoutResiliency()
    {
        Mock<ICommandDispatcher> dispatcher = new();
        _ = dispatcher
            .Setup(p => p.DoAsync(It.IsAny<ICommand>(), null, It.IsAny<CancellationToken>()))
            .Returns(Task.FromException<IEnumerable<BaseMessage>>(new InvalidOperationException("Command execution failed.")));
        MemoryStateProvider stateProvider = new();
        ResilientCommandProcessor processor = new(
            ResiliencyPolicy.None,
            dispatcher.Object,
            stateProvider,
            new Mock<ILogger<ResilientCommandProcessor>>().Object);

        const string key = "test1";
        string stateName = nameof(TaskProcessor) + key;
        DummyCommand1 command = new("My test 1", 1);
        (TaskProcessor taskProcessor, IEnumerable<BaseMessage> events) = await processor.ProcessAsync(key, command, null, CancellationToken.None);
        _ = taskProcessor.Should().NotBeNull();
        _ = taskProcessor.Status.Should().Be(TaskProcessorStatus.Canceled);
        _ = taskProcessor.History.CompletedDate.Should().BeNull();
        _ = taskProcessor.History.ProcessingStartDate.Should().NotBeNull();
        _ = taskProcessor.History.SuspendedDate.Should().BeNull();
        _ = taskProcessor.History.CanceledDate.Should().NotBeNull();
        _ = taskProcessor.Failure.Should().NotBeNull();
        _ = events.Should().HaveCount(1);
        _ = events.First().Should().BeOfType<CommandProcessingFailed>();
        _ = stateProvider.State.Should().BeEmpty();
        _ = stateProvider.UncommittedState.Should().NotBeEmpty();
        _ = stateProvider.UncommittedState.Should().ContainKey(stateName);
        _ = stateProvider.UncommittedState[stateName].Should().BeOfType<TaskProcessor>();
        taskProcessor = stateProvider.UncommittedState[stateName] as TaskProcessor;
        _ = taskProcessor.Should().NotBeNull();
    }

    [Fact]
    public async Task TaskStatusShouldBeSuspendedWhenCommandDispatcherFailsWithResiliency()
    {
        Mock<ICommandDispatcher> dispatcher = new();
        _ = dispatcher
            .Setup(p => p.DoAsync(It.IsAny<ICommand>(), null, It.IsAny<CancellationToken>()))
            .Returns(Task.FromException<IEnumerable<BaseMessage>>(new InvalidOperationException("Command execution failed.")));
        MemoryStateProvider stateProvider = new();
        ResilientCommandProcessor processor = new(
            new(
                10,
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromDays(100),
                false),
            dispatcher.Object,
            stateProvider,
            new Mock<ILogger<ResilientCommandProcessor>>().Object);

        const string key = "test1";
        string stateName = nameof(TaskProcessor) + key;
        DummyCommand1 command = new("My test 1", 1);
        (TaskProcessor retry, IEnumerable<BaseMessage> events) = await processor.ProcessAsync(key, command, null, CancellationToken.None);
        _ = retry.Should().NotBeNull();
        _ = events.Should().HaveCount(1);
        _ = events.First().Should().BeOfType<CommandProcessingFailed>();
        _ = stateProvider.State.Should().BeEmpty();
        _ = stateProvider.UncommittedState.Should().NotBeEmpty();
        _ = stateProvider.UncommittedState.Should().ContainKey(stateName);
        _ = stateProvider.UncommittedState[stateName].Should().BeOfType<TaskProcessor>();
        TaskProcessor taskProcessor = stateProvider.UncommittedState[stateName] as TaskProcessor;
        _ = taskProcessor.Status.Should().Be(TaskProcessorStatus.Suspended);
        _ = taskProcessor.History.CompletedDate.Should().BeNull();
        _ = taskProcessor.History.ProcessingStartDate.Should().NotBeNull();
        _ = taskProcessor.History.SuspendedDate.Should().NotBeNull();
        _ = taskProcessor.History.CanceledDate.Should().BeNull();
        _ = taskProcessor.Failure.Should().NotBeNull();
    }
}