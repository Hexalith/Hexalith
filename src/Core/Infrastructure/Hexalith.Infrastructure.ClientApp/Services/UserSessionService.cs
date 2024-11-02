// <copyright file="UserSessionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Services;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Distributed;

public class UserSessionService(IDistributedCache cache) : IUserSessionService
{
    private readonly IDistributedCache _cache = cache;

    /// <inheritdoc/>
    public async Task<string> CreateSessionAsync(ClaimsPrincipal user)
    {
        string sessionId = Guid.NewGuid().ToString();
        UserSession session = new()
        {
            Id = sessionId,
            UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
            Username = user.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty,
            CreatedAt = DateTime.UtcNow,
            LastActivity = DateTime.UtcNow,
        };

        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
            SlidingExpiration = TimeSpan.FromMinutes(30),
        };

        await _cache.SetAsync(
            sessionId,
            JsonSerializer.SerializeToUtf8Bytes(session),
            options);

        return sessionId;
    }

    /// <inheritdoc/>
    public Task<IEnumerable<UserSession>> GetActiveSessionsAsync(string userId) => throw new NotImplementedException();

    /// <inheritdoc/>
    public async Task<UserSession?> GetSessionAsync(string sessionId)
    {
        byte[]? data = await _cache.GetAsync(sessionId);
        return data == null ? null :
            JsonSerializer.Deserialize<UserSession>(data);
    }

    /// <inheritdoc/>
    public async Task InvalidateSessionAsync(string sessionId) => await _cache.RemoveAsync(sessionId);

    /// <inheritdoc/>
    public async Task<bool> ValidateSessionAsync(string sessionId)
    {
        UserSession session = await GetSessionAsync(sessionId);
        if (session == null)
        {
            return false;
        }

        session.LastActivity = DateTime.UtcNow;
        await UpdateSessionAsync(session);
        return true;
    }

    private async Task UpdateSessionAsync(UserSession session)
    {
        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
            SlidingExpiration = TimeSpan.FromMinutes(30),
        };

        await _cache.SetAsync(
            session.Id,
            JsonSerializer.SerializeToUtf8Bytes(session),
            options);
    }
}