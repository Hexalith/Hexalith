// <copyright file="UserSessionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Services;

using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

using Hexalith.Application.Sessions.Helpers;
using Hexalith.Application.Sessions.Models;
using Hexalith.Application.Sessions.Services;
using Hexalith.Extensions.Helpers;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

/// <summary>
/// Service for managing user sessions.
/// </summary>
public class SessionService : ISessionService
{
    private readonly IDistributedCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionService"/> class.
    /// </summary>
    /// <param name="cache">The distributed cache.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="timeProvider">The time provider.</param>
    public SessionService(IDistributedCache cache, IHttpContextAccessor httpContextAccessor, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(timeProvider);
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
        _timeProvider = timeProvider;
    }

    /// <inheritdoc/>
    public async Task CloseAsync(string id, CancellationToken cancellationToken)
        => await _cache.RemoveAsync(id, cancellationToken);

    /// <inheritdoc/>
    public async Task<SessionInformation?> GetAsync(string id, CancellationToken cancellationToken)
    {
        byte[]? data = await _cache.GetAsync(id, cancellationToken);
        return data == null ? null :
            JsonSerializer.Deserialize<SessionInformation>(data);
    }

    /// <inheritdoc/>
    public async Task<SessionInformation> OpenAsync(string partitionId, CancellationToken cancellationToken)
    {
        string sessionId = UniqueIdHelper.GenerateUniqueStringId();
        ClaimsPrincipal user = _httpContextAccessor.HttpContext?.User ?? throw new InvalidOperationException("User not found.");
        SessionInformation session = new(
            sessionId,
            partitionId,
            new UserInformation(
                user.GetUserId(),
                user.GetUserName(),
                user.IsGlobalAdministrator(),
                user.GetPartitionRoles()),
            new ContactInformation(
                user.GetUserId(),
                user.GetUserName(),
                user.GetUserId()),
            _timeProvider.GetLocalNow());

        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
            SlidingExpiration = TimeSpan.FromMinutes(30),
        };

        await _cache.SetAsync(
            sessionId,
            JsonSerializer.SerializeToUtf8Bytes(session),
            options,
            cancellationToken);

        return session;
    }
}