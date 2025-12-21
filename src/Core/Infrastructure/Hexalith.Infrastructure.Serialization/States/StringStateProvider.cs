// <copyright file="StringStateProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Serialization.States;

using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.States;
using Hexalith.Commons.Errors;
using Hexalith.Extensions.Common;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Class StringStateProvider.
/// Implements the <see cref="IStateStoreProvider" />.
/// </summary>
/// <seealso cref="IStateStoreProvider" />
public class StringStateProvider : IStateStoreProvider
{
    /// <summary>
    /// The state.
    /// </summary>
    private readonly Dictionary<string, string> _state = [];

    /// <summary>
    /// The uncommitted state.
    /// </summary>
    private readonly Dictionary<string, string> _uncommittedState = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="StringStateProvider"/> class.
    /// </summary>
    public StringStateProvider()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringStateProvider"/> class.
    /// </summary>
    /// <param name="state">The provider initial state.</param>
    public StringStateProvider(Dictionary<string, string> state) => _state = state;

    /// <summary>
    /// Gets the state.
    /// </summary>
    /// <value>The state.</value>
    public IReadOnlyDictionary<string, string> State => _state;

    /// <summary>
    /// Gets the state of the uncommitted.
    /// </summary>
    /// <value>The state of the uncommitted.</value>
    public IReadOnlyDictionary<string, string> UncommittedState => _uncommittedState;

    /// <inheritdoc/>
    public async Task AddStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        string v = JsonSerializer.Serialize(value, PolymorphicHelper.DefaultJsonSerializerOptions);
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
        foreach (KeyValuePair<string, string> entry in _uncommittedState.ToList())
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

        string v = JsonSerializer.Serialize(value, PolymorphicHelper.DefaultJsonSerializerOptions);
        _uncommittedState.Add(key, v);
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<ConditionalValue<T>> TryGetStateAsync<T>(string key, CancellationToken cancellationToken)
    {
        if (_uncommittedState.TryGetValue(key, out string? uncommitted))
        {
            T? value = JsonSerializer.Deserialize<T>(uncommitted, PolymorphicHelper.DefaultJsonSerializerOptions);
            return value is null
                ? throw new InvalidOperationException($"The key '{key}' was found in the uncommitted state store but the value is null.")
                : new ConditionalValue<T>(value);
        }

        if (_state.TryGetValue(key, out string? committed))
        {
            T? value = JsonSerializer.Deserialize<T>(committed, PolymorphicHelper.DefaultJsonSerializerOptions);
            return value is null
                ? throw new InvalidOperationException($"The key '{key}' was found in the state store but the value is null.")
                : new ConditionalValue<T>(value);
        }

        return await Task.FromResult(new ConditionalValue<T>()).ConfigureAwait(false);
    }
}