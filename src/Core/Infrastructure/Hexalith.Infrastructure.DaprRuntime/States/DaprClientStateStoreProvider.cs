// <copyright file="DaprClientStateStoreProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.States;

using System;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Client;

using Hexalith.Application.States;
using Hexalith.Extensions.Common;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/// <summary>
/// Class DaprClientStateStoreProvider.
/// Implements the <see cref="IStateStoreProvider" />.
/// </summary>
/// <seealso cref="IStateStoreProvider" />
public class DaprClientStateStoreProvider : IStateStoreProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DaprClientStateStoreProvider"/> class.
    /// </summary>
    /// <param name="daprClient">The dapr client.</param>
    /// <param name="settings">The settings.</param>
    [ActivatorUtilitiesConstructor]
    public DaprClientStateStoreProvider(DaprClient daprClient, IOptions<StateStoreSettings> settings)
        : this(
              daprClient,
              (settings ?? throw new ArgumentNullException(nameof(settings))).Value.Name)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DaprClientStateStoreProvider"/> class.
    /// </summary>
    /// <param name="daprClient">The dapr client.</param>
    /// <param name="stateStoreName">Name of the state store.</param>
    /// <exception cref="System.ArgumentNullException">Null argument.</exception>
    public DaprClientStateStoreProvider(DaprClient daprClient, string stateStoreName)
    {
        ArgumentNullException.ThrowIfNull(daprClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(stateStoreName);
        DaprClient = daprClient;
        StateStoreName = stateStoreName;
    }

    /// <summary>
    /// Gets the dapr client.
    /// </summary>
    /// <value>The dapr client.</value>
    protected DaprClient DaprClient { get; }

    /// <summary>
    /// Gets the name of the state store.
    /// </summary>
    /// <value>The name of the state store.</value>
    protected string StateStoreName { get; }

    /// <inheritdoc/>
    public async Task AddStateAsync<T>(string key, T value, CancellationToken cancellationToken) => await DaprClient.SaveStateAsync(StateStoreName, key, value, null, null, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<T> GetOrAddStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        T? result = await DaprClient
            .GetStateAsync<T>(
                StateStoreName,
                key,
                null,
                null,
                cancellationToken)
            .ConfigureAwait(false);
        if (result is null)
        {
            await DaprClient.SaveStateAsync(StateStoreName, key, value, null, null, cancellationToken).ConfigureAwait(false);
            return value;
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task<T> GetStateAsync<T>(string key, CancellationToken cancellationToken) => await DaprClient.GetStateAsync<T>(StateStoreName, key, null, null, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task SaveChangesAsync(CancellationToken cancellationToken) => await Task.CompletedTask.ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task SetStateAsync<T>(string key, T value, CancellationToken cancellationToken) => await DaprClient.SaveStateAsync(StateStoreName, key, value, null, null, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<ConditionalValue<T>> TryGetStateAsync<T>(string key, CancellationToken cancellationToken)
    {
        T result = await DaprClient
            .GetStateAsync<T>(StateStoreName, key, null, null, cancellationToken)
            .ConfigureAwait(false);
        return result == null
            ? new ConditionalValue<T>()
            : new ConditionalValue<T>(result);
    }
}