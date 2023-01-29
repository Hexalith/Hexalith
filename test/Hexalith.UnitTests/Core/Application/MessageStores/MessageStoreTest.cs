// <copyright file="MessageStoreTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.MessageStores
{
    using System.Data;

    using FluentAssertions;

    using Hexalith.Application.Abstractions.States;
    using Hexalith.Application.StreamStores;
    using Hexalith.Extensions.Common;
    using Hexalith.Extensions.Helpers;

    using Moq;

    public class MessageStoreTest
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
            (MessageStore<BaseTestEvent> eventStore, Mock<IStateStoreProvider> mock) = GetEventStoreWithMock(version);
            List<BaseTestEvent> list = GetEventList(events);
            long newVersion = await eventStore.AddAsync(
                list,
                version,
                CancellationToken.None);
            _ = newVersion.Should().Be(version + events);
            if (version == 0)
            {
                mock.Verify(
                    m => m.AddStateAsync(
                    It.Is<string>(s => s == _streamStateName),
                    It.Is<long>(s => s == version + events),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            }
            else
            {
                mock.Verify(
                    m => m.SetStateAsync(
                    It.Is<string>(s => s == _streamStateName),
                    It.Is<long>(s => s == version + events),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            mock.Verify(
                m => m.AddStateAsync(
                It.Is<string>(s => s.StartsWith(_streamStateName, StringComparison.InvariantCulture)),
                It.Is<BaseTestEvent>(s => s.Message.Contains("I am fake", StringComparison.InvariantCulture)),
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
            MessageStore<BaseTestEvent> eventStore = GetEventStore(version);

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
            MessageStore<BaseTestEvent> eventStore = GetEventStore(version);

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
            const string streamName = "Test";
            Mock<IStateStoreProvider> stateManager = new();
            MessageStore<BaseTestEvent> store = new(stateManager.Object, "Test");
            _ = store.StreamName.Should().Be(streamName);
            _ = store.GetMessageStateName(101).Should().Be(streamName + "Stream101");
        }

        [Fact]
        public void Check_stream_state_name()
        {
            const string streamName = "Test";
            Mock<IStateStoreProvider> stateManager = new();
            MessageStore<BaseTestEvent> store = new(stateManager.Object, streamName);
            _ = store.StreamName.Should().Be(streamName);
            _ = store.GetStreamStateName().Should().Be(streamName + "Stream");
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
            MessageStore<BaseTestEvent> store = new(stateManager.Object, "Test");
            long version = await store.GetVersionAsync(CancellationToken.None);
            _ = version.Should().Be(0L);
        }

        [Fact]
        public async Task Events_added_to_stream_should_be_persisted()
        {
            const long version = 100;

            (MessageStore<BaseTestEvent> eventStore, Mock<IStateStoreProvider> mock) = GetEventStoreWithMock(version);
            List<BaseTestEvent> list = GetEventList(2);
            long newVersion = await eventStore.AddAsync(
                list,
                version,
                CancellationToken.None);
            _ = newVersion.Should().Be(version + 2L);
            mock.Verify(
                m => m.AddStateAsync(
                It.Is("TestStream101", StringComparer.InvariantCulture),
                It.Is<BaseTestEvent>(s => s.GetType() == typeof(BaseTestEvent) && s.Id == list[0].Id),
                It.IsAny<CancellationToken>()),
                Times.Once);
            mock.Verify(
                m => m.AddStateAsync(
                It.Is("TestStream102", StringComparer.InvariantCulture),
                It.Is<BaseTestEvent>(s => s.GetType() == typeof(BaseTestEvent2) && s.Id == list[1].Id),
                It.IsAny<CancellationToken>()),
                Times.Once);
            mock.Verify(
                m => m.SetStateAsync(
                It.Is<string>(s => s == _streamStateName),
                It.Is<long>(s => s == version + 2L),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Get_stream_should_return_event_value()
        {
            BaseTestEvent2 testEvent = new("myId123", "hello", "463.33");
            Mock<IStateStoreProvider> stateManager = new();
            stateManager.Setup(m
                => m.TryGetStateAsync<BaseTestEvent>(
                       "TestStream123",
                       It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ConditionalValue<BaseTestEvent>(testEvent))
                .Verifiable();

            MessageStore<BaseTestEvent> store = new(stateManager.Object, "Test");
            BaseTestEvent @event = await store.GetAsync(123L, CancellationToken.None);
            stateManager.VerifyAll();
            _ = @event.Should().NotBeNull();
            _ = @event.Should().BeOfType<BaseTestEvent2>();
            _ = @event.Should().BeEquivalentTo(testEvent);
        }

        [Fact]
        public async Task Get_stream_version_should_return_correct_value()
        {
            MessageStore<BaseTestEvent> store = GetEventStore(100);
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

        private static MessageStore<BaseTestEvent> GetEventStore(long version)
        {
            (MessageStore<BaseTestEvent> s, Mock<IStateStoreProvider> _) = GetEventStoreWithMock(version);
            return s;
        }

        private static (MessageStore<BaseTestEvent> Store, Mock<IStateStoreProvider> StateManager) GetEventStoreWithMock(long version)
        {
            Mock<IStateStoreProvider> mock = new();
            _ = mock
                .Setup(m
                => m.TryGetStateAsync<long>(
                        It.Is<string>(s => s == _streamStateName),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ConditionalValue<long>(version));
            _ = mock
                .Setup(m
                => m.GetOrAddStateAsync(
                        It.Is<string>(s => s == _streamStateName),
                        It.Is<long>(l => l == 0L),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(version);
            MessageStore<BaseTestEvent> eventStore = new(mock.Object, _streamName);
            return (eventStore, mock);
        }
    }
}