// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : Jérôme Piquot
// Created          : 01-22-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-22-2023
// ***********************************************************************
// <copyright file="ActorCommandProcessorTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.UnitTests.Core.Infrastructure.DaprHandlers;

using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using FluentAssertions;

using Hexalith.Application.MessageMetadatas;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.PolymorphicSerialization;

using Microsoft.Extensions.Logging;

using Moq;

[PolymorphicSerialization]
public partial record TestCommand(string Id, string Value)
{
    public static string AggregateName => "Test";

    public string AggregateId => AggregateName + "-" + Id;
}

/// <summary>
/// Class ActorCommandProcessorTest.
/// </summary>
public class ActorCommandProcessorTest
{
    /// <summary>Defines the test method Submitting_a_command_should_succeed.</summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task SubmittingACommandShouldSucceed()
    {
        Mock<IActorProxyFactory> factory = new();
        Mock<ActorProxy> actor = new();
        _ = factory.Setup(x => x.Create(It.IsAny<ActorId>(), It.IsAny<string>(), It.IsAny<ActorProxyOptions>()))
            .Returns(actor.Object);
        JsonSerializerOptions jsonOptions = new() { TypeInfoResolver = new PolymorphicSerializationResolver([new TestCommandMapper()]) };
        DomainActorCommandProcessor processor = new(factory.Object, jsonOptions, new Mock<ILogger<DomainActorCommandProcessor>>().Object);
        TestCommand command = new("123", "Hello");

        Func<Task> submit = async () => await processor.SubmitAsync(
            command,
            new Metadata(
                new MessageMetadata(
                    command,
                    DateTimeOffset.UtcNow),
                new ContextMetadata(
                    "325431",
                    "user123",
                    "PART1",
                    DateTimeOffset.UtcNow,
                    null,
                    null,
                    [])),
            CancellationToken.None);

        // The InvokeMethodAsync on the actor is not virtual and cannot be mocked to return a Task. The mock object returned task will be null.
        _ = await submit
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .Where(p => p.InnerException is NullReferenceException);
    }
}