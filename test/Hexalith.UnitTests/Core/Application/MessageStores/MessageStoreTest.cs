// <copyright file="MessageStoreTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.MessageStores
{
    using System.Data;

    using FluentAssertions;

    using Hexalith.Application.States;
    using Hexalith.Application.States;
    using Hexalith.Application.StreamStores;
    using Hexalith.Extensions.Common;
    using Hexalith.Extensions.Helpers;
    using Hexalith.Infrastructure.Serialization.States;
    using Hexalith.UnitTests.Core.Application.Commands;

    using Moq;

    public class MessageStoreTest
    {
        private const string _fakeMessageStart = "I am fake event ";
        private const string _fakeValue2Start = "Hello world ";
        private const string _stateName = "TestStream";
        private const string _streamItemId = "TestStreamId-";
        private const string _streamName = "Test";

        [Fact]
        public async Task Add_and_get_from_serialized_command_state_should_return_same()
        {
            StringStateProvider provider = new();
            MessageStore<CommandState> store = new(provider, _streamName);
            DummyCommand1 command1 = new("5354323", 123);
            DummyCommand2 command2 = new("AAAABBCC", 960);
            CommandState original1 = new(DateTimeOffset.UtcNow, command1, command1.CreateMetadata());
            CommandState original2 = new(DateTimeOffset.UtcNow, command2, command2.CreateMetadata());
            _ = await store.AddAsync(original1.IntoArray(), 0, CancellationToken.None);
            _ = await store.AddAsync(original2.IntoArray(), 1, CancellationToken.None);
            CommandState result1 = await store.GetAsync(1, CancellationToken.None);
            CommandState result2 = await store.GetAsync(2, CancellationToken.None);
            _ = result1.Should().BeEquivalentTo(original1);
            _ = result2.Should().BeEquivalentTo(original2);
        }

        [Fact]
        public async Task Add_and_get_from_serialized_state_should_return_same()
        {
            StringStateProvider provider = new();
            MessageStore<DummyState> store = new(provider, _streamName);
            DummyState state = new("5354323", 123, "one two three");
            _ = await store.AddAsync(state.IntoArray(), 0, CancellationToken.None);
            DummyState result = await store.GetAsync(1, CancellationToken.None);
            _ = result.Should().BeEquivalentTo(state);
        }

        [Fact]
        public async Task Add_state_should_be_persisted()
        {
            MemoryStateProvider provider = new();
            MessageStore<DummyState> store = new(provider, _streamName);
            DummyState state = new("5354323", 123, "one two three");
            _ = await store.AddAsync(state.IntoArray(), 0, CancellationToken.None);
            _ = provider.UncommittedState.Should().HaveCount(3);
            _ = provider.UncommittedState[_stateName].Should().Be(1L);
            _ = provider.UncommittedState[_stateName + "1"].Should().Be(state.IdempotencyId);
            _ = provider.UncommittedState[_streamItemId + state.IdempotencyId].Should().Be(state);
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
        public async Task Add_state_should_persist_new_version(int events, long version)
        {
            MemoryStateProvider provider = new(new Dictionary<string, object> { { _stateName, version } });
            MessageStore<BaseTestEvent> eventStore = new(provider, _streamName);
            List<BaseTestEvent> list = GetEventList(events);
            _ = await eventStore.AddAsync(
                list,
                version,
                CancellationToken.None);
            _ = provider.UncommittedState[_stateName].Should().Be(version + events);
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
            MemoryStateProvider provider = new(new Dictionary<string, object> { { _stateName, version } });
            MessageStore<BaseTestEvent> eventStore = new(provider, _streamName);

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
        public async Task Add_to_stream_with_wrong_version_throw_dbConcurrencyException(int events, long version, long badVersion)
        {
            MemoryStateProvider provider = new(new Dictionary<string, object> { { _stateName, version } });
            MessageStore<BaseTestEvent> eventStore = new(provider, _streamName);

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
        public void Check_event_state_name()
        {
            Mock<IStateStoreProvider> stateManager = new();
            MessageStore<BaseTestEvent> store = new(stateManager.Object, _streamName);
            _ = store.StreamName.Should().Be(_streamName);
            _ = store.GetStreamItemStateName(101).Should().Be(_stateName + "101");
        }

        [Fact]
        public void Check_stream_state_name()
        {
            Mock<IStateStoreProvider> stateManager = new();
            MessageStore<BaseTestEvent> store = new(stateManager.Object, _streamName);
            _ = store.StreamName.Should().Be(_streamName);
            _ = store.GetStreamStateName().Should().Be(_streamName + "Stream");
        }

        [Fact]
        public async Task Empty_stream_version_should_be_zero()
        {
            Mock<IStateStoreProvider> stateManager = new();
            _ = stateManager.Setup(m
                => m.TryGetStateAsync<long>(
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ConditionalValue<long>());
            MessageStore<BaseTestEvent> store = new(stateManager.Object, _streamName);
            long version = await store.GetVersionAsync(CancellationToken.None);
            _ = version.Should().Be(0L);
        }

        [Fact]
        public async Task Events_added_to_stream_should_be_persisted()
        {
            MemoryStateProvider provider = new();
            MessageStore<BaseTestEvent> eventStore = new(provider, _streamName);
            List<BaseTestEvent> list = GetEventList(2);
            _ = await eventStore.AddAsync(
                list,
                0L,
                CancellationToken.None);
            _ = provider.UncommittedState[_stateName + "1"].Should().Be(list[0].IdempotencyId);
            _ = provider.UncommittedState[_stateName + "2"].Should().Be(list[1].IdempotencyId);
            _ = provider.UncommittedState[_streamItemId + list[0].IdempotencyId].Should().Be(list[0]);
            _ = provider.UncommittedState[_streamItemId + list[1].IdempotencyId].Should().Be(list[1]);
        }

        [Fact]
        public async Task Get_stream_should_return_event_value()
        {
            BaseTestEvent2 testEvent = new() { IdempotencyId = "554643", Id = "myId123", Message = "hello", Value2 = "463.33" };
            MemoryStateProvider provider = new(new Dictionary<string, object>
            {
                { _stateName, 123L },
                { _streamItemId + testEvent.IdempotencyId, testEvent },
                { _stateName + "123", testEvent.IdempotencyId },
            });
            MessageStore<BaseTestEvent> eventStore = new(provider, _streamName);

            BaseTestEvent @event = await eventStore.GetAsync(123L, CancellationToken.None);
            _ = @event.Should().NotBeNull();
            _ = @event.Should().BeOfType<BaseTestEvent2>();
            _ = @event.Should().BeEquivalentTo(testEvent);
        }

        [Fact]
        public async Task Get_stream_version_should_return_correct_value()
        {
            MemoryStateProvider provider = new(new Dictionary<string, object> { { "TestStream", 100L } });
            MessageStore<BaseTestEvent> eventStore = new(provider, _streamName);
            long version = await eventStore.GetVersionAsync(CancellationToken.None);
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
                    list.Add(new BaseTestEvent2 { IdempotencyId = id, Id = id, Message = _fakeMessageStart + $"two {id}", Value2 = _fakeValue2Start + id });
                }
                else
                {
                    list.Add(new BaseTestEvent { IdempotencyId = id, Id = id, Message = _fakeMessageStart + $"base {id}" });
                }
            }

            return list;
        }

        private record DummyState(string IdempotencyId, int Id, string Name) : IIdempotent
        {
        }
    }
}