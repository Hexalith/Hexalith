// <copyright file="ActorStateStoreProviderTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.States;

using Dapr.Actors.Runtime;

using FluentAssertions;

using Hexalith.Application.Commands;
using Hexalith.Applications.Commands;
using Hexalith.Infrastructure.DaprRuntime.States;

using Hexalith.UnitTests.Core.Application.Commands;

using Moq;

public class ActorStateStoreProviderTest
{
    [Fact]
    public async Task TryGetStateShouldSucceed()
    {
        DummyCommand1 command = new("Test", 123456);
        Mock<IActorStateManager> actorStateManager = new();
        _ = actorStateManager
            .Setup(p => p.TryGetStateAsync<object>("State", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<object>(
                true,
                command));
        Mock<IDomainCommandDispatcher> dispatcher = new();
        ActorStateStoreProvider storeProvider = new(actorStateManager.Object);
        Hexalith.Commons.Errors.ConditionalValue<object> result = await storeProvider.TryGetStateAsync<object>("State", CancellationToken.None);
        _ = result.HasValue.Should().BeTrue();
        _ = result.Value.Should().BeEquivalentTo(command);
    }
}