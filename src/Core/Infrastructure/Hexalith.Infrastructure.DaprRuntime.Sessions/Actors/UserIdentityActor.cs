// <copyright file="UserIdentityActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Actors;

using System.Threading.Tasks;

using Dapr.Actors.Runtime;

using Hexalith.Application.Sessions.Models;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Sessions.Helpers;

/// <summary>
/// Represents an actor that manages user identity state and operations in a distributed actor system.
/// </summary>
/// <param name="host">The Dapr actor host that provides the runtime context for the actor.</param>
/// <remarks>
/// This actor provides functionality to manage user identity lifecycle including creation,
/// enabling/disabling, and retrieval of user identity information. It uses the Dapr actor
/// state management to persist user identity data.
/// </remarks>
public class UserIdentityActor(ActorHost host) : Actor(host), IUserIdentityActor
{
    /// <summary>
    /// The state name used for storing user identity in the actor's state manager.
    /// </summary>
    private const string _stateName = "State";

    /// <summary>
    /// Cached instance of the user identity to avoid repeated state lookups.
    /// </summary>
    private UserIdentity? _user;

    /// <inheritdoc/>
    public async Task AddAsync(string id, string provider, string name, string email)
    {
        if (await ExistsAsync())
        {
            throw new InvalidOperationException("The user already exists.");
        }

        int count = await ActorProxyHelper.AllUserIdentityIdsActor.AddAsync(Id.ToUnescapeString());

        _user = new UserIdentity(
             id,
             provider,
             name,
             email,
             count < 2,
             false);
        await StateManager.AddStateAsync(_stateName, _user);
    }

    /// <inheritdoc/>
    public async Task DisableAsync()
    {
        UserIdentity currentUser = await GetAsync();
        if (!currentUser.Disabled)
        {
            currentUser.Disabled = true;
            _user = currentUser;
            await SaveAsync();
        }
    }

    /// <inheritdoc/>
    public async Task EnableAsync()
    {
        UserIdentity currentUser = await GetAsync();
        if (currentUser.Disabled)
        {
            currentUser.Disabled = false;
            _user = currentUser;
            await SaveAsync();
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync()
        => _user is not null || await StateManager.ContainsStateAsync(_stateName);

    /// <inheritdoc/>
    public async Task<UserIdentity?> FindAsync()
    {
        if (_user is not null)
        {
            return _user;
        }

        ConditionalValue<UserIdentity> result = await StateManager.TryGetStateAsync<UserIdentity>(_stateName);
        return result.HasValue
            ? result.Value
            : null;
    }

    /// <inheritdoc/>
    public async Task<UserIdentity> GetAsync()
        => _user ??= await GetUserAsync();

    /// <summary>
    /// Retrieves the user identity from the actor's state manager.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user identity.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the user identity does not exist in the state manager.</exception>
    private async Task<UserIdentity> GetUserAsync()
        => await StateManager.GetStateAsync<UserIdentity>(_stateName);

    /// <summary>
    /// Saves the current user identity state to the actor's state manager.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    private async Task SaveAsync()
    {
        await StateManager.SetStateAsync(
            _stateName,
            _user);
        await StateManager.SaveStateAsync();
    }
}