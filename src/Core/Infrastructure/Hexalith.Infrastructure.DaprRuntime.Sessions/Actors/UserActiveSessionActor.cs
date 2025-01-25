// <copyright file="UserActiveSessionActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Actors;

using System.Collections.Generic;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using Hexalith.Application.Sessions.Models;

/// <summary>
/// Represents an actor that manages sessions.
/// </summary>
public class UserActiveSessionActor : Actor, IUserActiveSessionActor
{
    private const string _stateName = "State";
    private Dictionary<string, ActiveSession>? _sessions;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserActiveSessionActor"/> class.
    /// </summary>
    /// <param name="host">The actor host.</param>
    public UserActiveSessionActor(ActorHost host)
        : base(host) => ArgumentNullException.ThrowIfNull(host);

    /// <inheritdoc/>
    public async Task AddAsync(string sessionId, DateTimeOffset expirationDate)
    {
        _sessions ??= await GetOrAddSessionAsync();
        if (_sessions.TryAdd(sessionId, new ActiveSession(sessionId, expirationDate)))
        {
            await SaveAsync();
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ActiveSession>> AllAsync()
        => [.. (_sessions ??= await GetOrAddSessionAsync()).Values];

    /// <inheritdoc/>
    public async Task RemoveAsync(string sessionId)
    {
        _sessions ??= await GetOrAddSessionAsync();
        if (_sessions.Remove(sessionId))
        {
            await SaveAsync();
        }
    }

    private async Task<Dictionary<string, ActiveSession>> GetOrAddSessionAsync()
        => (await StateManager.GetOrAddStateAsync<IEnumerable<ActiveSession>>(_stateName, []))
            .ToDictionary(k => k.Id, v => v);

    private async Task SaveAsync()
    {
        IEnumerable<ActiveSession> state = (_sessions ?? throw new InvalidOperationException("The state is not initialized."))
                .Select(p => p.Value);

        await StateManager.SetStateAsync(
            _stateName,
            state);
        await StateManager.SaveStateAsync();
    }
}