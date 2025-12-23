// <copyright file="MessageStoreTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.MessageStores;

using System.Data;

using Hexalith.Application.States;
using Hexalith.Application.StreamStores;
using Hexalith.Applications.States;
using Hexalith.Commons.Errors;
using Hexalith.Commons.Metadatas;
using Hexalith.Commons.Objects;
using Hexalith.Commons.Strings;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.Serialization.States;
using Hexalith.PolymorphicSerializations;
using Hexalith.UnitTests.Core.Application.Commands;

using NSubstitute;

using Shouldly;

public class MessageStoreTest
{
    private const string _fakeMessageStart = "I am fake event ";
    private const string _fakeValue2Start = "Hello world ";
    private const string _stateName = "TestStream";
    private const string _streamItemId = "TestStreamId-";
    private const string _streamName = "Test";

    public MessageStoreTest() => Extensions.HexalithUnitTestsSerialization.RegisterPolymorphicMappers();

    [Fact]
    public async Task AddAndGetFromSerializedCommandStateShouldReturnSame()
    {
        StringStateProvider provider = new();
        MessageStore<MessageState> store = new(provider, _streamName);
        DummyCommand1 command1 = new("5354323", 123);
        DummyCommand2 command2 = new("AAAABBCC", 960);
        MessageState original1 = new(command1, command1.CreateMetadata());
        MessageState original2 = new(command2, command2.CreateMetadata());
        _ = await store.AddAsync([original1], 0, CancellationToken.None);
        _ = await store.AddAsync([original2], 1, CancellationToken.None);
        _ = provider.SaveChangesAsync(CancellationToken.None);
        MessageState result1 = await store.GetAsync(1, CancellationToken.None);
        MessageState result2 = await store.GetAsync(2, CancellationToken.None);
        result1.ShouldBeEquivalentTo(original1);
        result2.ShouldBeEquivalentTo(original2);
    }

    [Fact]
    public async Task AddAndGetFromSerializedStateShouldReturnSame()
    {
        StringStateProvider provider = new();
        MessageStore<DummyState> store = new(provider, _streamName);
        DummyState state = new("5354323", 123, "one two three");
        _ = await store.AddAsync([state], 0, CancellationToken.None);
        DummyState result = await store.GetAsync(1, CancellationToken.None);
        result.ShouldBeEquivalentTo(state);
    }

    [Fact]
    public async Task AddStateShouldBePersisted()
    {
        MemoryStateProvider provider = new();
        MessageStore<DummyState> store = new(provider, _streamName);
        DummyState state = new("5354323", 123, "one two three");
        _ = await store.AddAsync([state], 0, CancellationToken.None);
        provider.UncommittedState.Count.ShouldBe(3);
        provider.UncommittedState[_stateName].ShouldBe(1L);
        provider.UncommittedState[_stateName + "1"].ShouldBe(state);
        provider.UncommittedState[_streamItemId + state.IdempotencyId].ShouldBe(1L);
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
    public async Task AddStateShouldPersistNewVersion(int events, long version)
    {
        MemoryStateProvider provider = new(new Dictionary<string, object> { { _stateName, version } });
        MessageStore<MessageState> eventStore = new(provider, _streamName);
        List<MessageState> list = GetEventList(events);
        _ = await eventStore.AddAsync(
            list,
            version,
            CancellationToken.None);
        provider.UncommittedState[_stateName].ShouldBe(version + events);
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
    public async Task AddToStreamReturnsVersionPlusEventCount(int events, long version)
    {
        MemoryStateProvider provider = new(new Dictionary<string, object> { { _stateName, version } });
        MessageStore<MessageState> eventStore = new(provider, _streamName);

        long newVersion = await eventStore.AddAsync(
            GetEventList(events),
            version,
            CancellationToken.None);
        newVersion.ShouldBe(version + events);
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
    public async Task AddToStreamWithWrongVersionThrowDbConcurrencyException(int events, long version, long badVersion)
    {
        MemoryStateProvider provider = new(new Dictionary<string, object> { { _stateName, version } });
        MessageStore<MessageState> eventStore = new(provider, _streamName);

        async Task<long> Act() => await eventStore.AddAsync(
            GetEventList(events),
            badVersion,
            CancellationToken.None);

        DBConcurrencyException exception = await Assert.ThrowsAsync<DBConcurrencyException>(Act);
        exception.Message.ShouldNotBeNullOrEmpty();
        exception.Message.ShouldContain(version.ToInvariantString());
        exception.Message.ShouldContain(badVersion.ToInvariantString());
    }

    [Fact]
    public void CheckEventStateName()
    {
        IStateStoreProvider stateManager = Substitute.For<IStateStoreProvider>();
        MessageStore<MessageState> store = new(stateManager, _streamName);
        store.StreamName.ShouldBe(_streamName);
        store.GetStreamItemStateName(101).ShouldBe(_stateName + "101");
    }

    [Fact]
    public void CheckStreamStateName()
    {
        IStateStoreProvider stateManager = Substitute.For<IStateStoreProvider>();
        MessageStore<MessageState> store = new(stateManager, _streamName);
        store.StreamName.ShouldBe(_streamName);
        store.StreamStateName.ShouldBe(_streamName + "Stream");
    }

    [Fact]
    public async Task EmptyStreamVersionShouldBeZero()
    {
        IStateStoreProvider stateManager = Substitute.For<IStateStoreProvider>();
        stateManager.TryGetStateAsync<long>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(new ConditionalValue<long>());
        MessageStore<MessageState> store = new(stateManager, _streamName);
        long version = await store.GetVersionAsync(CancellationToken.None);
        version.ShouldBe(0L);
    }

    [Fact]
    public async Task EventsAddedToStreamShouldBePersisted()
    {
        MemoryStateProvider provider = new();
        MessageStore<MessageState> eventStore = new(provider, _streamName);
        List<MessageState> list = GetEventList(2);
        _ = await eventStore.AddAsync(
            list,
            0L,
            CancellationToken.None);
        provider.UncommittedState[_stateName + "1"].ShouldBe(list[0]);
        provider.UncommittedState[_stateName + "2"].ShouldBe(list[1]);
        provider.UncommittedState[_streamItemId + list[0].IdempotencyId].ShouldBe(1L);
        provider.UncommittedState[_streamItemId + list[1].IdempotencyId].ShouldBe(2L);
    }

    [Fact]
    public async Task GetStreamShouldReturnEventValue()
    {
        BaseTestEvent2 testEvent = new("554643", "myId123", "hello", "463.33");
        MemoryStateProvider provider = new(new Dictionary<string, object>
        {
            { _stateName, 123L },
            { _streamItemId + testEvent.IdempotencyId, 123L },
            { _stateName + "123", new MessageState(testEvent, testEvent.CreateMetadata( "user123", "part123", DateTimeOffset.Now)) },
        });
        MessageStore<MessageState> eventStore = new(provider, _streamName);

        MessageState @event = await eventStore.GetAsync(123L, CancellationToken.None);
        @event.Metadata.ShouldNotBeNull();
        @event.Metadata.ShouldBeOfType<Metadata>();

        @event.Message.ShouldNotBeNullOrWhiteSpace();
        @event.MessageObject.ShouldNotBeNull();
        @event.MessageObject.ShouldBeOfType<BaseTestEvent2>();
        @event.MessageObject.ShouldBeEquivalentTo(testEvent);
    }

    [Fact]
    public async Task GetStreamVersionShouldReturnCorrectValue()
    {
        MemoryStateProvider provider = new(new Dictionary<string, object> { { "TestStream", 100L } });
        MessageStore<MessageState> eventStore = new(provider, _streamName);
        long version = await eventStore.GetVersionAsync(CancellationToken.None);
        version.ShouldBe(100L);
    }

    private static List<MessageState> GetEventList(int count)
    {
        List<MessageState> list = new(count);
        for (int i = 1; i <= count; i++)
        {
            string id = i.ToInvariantString();
            object e = i % 2 == 0
                ? new BaseTestEvent2(
                    id,
                    id,
                    _fakeMessageStart + $"two {id}",
                    _fakeValue2Start + id)
                : (object)new BaseTestEvent(id, id, _fakeMessageStart + $"base {id}");
            list.Add(new MessageState(
                (Polymorphic)e,
                new Metadata(
                 e.CreateMessageMetadata(DateTimeOffset.Now),
                 new ContextMetadata(
                     id,
                     "TestUser",
                     "Test",
                     DateTimeOffset.Now,
                     TimeSpan.FromSeconds(10),
                     11,
                     "V12",
                     "SES2132",
                     ["sc01", "sc02"]))));
        }

        return list;
    }

    private sealed record DummyState(string IdempotencyId, int Id, string Name) : IIdempotent
    {
    }
}
