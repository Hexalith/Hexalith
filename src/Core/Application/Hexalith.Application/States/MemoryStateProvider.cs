// <copyright file="MemoryStateProvider.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.States;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Extensions.Common;

/// <summary>
/// Class MemoryStateProvider.
/// Implements the <see cref="IStateStoreProvider" />.
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
    public Task AddStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        object? v = value;
        _uncommittedState.Add(key, v);
        return Task.CompletedTask;
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
    public Task SaveChangesAsync(CancellationToken cancellationToken)
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

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task SetStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        if (_uncommittedState.ContainsKey(key))
        {
            _ = _uncommittedState.Remove(key);
        }

        _uncommittedState.Add(key, value);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<ConditionalValue<T>> TryGetStateAsync<T>(string key, CancellationToken cancellationToken)
    {
        bool hasValue = _uncommittedState.TryGetValue(key, out object? uncommitted);
        if (hasValue)
        {
            return Task.FromResult(new ConditionalValue<T>((T)uncommitted!));
        }

        hasValue = _state.TryGetValue(key, out object? committed);
        return Task.FromResult(hasValue ? new ConditionalValue<T>((T)committed!) : new ConditionalValue<T>());
    }
}