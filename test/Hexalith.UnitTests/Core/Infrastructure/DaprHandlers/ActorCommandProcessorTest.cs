// <copyright file="ActorCommandProcessorTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprHandlers;

using System;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using FluentAssertions;

using Hexalith.Commons.Metadatas;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.PolymorphicSerializations;

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
        PolymorphicSerializationResolver.TryAddDefaultMapper(new TestCommandMapper());
        DomainActorCommandProcessor processor = new(factory.Object, false, new Mock<ILogger<DomainActorCommandProcessor>>().Object);
        TestCommand command = new("123", "Hello");

        Func<Task> submit = async () => await processor.SubmitAsync(
            command,
            new Metadata(
                    command.CreateMessageMetadata(DateTimeOffset.UtcNow),
                new ContextMetadata(
                    "325431",
                    "user123",
                    "PART1",
                    DateTimeOffset.UtcNow,
                    TimeSpan.FromMinutes(5),
                    null,
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