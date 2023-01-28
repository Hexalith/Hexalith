namespace Hexalith.UnitTests.Core.Infrastructure.DaprAggregateActor;

using Dapr.Actors.Runtime;

using FluentAssertions;

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

public class ActorStateStoreProviderTest
{
    [Fact]
    public async Task Try_get_state_should_succeed()
    {
        var command = new DummyCommand1("Test", 123456);
        var actorStateManager = new Mock<IActorStateManager>();
        actorStateManager
            .Setup(p => p.TryGetStateAsync<BaseCommand>("State", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<BaseCommand>(true, command));
        var dispatcher = new Mock<ICommandDispatcher>();
        var storeProvider = new ActorStateStoreProvider(actorStateManager.Object);
        var result = await storeProvider.TryGetStateAsync<BaseCommand>("State", CancellationToken.None);
        result.HasValue.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(command);
    }
}