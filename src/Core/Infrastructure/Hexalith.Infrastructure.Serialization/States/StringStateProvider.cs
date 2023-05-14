// <copyright file="StringStateProvider.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Serialization.States;

using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.States;
using Hexalith.Extensions.Common;

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
    private readonly Dictionary<string, string> _state = new();

    /// <summary>
    /// The uncommitted state.
    /// </summary>
    private readonly Dictionary<string, string> _uncommittedState = new();

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
    public Task AddStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        string v = JsonSerializer.Serialize(value);
        _uncommittedState.Add(key, v);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<T> GetOrAddStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        ConditionalValue<T> result = await TryGetStateAsync<T>(key, cancellationToken);
        if (result.HasValue)
        {
            return result.Value;
        }

        await AddStateAsync(key, value, cancellationToken);

        return await GetStateAsync<T>(key, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<T> GetStateAsync<T>(string key, CancellationToken cancellationToken)
    {
        ConditionalValue<T> result = await TryGetStateAsync<T>(key, cancellationToken);
        return result.HasValue
            ? result.Value
            : throw new KeyNotFoundException($"The key '{key}' was not found in the state store.");
    }

    /// <inheritdoc/>
    public Task SaveChangesAsync(CancellationToken cancellationToken)
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

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task SetStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        if (_uncommittedState.ContainsKey(key))
        {
            _ = _uncommittedState.Remove(key);
        }

        string v = JsonSerializer.Serialize(value);
        _uncommittedState.Add(key, v);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<ConditionalValue<T>> TryGetStateAsync<T>(string key, CancellationToken cancellationToken)
    {
        if (_uncommittedState.TryGetValue(key, out string? uncommitted))
        {
            T? value = JsonSerializer.Deserialize<T>(uncommitted);
            return value is null
                ? Task.FromException<ConditionalValue<T>>(new InvalidOperationException($"The key '{key}' was found in the uncommitted state store but the value is null."))
                : Task.FromResult(new ConditionalValue<T>(value));
        }

        if (_state.TryGetValue(key, out string? committed))
        {
            T? value = JsonSerializer.Deserialize<T>(committed);
            return value is null
                ? Task.FromException<ConditionalValue<T>>(new InvalidOperationException($"The key '{key}' was found in the state store but the value is null."))
                : Task.FromResult(new ConditionalValue<T>(value));
        }

        return Task.FromResult(new ConditionalValue<T>());
    }
}