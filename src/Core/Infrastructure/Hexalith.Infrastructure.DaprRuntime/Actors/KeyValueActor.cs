// <copyright file="KeyValueActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System;

using Dapr.Actors.Runtime;

using Hexalith.Application;

/// <summary>
/// Key value actor class.
/// Implements the <see cref="Actor" />
/// Implements the <see cref="Actors.IKeyValueActor" />.
/// </summary>
/// <seealso cref="Actor" />
/// <seealso cref="Actors.IKeyValueActor" />
public class KeyValueActor : Actor, IKeyValueActor
{
    /// <summary>
    /// The is loaded.
    /// </summary>
    private bool _isLoaded;

    /// <summary>
    /// The value.
    /// </summary>
    private string? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyValueActor" /> class.
    /// </summary>
    /// <param name="host">The <see cref="ActorHost" /> that will host this actor instance.</param>
    /// <param name="stateManager">The state manager to be used for managing actor state.</param>
    public KeyValueActor(ActorHost host, IActorStateManager? stateManager = null)
        : base(host)
    {
        ArgumentNullException.ThrowIfNull(host);
        if (stateManager is not null)
        {
            StateManager = stateManager;
        }
    }

    /// <inheritdoc/>
    public async Task<string?> GetAsync()
    {
        if (!_isLoaded)
        {
            ConditionalValue<string?> result = await StateManager.TryGetStateAsync<string?>(ApplicationConstants.StateName).ConfigureAwait(false);
            _value = result.HasValue ? result.Value : null;
            _isLoaded = true;
        }

        return _value;
    }

    /// <inheritdoc/>
    public async Task RemoveAsync()
    {
        if (!_isLoaded || _value != null)
        {
            await StateManager.SetStateAsync<string?>(ApplicationConstants.StateName, null).ConfigureAwait(false);
            await StateManager.SaveStateAsync().ConfigureAwait(false);
            _value = null;
            _isLoaded = true;
        }
    }

    /// <inheritdoc/>
    public async Task SetAsync(string? value)
    {
        if (!_isLoaded || _value != value)
        {
            await StateManager.SetStateAsync(ApplicationConstants.StateName, value).ConfigureAwait(false);
            await StateManager.SaveStateAsync().ConfigureAwait(false);
            _value = value;
            _isLoaded = true;
        }
    }
}