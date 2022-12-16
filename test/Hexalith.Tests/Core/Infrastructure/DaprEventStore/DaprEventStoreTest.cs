// <copyright file="DaprEventStoreTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Tests.Core.Infrastructure.DaprEventStore;

using Dapr.Actors.Runtime;

using FluentAssertions;

using Hexalith.Infrastructure.DaprEventStore;

using Moq;

public class DaprEventStoreTest
{
    [Fact]
    public void Check_event_state_name()
    {
        const string streamName = "Test";
        Mock<IActorStateManager> stateManager = new();
        ActorEventStore<BaseTestEvent> store = new(stateManager.Object, "Test");
        _ = store.StreamName.Should().Be(streamName);
        _ = store.GetEventStateName(101).Should().Be(streamName + "Stream101");
    }

    [Fact]
    public void Check_stream_state_name()
    {
        const string streamName = "Test";
        Mock<IActorStateManager> stateManager = new();
        ActorEventStore<BaseTestEvent> store = new(stateManager.Object, "Test");
        _ = store.StreamName.Should().Be(streamName);
        _ = store.GetStreamStateName().Should().Be(streamName + "Stream");
    }
}
