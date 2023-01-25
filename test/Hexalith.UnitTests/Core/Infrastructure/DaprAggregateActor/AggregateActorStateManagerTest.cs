namespace Hexalith.UnitTests.Core.Infrastructure.DaprAggregateActor;

using Dapr.Actors.Runtime;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.DaprAggregateActor;
using Hexalith.UnitTests.Core.Domain;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class AggregateActorStateManagerTest
{
    [Fact]
    public async Task Initialize_should_succeed()
    {
        var command = new DummyCommand1("Test", 123456);
        var meta = new Metadata(
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
        var state = new CommandState(
            DateTimeOffset.UtcNow,
            "TEST123456",
            command,
            meta,
            null);
        var actorStateManager = new Mock<IActorStateManager>();
        actorStateManager
            .Setup(p => p.TryGetStateAsync<CommandState>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<CommandState>(true, state));
        var dispatcher = new Mock<ICommandDispatcher>();
        var storeProvider = new ActorStateStoreProvider(actorStateManager.Object);
        var stateManager = new AggregateActorStateManager(
            dispatcher.Object,
            new DateTimeService());
        await stateManager.InitializeAsync(
            storeProvider,
            async (name, data, start, period)
               => await Task.CompletedTask,
            CancellationToken.None);
    }
}