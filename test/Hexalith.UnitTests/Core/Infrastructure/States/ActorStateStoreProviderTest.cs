// <copyright file="ActorStateStoreProviderTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.States;

using Dapr.Actors.Runtime;

using Hexalith.Infrastructure.DaprRuntime.States;

using Hexalith.UnitTests.Core.Application.Commands;

using NSubstitute;

using Shouldly;

public class ActorStateStoreProviderTest
{
    [Fact]
    public async Task TryGetStateShouldSucceed()
    {
        DummyCommand1 command = new("Test", 123456);
        IActorStateManager actorStateManager = Substitute.For<IActorStateManager>();
        actorStateManager
            .TryGetStateAsync<object>("State", Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<object>(true, command));
        ActorStateStoreProvider storeProvider = new(actorStateManager);
        Hexalith.Commons.Errors.ConditionalValue<object> result = await storeProvider.TryGetStateAsync<object>("State", CancellationToken.None);
        result.HasValue.ShouldBeTrue();
        result.Value.ShouldBeEquivalentTo(command);
    }
}
