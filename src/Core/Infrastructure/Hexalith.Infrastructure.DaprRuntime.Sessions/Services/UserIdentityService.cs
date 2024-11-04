// <copyright file="UserIdentityService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Services;

using System;
using System.Threading.Tasks;

using Hexalith.Application.Sessions.Models;
using Hexalith.Application.Sessions.Services;
using Hexalith.Infrastructure.DaprRuntime.Sessions.Actors;

/// <summary>
/// Service for managing user identity operations using Dapr actors.
/// </summary>
/// <param name="provider">The identity provider name used for all operations.</param>
/// <remarks>
/// This service implements the IUserIdentityService interface using Dapr actors for state management
/// and distributed operations. Each method delegates to a UserIdentityActor instance.
/// </remarks>
public class UserIdentityService(string provider) : IUserIdentityService
{
    /// <inheritdoc/>
    public async Task<UserIdentity> AddAsync(string id, string provider, string name, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        IUserIdentityActor actor = IUserIdentityActor.Actor(id, provider);
        await actor.AddAsync(id, provider, name, email);
        return await actor.GetAsync();
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await IUserIdentityActor
            .Actor(id, provider)
            .ExistsAsync();
    }

    /// <inheritdoc/>
    public async Task<UserIdentity?> FindAsync(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await IUserIdentityActor
            .Actor(id, provider)
            .FindAsync();
    }

    /// <inheritdoc/>
    public async Task<UserIdentity> GetAsync(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await IUserIdentityActor
            .Actor(id, provider)
            .GetAsync();
    }
}