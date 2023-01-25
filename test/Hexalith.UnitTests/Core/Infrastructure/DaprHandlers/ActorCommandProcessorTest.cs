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

using Dapr.Actors;
using Dapr.Actors.Client;

using FluentAssertions;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Infrastructure.DaprHandlers;
using Hexalith.UnitTests.Core.Domain;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Class ActorCommandProcessorTest.
/// </summary>
public class ActorCommandProcessorTest
{
    /// <summary>Defines the test method Submitting_a_command_should_succeed.</summary>
    /// <returns>Task.</returns>
    [Fact]
    public async Task Submitting_a_command_should_succeed()
    {
        var factory = new Mock<IActorProxyFactory>();
        var actor = new Mock<ActorProxy>();
        factory.Setup(x => x.Create(It.IsAny<ActorId>(), It.IsAny<string>(), It.IsAny<ActorProxyOptions>()))
            .Returns(actor.Object);
        var processor = new DummyActorsCommandProcessor(factory.Object);
        var command = new DummyCommand1();

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
        // The InvokeMethodAsyn on the actor is not virtual and cannot be mocked to return a Task. The mock object returned task will be null.
        await submit.Should()
            .ThrowAsync<InvalidOperationException>()
            .Where(p => p.InnerException is NullReferenceException);
    }
}

/// <summary>
/// Class DummyActorsCommandProcessor.
/// Implements the <see cref="ActorsCommandProcessor" />.
/// </summary>
/// <seealso cref="ActorsCommandProcessor" />
public class DummyActorsCommandProcessor : ActorsCommandProcessor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DummyActorsCommandProcessor"/> class.
    /// </summary>
    /// <param name="actorProxy">The actor proxy.</param>
    public DummyActorsCommandProcessor(IActorProxyFactory actorProxy)
        : base(actorProxy)
    {
    }

    /// <summary>
    /// Gets the name of the actor method.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>string.</returns>
    protected override string GetActorMethodName(ICommand command) => "TotoAsync";

    /// <summary>
    /// Gets the name of the actor.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>string.</returns>
    protected override string GetActorName(ICommand command) => "Titi";
}