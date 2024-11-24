// <copyright file="MemoryStateProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.States;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Extensions.Common;

/// <summary>
/// Memory State Provider.
/// </summary>
/// <seealso cref="IStateStoreProvider" />
public class MemoryStateProvider : IStateStoreProvider
{
    /// <summary>
    /// The state.
    /// </summary>
    private readonly Dictionary<string, object?> _state = [];

    /// <summary>
    /// The uncommitted state.
    /// </summary>
    private readonly Dictionary<string, object?> _uncommittedState = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryStateProvider"/> class.
    /// </summary>
    public MemoryStateProvider()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryStateProvider"/> class.
    /// </summary>
    /// <param name="state">The provider initial state.</param>
    public MemoryStateProvider(Dictionary<string, object?> state) => _state = state;

    /// <summary>
    /// Gets the state.
    /// </summary>
    /// <value>The state.</value>
    public IReadOnlyDictionary<string, object?> State => _state;

    /// <summary>
    /// Gets the state of the uncommitted.
    /// </summary>
    /// <value>The state of the uncommitted.</value>
    public IReadOnlyDictionary<string, object?> UncommittedState => _uncommittedState;

    /// <inheritdoc/>
    public async Task AddStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        object? v = value;
        _uncommittedState.Add(key, v);
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<T> GetOrAddStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        ConditionalValue<T> result = await TryGetStateAsync<T>(key, cancellationToken).ConfigureAwait(false);
        if (result.HasValue)
        {
            return result.Value;
        }

        await AddStateAsync(key, value, cancellationToken).ConfigureAwait(false);

        return await GetStateAsync<T>(key, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<T> GetStateAsync<T>(string key, CancellationToken cancellationToken)
    {
        ConditionalValue<T> result = await TryGetStateAsync<T>(key, cancellationToken).ConfigureAwait(false);
        return result.HasValue
            ? result.Value
            : throw new KeyNotFoundException($"The key '{key}' was not found in the state store.");
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        foreach (KeyValuePair<string, object?> entry in _uncommittedState.ToList())
        {
            if (_state.ContainsKey(entry.Key))
            {
                _ = _state.Remove(entry.Key);
            }

            _state.Add(entry.Key, entry.Value);
            _ = _uncommittedState.Remove(entry.Key);
        }

        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task SetStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        if (_uncommittedState.ContainsKey(key))
        {
            _ = _uncommittedState.Remove(key);
        }

        _uncommittedState.Add(key, value);
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ConditionalValue<T>> TryGetStateAsync<T>(string key, CancellationToken cancellationToken)
    {
        bool hasValue = _uncommittedState.TryGetValue(key, out object? uncommitted);
        if (hasValue)
        {
            return await Task.FromResult(new ConditionalValue<T>((T)uncommitted!)).ConfigureAwait(false);
        }

        hasValue = _state.TryGetValue(key, out object? committed);
        return await Task.FromResult(hasValue ? new ConditionalValue<T>((T)committed!) : new ConditionalValue<T>()).ConfigureAwait(false);
    }
}