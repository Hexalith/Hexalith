// <copyright file="MemoryStateProviderTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.States;

using FluentAssertions;

using Hexalith.Application.States;
using Hexalith.Extensions.Common;

public class MemoryStateProviderTest
{
    [Fact]
    public async Task AddStateShouldBePersisted()
    {
        MemoryStateProvider provider = new();
        DummyState state = new("5354323", 123, "one two three");
        await provider.SetStateAsync(state.IdempotencyId, state, CancellationToken.None);
        _ = provider.UncommittedState.Should().HaveCount(1);
        _ = provider.UncommittedState[state.IdempotencyId].Should().Be(state);
    }

    [Fact]
    public async Task GetStateShouldReturnContent()
    {
        DummyState state = new("5354323", 123, "one two three");
        MemoryStateProvider provider = new(new Dictionary<string, object> { { state.IdempotencyId, state } });
        DummyState result = await provider.GetStateAsync<DummyState>(state.IdempotencyId, CancellationToken.None);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(state);
    }

    [Fact]
    public async Task TryGetStateShouldReturnContent()
    {
        DummyState state = new("5354323", 123, "one two three");
        MemoryStateProvider provider = new(new Dictionary<string, object> { { state.IdempotencyId, state } });
        ConditionalValue<DummyState> result = await provider.TryGetStateAsync<DummyState>(state.IdempotencyId, CancellationToken.None);
        _ = result.Should().NotBeNull();
        _ = result.HasValue.Should().BeTrue();
        _ = result.Value.Should().BeEquivalentTo(state);
    }

    [Fact]
    public async Task TryGetStateShouldReturnNocontent()
    {
        DummyState state = new("5354323", 123, "one two three");
        MemoryStateProvider provider = new(new Dictionary<string, object> { { state.IdempotencyId, state } });
        ConditionalValue<DummyState> result = await provider.TryGetStateAsync<DummyState>(state.IdempotencyId, CancellationToken.None);
        _ = result.Should().NotBeNull();
        _ = result.HasValue.Should().BeTrue();
        _ = result.Value.Should().BeEquivalentTo(state);
    }

    private record DummyState(string IdempotencyId, int Id, string Name) : IIdempotent
    {
    }
}