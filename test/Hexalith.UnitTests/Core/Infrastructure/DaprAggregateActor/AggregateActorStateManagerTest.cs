// <copyright file="AggregateActorStateManagerTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprAggregateActor;

using Dapr.Actors.Runtime;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.DaprAggregateActor;
using Hexalith.UnitTests.Core.Application.Commands;

using Moq;

using System;
using System.Threading.Tasks;

public class AggregateActorStateManagerTest
{
    [Fact]
    public async Task Initialize_should_succeed()
    {
        DummyCommand1 command = new("Test", 123456);
        Metadata meta = new(
                "TEST123456",
                command,
                DateTimeOffset.UtcNow,
                new ContextMetadata(
                    "COR123",
                    "TestUser",
                    null,
                    null,
                    null),
                null);
        CommandState state = new(
            DateTimeOffset.UtcNow,
            "TEST123456",
            command,
            meta,
            null);
        Mock<IActorStateManager> actorStateManager = new();
        _ = actorStateManager
            .Setup(p => p.TryGetStateAsync<CommandState>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<CommandState>(true, state));
        Mock<ICommandDispatcher> dispatcher = new();
        ActorStateStoreProvider storeProvider = new(actorStateManager.Object);
        AggregateActorStateManager stateManager = new(
            dispatcher.Object,
            new DateTimeService());
        await stateManager.InitializeAsync(
            storeProvider,
            async (name, data, start, period)
               => await Task.CompletedTask,
            CancellationToken.None);
    }
}