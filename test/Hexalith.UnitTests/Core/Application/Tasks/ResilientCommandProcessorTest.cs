// <copyright file="ResilientCommandProcessorTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Tasks;

using System.Threading.Tasks;

using Hexalith.Application.States;
using Hexalith.Application.Tasks;
using Hexalith.Applications.Commands;
using Hexalith.Commons.Metadatas;
using Hexalith.UnitTests.Core.Application.Commands;
using Hexalith.UnitTests.Core.Domain.Events;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using Shouldly;

public class ResilientCommandProcessorTest
{
    [Fact]
    public async Task CompletedStateShouldBeValid()
    {
        IDomainCommandDispatcher dispatcher = Substitute.For<IDomainCommandDispatcher>();
        dispatcher
            .DoAsync(Arg.Any<object>(), Arg.Any<Metadata>(), null, Arg.Any<CancellationToken>())
            .Returns(new ExecuteCommandResult(null, [new DummyEvent1("My test response", 123)], [], false));
        MemoryStateProvider stateProvider = new();
        ResilientCommandProcessor processor = new(
            ResiliencyPolicy.None,
            dispatcher,
            stateProvider,
            TimeProvider.System,
            Substitute.For<ILogger<ResilientCommandProcessor>>());
        const string key = "test1";
        string stateName = nameof(TaskProcessor) + key;
        DummyCommand1 command = new("My test 1", 1);
        (TaskProcessor taskProcessor, ExecuteCommandResult result) = await processor.ProcessAsync(key, command, command.CreateMetadata(), null, CancellationToken.None);
        taskProcessor.ShouldNotBeNull();
        taskProcessor.Ended.ShouldBeTrue();
        taskProcessor.Status.ShouldBe(TaskProcessorStatus.Completed);
        taskProcessor.Failure.ShouldBeNull();

        result.SourceEvents.Count().ShouldBe(1);
        result.SourceEvents.First().ShouldBeOfType<DummyEvent1>();
        stateProvider.State.ShouldBeEmpty();
        stateProvider.UncommittedState.ShouldNotBeEmpty();
        stateProvider.UncommittedState.ShouldContainKey(stateName);
        stateProvider.UncommittedState[stateName].ShouldBeOfType<TaskProcessor>();
    }

    [Fact]
    public async Task TaskStatusShouldBeCancelledWhenCommandDispatcherFailsWithoutResiliency()
    {
        IDomainCommandDispatcher dispatcher = Substitute.For<IDomainCommandDispatcher>();
        dispatcher
            .DoAsync(Arg.Any<object>(), Arg.Any<Metadata>(), null, Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("Command execution failed."));
        MemoryStateProvider stateProvider = new();
        ResilientCommandProcessor processor = new(
            ResiliencyPolicy.None,
            dispatcher,
            stateProvider,
            TimeProvider.System,
            Substitute.For<ILogger<ResilientCommandProcessor>>());

        const string key = "test1";
        string stateName = nameof(TaskProcessor) + key;
        DummyCommand1 command = new("My test 1", 1);
        (TaskProcessor taskProcessor, ExecuteCommandResult result) = await processor.ProcessAsync(key, command, command.CreateMetadata(), null, CancellationToken.None);
        taskProcessor.ShouldNotBeNull();
        taskProcessor.Status.ShouldBe(TaskProcessorStatus.Canceled);
        taskProcessor.History.CompletedDate.ShouldBeNull();
        taskProcessor.History.ProcessingStartDate.ShouldNotBeNull();
        taskProcessor.History.SuspendedDate.ShouldBeNull();
        taskProcessor.History.CanceledDate.ShouldNotBeNull();
        taskProcessor.Failure.ShouldNotBeNull();
        stateProvider.State.ShouldBeEmpty();
        stateProvider.UncommittedState.ShouldNotBeEmpty();
        stateProvider.UncommittedState.ShouldContainKey(stateName);
        stateProvider.UncommittedState[stateName].ShouldBeOfType<TaskProcessor>();
        taskProcessor = stateProvider.UncommittedState[stateName] as TaskProcessor;
        taskProcessor.ShouldNotBeNull();
    }

    [Fact]
    public async Task TaskStatusShouldBeSuspendedWhenCommandDispatcherFailsWithResiliency()
    {
        IDomainCommandDispatcher dispatcher = Substitute.For<IDomainCommandDispatcher>();
        dispatcher
            .DoAsync(Arg.Any<object>(), Arg.Any<Metadata>(), null, Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("Command execution failed."));
        MemoryStateProvider stateProvider = new();
        ResilientCommandProcessor processor = new(
            new(
                10,
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromDays(100),
                false),
            dispatcher,
            stateProvider,
            TimeProvider.System,
            Substitute.For<ILogger<ResilientCommandProcessor>>());

        const string key = "test1";
        string stateName = nameof(TaskProcessor) + key;
        DummyCommand1 command = new("My test 1", 1);
        (TaskProcessor retry, ExecuteCommandResult result) = await processor.ProcessAsync(key, command, command.CreateMetadata(), null, CancellationToken.None);
        retry.ShouldNotBeNull();
        stateProvider.State.ShouldBeEmpty();
        stateProvider.UncommittedState.ShouldNotBeEmpty();
        stateProvider.UncommittedState.ShouldContainKey(stateName);
        stateProvider.UncommittedState[stateName].ShouldBeOfType<TaskProcessor>();
        TaskProcessor taskProcessor = stateProvider.UncommittedState[stateName] as TaskProcessor;
        taskProcessor.Status.ShouldBe(TaskProcessorStatus.Suspended);
        taskProcessor.History.CompletedDate.ShouldBeNull();
        taskProcessor.History.ProcessingStartDate.ShouldNotBeNull();
        taskProcessor.History.SuspendedDate.ShouldNotBeNull();
        taskProcessor.History.CanceledDate.ShouldBeNull();
        taskProcessor.Failure.ShouldNotBeNull();
    }
}
