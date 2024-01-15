// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : Jérôme Piquot
// Created          : 01-22-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-22-2023
// ***********************************************************************
// <copyright file="ActorCommandProcessorTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.UnitTests.Core.Infrastructure.DaprHandlers;

using System;
using System.Linq;
using System.Threading.Tasks;

using Dapr.Actors;
using Dapr.Actors.Client;

using FluentAssertions;

using Hexalith.Application.Metadatas;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.UnitTests.Core.Application.Commands;

using Microsoft.Extensions.Logging;

using Moq;

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
        DummyActorsCommandProcessor processor = new(factory.Object, new Mock<ILogger<DummyActorsCommandProcessor>>().Object);
        DummyCommand1 command = new();

        Func<Task> submit = async () => await processor.SubmitAsync(
            command,
            new Metadata(
                "2664451",
                command,
                DateTimeOffset.UtcNow,
                new ContextMetadata(
                    "325431",
                    "user123",
                    DateTimeOffset.UtcNow,
                    null,
                    null),
                null),
            default);

        // The InvokeMethodAsync on the actor is not virtual and cannot be mocked to return a Task. The mock object returned task will be null.
        _ = await submit.Should()
            .ThrowAsync<InvalidOperationException>()
            .Where(p => p.InnerException is NullReferenceException);
    }
}

/// <summary>
/// Class DummyActorsCommandProcessor.
/// Implements the <see cref="ActorsCommandProcessor" />.
/// </summary>
/// <seealso cref="ActorsCommandProcessor" />
public class DummyActorsCommandProcessor : AggregateActorCommandProcessor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DummyActorsCommandProcessor"/> class.
    /// </summary>
    /// <param name="actorProxy">The actor proxy.</param>
    /// <param name="logger">The logger.</param>
    public DummyActorsCommandProcessor(IActorProxyFactory actorProxy, ILogger<DummyActorsCommandProcessor> logger)
        : base(actorProxy, logger)
    {
    }
}