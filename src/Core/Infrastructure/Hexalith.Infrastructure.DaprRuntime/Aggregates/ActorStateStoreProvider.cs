// <copyright file="ActorStateStoreProvider.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Aggregates;

using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using Hexalith.Application.Abstractions.States;

/// <summary>
/// Class ActorStateStoreProvider.
/// Implements the <see cref="IStateStoreProvider" />.
/// </summary>
/// <seealso cref="IStateStoreProvider" />
public class ActorStateStoreProvider : IStateStoreProvider
{
    /// <summary>
    /// The actor state manager.
    /// </summary>
    private readonly IActorStateManager _actorStateManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorStateStoreProvider" /> class.
    /// </summary>
    /// <param name="actorStateManager">The actor state manager.</param>
    public ActorStateStoreProvider(IActorStateManager actorStateManager)
    {
        ArgumentNullException.ThrowIfNull(actorStateManager);
        _actorStateManager = actorStateManager;
    }

    /// <inheritdoc/>
    public async Task AddStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        await _actorStateManager.SetStateAsync(key, value, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<T> GetOrAddStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        return await _actorStateManager.GetOrAddStateAsync(key, value, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<T> GetStateAsync<T>(string key, CancellationToken cancellationToken)
    {
        return await _actorStateManager.GetStateAsync<T>(key, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _actorStateManager.SaveStateAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SetStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        await _actorStateManager.SetStateAsync(key, value, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Extensions.Common.ConditionalValue<T>> TryGetStateAsync<T>(string key, CancellationToken cancellationToken)
    {
        ConditionalValue<T> result = await _actorStateManager.TryGetStateAsync<T>(key, cancellationToken);
        return result.HasValue ? new Extensions.Common.ConditionalValue<T>(result.Value) : new Extensions.Common.ConditionalValue<T>();
    }
}