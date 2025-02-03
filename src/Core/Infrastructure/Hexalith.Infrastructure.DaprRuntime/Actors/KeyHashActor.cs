// <copyright file="KeyHashActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System;
using System.Collections.Generic;

using Dapr.Actors.Runtime;

using Hexalith.Application;

/// <summary>
/// Actor that manages a set of unique keys.
/// </summary>
public class KeyHashActor : Actor, IKeyHashActor
{
    private HashSet<string>? _state;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyHashActor"/> class.
    /// </summary>
    /// <param name="host">The <see cref="ActorHost" /> that will host this actor instance.</param>
    /// <param name="stateManager">The state manager to be used for managing actor state.</param>
    public KeyHashActor(ActorHost host, IActorStateManager? stateManager = null)
        : base(host)
    {
        ArgumentNullException.ThrowIfNull(host);
        if (stateManager is not null)
        {
            StateManager = stateManager;
        }
    }

    /// <inheritdoc/>
    public async Task<int> AddAsync(string value)
    {
        if (!(_state ??= await GetStateAsync()).Contains(value))
        {
            _ = _state.Add(value);
            await SaveStateAsync();
        }

        return _state.Count;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> AllAsync()
        => _state ??= await GetStateAsync();

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string value)
        => (_state ??= await GetStateAsync()).Contains(value);

    /// <inheritdoc/>
    public async Task RemoveAsync(string value)
    {
        if ((_state ??= await GetStateAsync()).Contains(value))
        {
            _ = _state.Remove(value);
            await SaveStateAsync();
        }
    }

    /// <summary>
    /// Gets the current state of the actor.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the state.</returns>
    private async Task<HashSet<string>> GetStateAsync()
        => [.. await StateManager.GetOrAddStateAsync(ApplicationConstants.StateName, Array.Empty<string>())];

    /// <summary>
    /// Saves the current state of the actor.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private new async Task SaveStateAsync()
    {
        await StateManager.SetStateAsync(ApplicationConstants.StateName, _state?.ToArray() ?? [], CancellationToken.None);
        await StateManager.SaveStateAsync();
    }
}