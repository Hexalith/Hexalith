// <copyright file="ActorStateStoreProvider.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.States;

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using Hexalith.Application.Abstractions.States;
using Hexalith.Infrastructure.Serialization.Helpers;

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
        await _actorStateManager.AddStateAsync(key, GetJsonDocument(value), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<T> GetOrAddStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        return GetValue<T>(await _actorStateManager.GetOrAddStateAsync(key, GetJsonDocument(value), cancellationToken));
    }

    /// <inheritdoc/>
    public async Task<T> GetStateAsync<T>(string key, CancellationToken cancellationToken)
    {
        try
        {
            return GetValue<T>(await _actorStateManager.GetStateAsync<JsonDocument>(key, cancellationToken));
        }
        catch (NotSupportedException ex)
        {
            throw new NotSupportedException($"Error while getting state of type {typeof(T).Name} with Key={key}.", ex);
        }
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _actorStateManager.SaveStateAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SetStateAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        await _actorStateManager.SetStateAsync(key, GetJsonDocument(value), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Extensions.Common.ConditionalValue<T>> TryGetStateAsync<T>(string key, CancellationToken cancellationToken)
    {
        ConditionalValue<JsonDocument> result = await _actorStateManager.TryGetStateAsync<JsonDocument>(key, cancellationToken);
        return result.HasValue ? new Extensions.Common.ConditionalValue<T>(GetValue<T>(result.Value)) : new Extensions.Common.ConditionalValue<T>();
    }

    private JsonDocument GetJsonDocument<T>(T value)
    {
        // Dapr issue with Json serialization of polymorphic types. The $type property is not the first field to the json string.
        // Replacing value by a JsonDocument to fix the issue.
        string json = JsonSerializer.Serialize(value, new JsonSerializerOptions().AddPolymorphism());
        JsonDocument jsonElement = JsonDocument.Parse(json);
        return jsonElement;
    }

    private T GetValue<T>(JsonDocument json)
    {
        // Dapr issue with Json serialization of polymorphic types. The $type property is not the first field to the json string.
        // Replacing value by a JsonDocument to fix the issue.
        return JsonSerializer.Deserialize<T>(json.RootElement.GetRawText(), new JsonSerializerOptions().AddPolymorphism())!;
    }
}