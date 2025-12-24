// <copyright file="MemoryStateProviderTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.States;

using Hexalith.Application.States;
using Hexalith.Commons.Errors;
using Hexalith.Commons.Objects;
using Hexalith.Extensions.Common;

using Shouldly;

public class MemoryStateProviderTest
{
    [Fact]
    public async Task AddStateShouldBePersisted()
    {
        MemoryStateProvider provider = new();
        DummyState state = new("5354323", 123, "one two three");
        await provider.SetStateAsync(state.IdempotencyId, state, CancellationToken.None);
        provider.UncommittedState.Count.ShouldBe(1);
        provider.UncommittedState[state.IdempotencyId].ShouldBe(state);
    }

    [Fact]
    public async Task GetStateShouldReturnContent()
    {
        DummyState state = new("5354323", 123, "one two three");
        MemoryStateProvider provider = new(new Dictionary<string, object> { { state.IdempotencyId, state } });
        DummyState result = await provider.GetStateAsync<DummyState>(state.IdempotencyId, CancellationToken.None);
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(state);
    }

    [Fact]
    public async Task TryGetStateShouldReturnContent()
    {
        DummyState state = new("5354323", 123, "one two three");
        MemoryStateProvider provider = new(new Dictionary<string, object> { { state.IdempotencyId, state } });
        ConditionalValue<DummyState> result = await provider.TryGetStateAsync<DummyState>(state.IdempotencyId, CancellationToken.None);
        result.ShouldNotBeNull();
        result.HasValue.ShouldBeTrue();
        result.Value.ShouldBeEquivalentTo(state);
    }

    [Fact]
    public async Task TryGetStateShouldReturnNocontent()
    {
        DummyState state = new("5354323", 123, "one two three");
        MemoryStateProvider provider = new(new Dictionary<string, object> { { state.IdempotencyId, state } });
        ConditionalValue<DummyState> result = await provider.TryGetStateAsync<DummyState>(state.IdempotencyId, CancellationToken.None);
        result.ShouldNotBeNull();
        result.HasValue.ShouldBeTrue();
        result.Value.ShouldBeEquivalentTo(state);
    }

    private record DummyState(string IdempotencyId, int Id, string Name) : IIdempotent
    {
    }
}