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
using Hexalith.Infrastructure.DaprRuntime.Sessions.Helpers;

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
    public async Task<SessionInformation?> OpenAsync(string? partitionId, CancellationToken cancellationToken)
    {
        ClaimsPrincipal user = _httpContextAccessor.HttpContext?.User ?? throw new InvalidOperationException("User not found.");
        string userId = user.GetUserId();
        string provider = user.GetIdentityProvider();
        string sessionId = UniqueIdHelper.GenerateUniqueStringId();
        if (string.IsNullOrWhiteSpace(partitionId))
        {
            partitionId = await _partitionService.DefaultAsync(cancellationToken);
        }

        UserIdentity? identity = await _userService.FindAsync(userId, provider, cancellationToken);
        if (identity == null)
        {
            string? email = user.FindUserEmail();
            if (email == null)
            {
                return null;
            }

            string userName = user.GetUserName();
            identity = await _userService.AddAsync(userId, provider, userName, email, cancellationToken);
        }

        await sessionId.SessionActor().OpenAsync(partitionId, userId, provider);

        SessionInformation session = new(
            sessionId,
            partitionId,
            new UserInformation(
                identity.Id,
                identity.Provider,
                identity.Name,
                identity.IsGlobalAdministrator,
                user.GetPartitionRoles()),
            new ContactInformation(
                identity.Id,
                identity.Email,
                identity.Name),
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