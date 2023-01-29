// <copyright file="ActorStateStoreProviderTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprAggregateActor;

using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using FluentAssertions;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Infrastructure.DaprAggregateActor;
using Hexalith.UnitTests.Core.Application.Commands;

using Moq;

public class ActorStateStoreProviderTest
{
    [Fact]
    public async Task Try_get_state_should_succeed()
    {
        DummyCommand1 command = new("Test", 123456);
        Mock<IActorStateManager> actorStateManager = new();
        _ = actorStateManager
            .Setup(p => p.TryGetStateAsync<BaseCommand>("State", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<BaseCommand>(true, command));
        Mock<ICommandDispatcher> dispatcher = new();
        ActorStateStoreProvider storeProvider = new(actorStateManager.Object);
        Extensions.Common.ConditionalValue<BaseCommand> result = await storeProvider.TryGetStateAsync<BaseCommand>("State", CancellationToken.None);
        _ = result.HasValue.Should().BeTrue();
        _ = result.Value.Should().BeEquivalentTo(command);
    }
}