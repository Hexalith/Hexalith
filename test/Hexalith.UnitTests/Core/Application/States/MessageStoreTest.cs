// <copyright file="MessageStoreTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.States;

using FluentAssertions;

using Hexalith.Application.States;
using Hexalith.Application.StreamStores;
using Hexalith.Extensions.Helpers;

public class MessageStoreTest
{
    [Fact]
    public async Task Add_state_should_be_persisted()
    {
        const string storeName = "DummyStore";
        const string streamName = storeName + "Stream";

        MemoryStateProvider provider = new();
        MessageStore<DummyState> store = new(provider, storeName);
        DummyState state = new(123, "one two three");
        _ = await store.AddAsync(state.IntoArray(), 0, CancellationToken.None);
        _ = provider.UncommittedState.Should().HaveCount(2);
        _ = provider.UncommittedState[streamName].Should().Be(1L);
        _ = provider.UncommittedState[streamName + "1"].Should().Be(state);
    }

    private record DummyState(int Id, string Name)
    {
    }
}