// <copyright file="SessionActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Actors;

using System.Threading.Tasks;

using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using Hexalith.Application.Sessions.Models;

/// <summary>
/// Represents an actor that manages sessions.
/// </summary>
public class SessionActor : Actor, ISessionActor
{
    private const string _collectionActorName = nameof(Sessions);
    private const string _stateName = "State";
    private readonly TimeProvider _timeProvider;
    private Session? _session;

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionActor"/> class.
    /// </summary>
    /// <param name="host">The actor host.</param>
    /// <param name="timeProvider">The time provider.</param>
    public SessionActor(ActorHost host, TimeProvider timeProvider)
        : base(host)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        _timeProvider = timeProvider;
    }

    /// <inheritdoc/>
    public async Task CloseAsync()
    {
        await CloseAndRemoveActiveSessionAsync();
        await SaveAsync();
    }

    /// <inheritdoc/>
    public async Task<Session> GetAsync()
    {
        _ = _session ??= await GetSessionAsync();
        if (_session.CreatedAt.Add(_session.Expiration).UtcDateTime < _timeProvider.GetUtcNow())
        {
            await CloseAndRemoveActiveSessionAsync();
        }

        await SaveAsync();
        return _session;
    }

    /// <inheritdoc/>
    public async Task OpenAsync(string partitionId, string userId)
    {
        if (_session is not null || (await StateManager.TryGetStateAsync<Session>(_stateName)).HasValue)
        {
            throw new InvalidOperationException("The session is already opened.");
        }

        DateTimeOffset date = _timeProvider.GetLocalNow();
        _session = new Session(
             Id.GetId(),
             userId,
             partitionId,
             date,
             TimeSpan.FromDays(1),
             date,
             false);
        await UserActiveSessionActor(_session.UserId).AddAsync(_session.Id, date.Add(_session.Expiration));
        await SaveAsync();
    }

    private async Task CloseAndRemoveActiveSessionAsync()
    {
        _session = (_session ??= await GetSessionAsync()) with { Disabled = true };
        await UserActiveSessionActor(_session.UserId).RemoveAsync(_session.Id);
    }

    private async Task<Session> GetSessionAsync()
        => await StateManager.GetStateAsync<Session>(_stateName);

    private async Task SaveAsync()
    {
        await StateManager.SetStateAsync(
            _stateName,
            (_session ?? throw new InvalidOperationException("The state is not initialized."))
                with
            { LastActivity = _timeProvider.GetLocalNow() });
        await StateManager.SaveStateAsync();
    }

    private IUserActiveSessionActor UserActiveSessionActor(string userId)
                                    => ActorProxy.Create<IUserActiveSessionActor>(
                new Dapr.Actors.ActorId(userId),
                IUserActiveSessionActor.ActorName);
}