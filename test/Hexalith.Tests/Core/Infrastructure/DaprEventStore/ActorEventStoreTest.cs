// <copyright file="ActorEventStoreTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Tests.Core.Infrastructure.DaprEventStore;

using Dapr.Actors.Runtime;

using FluentAssertions;

using Hexalith.Infrastructure.DaprEventStore;

using Moq;

public class ActorEventStoreTest
{
    [Fact]
    public void Check_deserialize_base_class_state_succeed()
    {
        const string id = "123";
        const string json = $$"""{"id":"{{id}}"}""";
        Mock<IActorStateManager> stateManager = new();
        ActorEventStore<BaseTestEvent> store = new(stateManager.Object, "Test");
        BaseTestEvent testEvent = store.Deserialize(json);
        _ = testEvent.Id.Should().Be(id);
    }

    [Fact]
    public void Check_deserialize_child_class_state_succeed()
    {
        const string id = "123";
        const string value2 = "456";
        const string json = $$"""{"$type":"{{nameof(BaseTestEvent2)}}","id":"{{id}}","value2":"{{value2}}"}""";
        Mock<IActorStateManager> stateManager = new();
        ActorEventStore<BaseTestEvent> store = new(stateManager.Object, "Test");
        BaseTestEvent testEvent = store.Deserialize(json);
        _ = testEvent.Should().BeOfType<BaseTestEvent2>();
        BaseTestEvent2 testEvent2 = (BaseTestEvent2)testEvent;
        _ = testEvent2.Id.Should().Be(id);
        _ = testEvent2.Value2.Should().Be(value2);
    }

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
    public void Check_serialize_base_class_state_succeed()
    {
        const string id = "123";
        BaseTestEvent testEvent = new(id);
        Mock<IActorStateManager> stateManager = new();
        ActorEventStore<BaseTestEvent> store = new(stateManager.Object, "Test");
        string json = store.Serialize(testEvent);
        _ = json.Should().Contain($"\"id\":\"{id}\"");
    }

    [Fact]
    public void Check_serialize_child_class_state_succeed()
    {
        const string id = "123";
        const string value2 = "456";
        BaseTestEvent2 testEvent = new(id, value2);
        Mock<IActorStateManager> stateManager = new();
        ActorEventStore<BaseTestEvent> store = new(stateManager.Object, "Test");
        string json = store.Serialize(testEvent);
        _ = json.Should().Contain($"\"$type\":\"{nameof(BaseTestEvent2)}\"");
        _ = json.Should().Contain($"\"id\":\"{id}\"");
        _ = json.Should().Contain($"\"value2\":\"{value2}\"");
    }

    [Fact]
    public void Check_stream_state_name()
    {
        const string streamName = "Test";
        Mock<IActorStateManager> stateManager = new();
        ActorEventStore<BaseTestEvent> store = new(stateManager.Object, streamName);
        _ = store.StreamName.Should().Be(streamName);
        _ = store.GetStreamStateName().Should().Be(streamName + "Stream");
    }

    [Fact]
    public async Task Empty_stream_version_should_be_zero()
    {
        Mock<IActorStateManager> stateManager = new();
        _ = stateManager.Setup(m
            => m.TryGetStateAsync<long>(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<long>(hasValue: false, 0L));
        ActorEventStore<BaseTestEvent> store = new(stateManager.Object, "Test");
        long version = await store.GetVersionAsync(CancellationToken.None);
        _ = version.Should().Be(0L);
    }

    [Fact]
    public async Task Get_stream_should_return_event_value()
    {
        const string id = "123";
        const string value2 = "456";
        const string json = $$"""{"$type":"{{nameof(BaseTestEvent2)}}","id":"{{id}}","value2":"{{value2}}"}""";
        Mock<IActorStateManager> stateManager = new();
        stateManager.Setup(m
            => m.TryGetStateAsync<string>(
                   "TestStream123",
                   It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<string>(hasValue: true, json))
            .Verifiable();

        ActorEventStore<BaseTestEvent> store = new(stateManager.Object, "Test");
        IEnumerable<BaseTestEvent> events = await store.GetAsync(123L, 123L, CancellationToken.None);
        stateManager.VerifyAll();
        _ = events.Should().HaveCount(1);
        BaseTestEvent testEvent = events.First();
        _ = testEvent.Should().BeOfType<BaseTestEvent2>();
        BaseTestEvent2 testEvent2 = (BaseTestEvent2)testEvent;
        _ = testEvent2.Id.Should().Be(id);
        _ = testEvent2.Value2.Should().Be(value2);
    }

    [Fact]
    public async Task Get_stream_version_should_return_correct_value()
    {
        const long streamVersion = 123L;
        Mock<IActorStateManager> stateManager = new();
        _ = stateManager.Setup(m
            => m.TryGetStateAsync<long>(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<long>(hasValue: true, streamVersion));
        ActorEventStore<BaseTestEvent> store = new(stateManager.Object, "Test");
        long version = await store.GetVersionAsync(CancellationToken.None);
        _ = version.Should().Be(streamVersion);
    }
}
