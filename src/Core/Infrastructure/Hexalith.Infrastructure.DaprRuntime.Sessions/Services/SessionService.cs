// <copyright file="SessionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Services;

using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

using Hexalith.Application.Partitions.Services;
using Hexalith.Application.Sessions.Helpers;
using Hexalith.Application.Sessions.Models;
using Hexalith.Application.Sessions.Services;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Sessions.Actors;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

/// <summary>
/// Service for managing user sessions.
/// </summary>
public class SessionService : ISessionService
{
    private readonly IDistributedCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPartitionService _partitionService;
    private readonly TimeProvider _timeProvider;
    private readonly IUserIdentityService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionService"/> class.
    /// </summary>
    /// <param name="cache">The distributed cache.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="partitionService">The partition service.</param>
    /// <param name="userService">The user service.</param>
    /// <param name="timeProvider">The time provider.</param>
    public SessionService(
        IDistributedCache cache,
        IHttpContextAccessor httpContextAccessor,
        IPartitionService partitionService,
        IUserIdentityService userService,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(partitionService);
        ArgumentNullException.ThrowIfNull(userService);
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
        _partitionService = partitionService;
        _userService = userService;
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
    public async Task<SessionInformation> OpenAsync(string? partitionId, CancellationToken cancellationToken)
    {
        ClaimsPrincipal user = _httpContextAccessor.HttpContext?.User ?? throw new InvalidOperationException("User not found.");
        string userId = user.GetUserId();
        string userName = user.GetUserName();
        string provider = user.GetIdentityProvider();
        string sessionId = UniqueIdHelper.GenerateUniqueStringId();
        ISessionActor actor = ISessionActor.Actor(sessionId);
        if (string.IsNullOrWhiteSpace(partitionId))
        {
            partitionId = await _partitionService.DefaultAsync(cancellationToken);
        }

        _ = await _userService.FindAsync(userId);
        await actor.OpenAsync(partitionId, userId, userName);

        SessionInformation session = new(
            sessionId,
            partitionId,
            new UserInformation(
                userId,
                provider,
                userName,
                user.IsGlobalAdministrator(),
                user.GetPartitionRoles()),
            new ContactInformation(
                user.GetUserId(),
                user.GetUserEmail() ?? string.Empty,
                user.GetUserName()),
            _timeProvider.GetLocalNow(),
            TimeSpan.FromDays(1));

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
