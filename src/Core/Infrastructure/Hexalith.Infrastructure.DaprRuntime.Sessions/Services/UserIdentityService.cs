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
using Hexalith.Infrastructure.DaprRuntime.Sessions.Helpers;

/// <summary>
/// Service for managing user identity operations using Dapr actors.
/// </summary>
/// <remarks>
/// This service implements the IUserIdentityService interface using Dapr actors for state management
/// and distributed operations. Each method delegates to a UserIdentityActor instance.
/// </remarks>
public class UserIdentityService : IUserIdentityService
{
    /// <inheritdoc/>
    public async Task<UserIdentity> AddAsync(string id, string provider, string name, string email, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        IUserIdentityActor actor = (id, provider).UserIdentityActor();
        await actor.AddAsync(id, provider, name, email).ConfigureAwait(false);
        return await actor.GetAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string id, string provider, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);

        return await (id, provider)
            .UserIdentityActor()
            .ExistsAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<UserIdentity?> FindAsync(string id, string provider, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);

        return await (id, provider)
            .UserIdentityActor()
            .FindAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<UserIdentity> GetAsync(string id, string provider, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(provider);

        return await (id, provider)
            .UserIdentityActor()
            .GetAsync()
            .ConfigureAwait(false);
    }
}