// <copyright file="ActorEventStoreTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprEventStore;

using Dapr.Actors.Runtime;

using FluentAssertions;

using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprEventStore;

using Moq;

using System.Data;

public class ActorEventStoreTest
{
    private const string _fakeMessageStart = "I am fake event ";
    private const string _fakeValue2Start = "Hello world ";
    private const string _streamName = "Test";
    private const string _streamStateName = "TestStream";

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1069)]
    [InlineData(1, 0)]
    [InlineData(3, 0)]
    [InlineData(5, 0)]
    [InlineData(7, 5842)]
    [InlineData(11, 1025)]
    [InlineData(121, 10)]
    [InlineData(142, 123_456_789)]
    public async Task Add_state_should_persist_snapshot(int events, long version)
    {
        (ActorEventStore<BaseTestEvent> eventStore, Mock<IActorStateManager> mock) = GetEventStoreWithMock(version);
        List<BaseTestEvent> list = GetEventList(events);
        long newVersion = await eventStore.AddAsync(
            list,
            version,
            CancellationToken.None);
        _ = newVersion.Should().Be(version + events);
        if (version == 0)
        {
            mock.Verify(
                m => m.AddStateAsync<long>(
                It.Is<string>(s => s == _streamStateName),
                It.Is<long>(s => s == version + events),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
        else
        {
            mock.Verify(
                m => m.SetStateAsync<long>(
                It.Is<string>(s => s == _streamStateName),
                It.Is<long>(s => s == version + events),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        mock.Verify(
            m => m.AddStateAsync<string>(
            It.Is<string>(s => s.StartsWith(_streamStateName, StringComparison.InvariantCulture)),
            It.Is<string>(s => s.Contains("I am fake", StringComparison.InvariantCulture)),
            It.IsAny<CancellationToken>()),
            Times.Exactly(events));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1069)]
    [InlineData(1, 0)]
    [InlineData(3, 0)]
    [InlineData(5, 0)]
    [InlineData(7, 5842)]
    [InlineData(11, 1025)]
    [InlineData(121, 10)]
    [InlineData(142, 123_456_789)]
    public async Task Add_to_stream_returns_version_plus_event_count(int events, long version)
    {
        ActorEventStore<BaseTestEvent> eventStore = GetEventStore(version);

        long newVersion = await eventStore.AddAsync(
            GetEventList(events),
            version,
            CancellationToken.None);
        _ = newVersion.Should().Be(version + events);
    }

    [Theory]
    [InlineData(0, 0, 101)]
    [InlineData(0, 1069, 0)]
    [InlineData(3, 0, 1)]
    [InlineData(5, 0, 540)]
    [InlineData(7, 5842, 0)]
    [InlineData(11, 1025, 5000)]
    [InlineData(121, 10, 1)]
    [InlineData(142, 123_456_789, 4235)]
    public async Task Add_to_stream_with_wrong_version_throw_dbconcurrencyexception(int events, long version, long badVersion)
    {
        ActorEventStore<BaseTestEvent> eventStore = GetEventStore(version);

        async Task<long> Act()
        {
            return await eventStore.AddAsync(
            GetEventList(events),
            badVersion,
            CancellationToken.None).ConfigureAwait(false);
        }

        DBConcurrencyException exception = await Assert.ThrowsAsync<DBConcurrencyException>(Act);
        _ = exception.Message.Should().NotBeEmpty();
        _ = exception.Message.Should().Contain(version.ToInvariantString());
        _ = exception.Message.Should().Contain(badVersion.ToInvariantString());
    }

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
        BaseTestEvent testEvent = new(id, _fakeMessageStart + id);
        Mock<IActorStateManager> stateManager = new();
        ActorEventStore<BaseTestEvent> store = new(stateManager.Object, "Test");
        string json = store.Serialize(testEvent);
        _ = json.Should().Contain($"\"id\":\"{id}\"");
        _ = json.Should().Contain($"\"message\":\"{_fakeMessageStart + id}\"");
    }

    [Fact]
    public void Check_serialize_child_class_state_succeed()
    {
        const string id = "123";
        BaseTestEvent2 testEvent = new(id, _fakeMessageStart + id, _fakeValue2Start + id);
        Mock<IActorStateManager> stateManager = new();
        ActorEventStore<BaseTestEvent> store = new(stateManager.Object, "Test");
        string json = store.Serialize(testEvent);
        _ = json.Should().Contain($"\"$type\":\"{nameof(BaseTestEvent2)}\"");
        _ = json.Should().Contain($"\"id\":\"{id}\"");
        _ = json.Should().Contain($"\"message\":\"{_fakeMessageStart + id}\"");
        _ = json.Should().Contain($"\"value2\":\"{_fakeValue2Start + id}\"");
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
    public async Task Events_added_to_stream_should_be_persisted()
    {
        const long version = 100;

        (ActorEventStore<BaseTestEvent> eventStore, Mock<IActorStateManager> mock) = GetEventStoreWithMock(version);
        List<BaseTestEvent> list = GetEventList(2);
        long newVersion = await eventStore.AddAsync(
            list,
            version,
            CancellationToken.None);
        _ = newVersion.Should().Be(version + 2L);
        mock.Verify(
            m => m.AddStateAsync<string>(
            It.Is<string>(s => s == _streamStateName + "101"),
            It.Is<string>(s => s.Contains("\"id\":\"1\"", StringComparison.InvariantCultureIgnoreCase)),
            It.IsAny<CancellationToken>()),
            Times.Once);
        mock.Verify(
            m => m.AddStateAsync<string>(
            It.Is<string>(s => s == _streamStateName + "102"),
            It.Is<string>(s =>
                s.Contains("\"id\":\"2\"", StringComparison.InvariantCultureIgnoreCase) &&
                s.Contains($"\"$type\":\"{nameof(BaseTestEvent2)}\"", StringComparison.InvariantCultureIgnoreCase)),
            It.IsAny<CancellationToken>()),
            Times.Once);
        mock.Verify(
            m => m.SetStateAsync<long>(
            It.Is<string>(s => s == _streamStateName),
            It.Is<long>(s => s == version + 2L),
            It.IsAny<CancellationToken>()),
            Times.Once);
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
        ActorEventStore<BaseTestEvent> store = GetEventStore(100);
        long version = await store.GetVersionAsync(CancellationToken.None);
        _ = version.Should().Be(100L);
    }

    private static List<BaseTestEvent> GetEventList(int count)
    {
        List<BaseTestEvent> list = new(count);
        for (int i = 1; i <= count; i++)
        {
            string id = i.ToInvariantString();
            if (i % 2 == 0)
            {
                list.Add(new BaseTestEvent2(id, _fakeMessageStart + $"two {id}", _fakeValue2Start + id));
            }
            else
            {
                list.Add(new BaseTestEvent(id, _fakeMessageStart + $"base {id}"));
            }
        }

        return list;
    }

    private static ActorEventStore<BaseTestEvent> GetEventStore(long version)
    {
        (ActorEventStore<BaseTestEvent> s, Mock<IActorStateManager> _) = GetEventStoreWithMock(version);
        return s;
    }

    private static (ActorEventStore<BaseTestEvent> Store, Mock<IActorStateManager> StateManager) GetEventStoreWithMock(long version)
    {
        Mock<IActorStateManager> mock = new();
        _ = mock
            .Setup(m
            => m.TryGetStateAsync<long>(
                    It.Is<string>(s => s == _streamStateName),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ConditionalValue<long>(hasValue: true, version));
        _ = mock
            .Setup(m
            => m.GetOrAddStateAsync<long>(
                    It.Is<string>(s => s == _streamStateName),
                    It.Is<long>(l => l == 0L),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(version);
        ActorEventStore<BaseTestEvent> eventStore = new(mock.Object, _streamName);
        return (eventStore, mock);
    }
}
