// <copyright file="ActorCommandProcessorTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprHandlers;

using System;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Commons.Metadatas;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.PolymorphicSerializations;

using Microsoft.Extensions.Logging;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using Shouldly;

[PolymorphicSerialization]
public partial record TestCommand(string Id, string Value)
{
    public static string DomainName => "Test";

    public string DomainId => DomainName + "-" + Id;
}

/// <summary>
/// Class ActorCommandProcessorTest.
/// </summary>
public class ActorCommandProcessorTest
{
    /// <summary>Defines the test method that submitting a command with a failing actor should wrap exception.</summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task SubmittingACommandWithFailingActorShouldWrapException()
    {
        IActorProxyFactory factory = Substitute.For<IActorProxyFactory>();
        IDomainAggregateActor actor = Substitute.For<IDomainAggregateActor>();

        // Set up the actor to throw when SubmitCommandAsJsonAsync is called
        actor.SubmitCommandAsJsonAsync(Arg.Any<string>())
            .ThrowsAsync(new NullReferenceException("Test exception"));

        factory.CreateActorProxy<IDomainAggregateActor>(Arg.Any<ActorId>(), Arg.Any<string>(), Arg.Any<ActorProxyOptions>())
            .Returns(actor);

        PolymorphicSerializationResolver.TryAddDefaultMapper(new TestCommandMapper());
        DomainActorCommandProcessor processor = new(factory, false, Substitute.For<ILogger<DomainActorCommandProcessor>>());
        TestCommand command = new("123", "Hello");

        // Submit the command
        Func<Task> submit = () => processor.SubmitAsync(
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

        // Verify that the exception is wrapped in InvalidOperationException
        InvalidOperationException exception = await Should.ThrowAsync<InvalidOperationException>(submit);
        exception.InnerException.ShouldBeOfType<NullReferenceException>();
    }
}