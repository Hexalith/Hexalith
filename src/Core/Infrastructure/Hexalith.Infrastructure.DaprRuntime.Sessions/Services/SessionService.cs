// <copyright file="SessionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Services;

using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Hexalith.Application.Partitions.Services;
using Hexalith.Application.Sessions.Helpers;
using Hexalith.Application.Sessions.Models;
using Hexalith.Application.Sessions.Services;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Sessions.Helpers;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

/// <summary>
/// Service for managing user sessions.
/// </summary>
public partial class SessionService : ISessionService
{
    private readonly HybridCache _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<SessionService> _logger;
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
    /// <param name="logger">The logger.</param>
    public SessionService(
        HybridCache cache,
        IHttpContextAccessor httpContextAccessor,
        IPartitionService partitionService,
        IUserIdentityService userService,
        TimeProvider timeProvider,
        ILogger<SessionService> logger)
    {
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(partitionService);
        ArgumentNullException.ThrowIfNull(userService);
        ArgumentNullException.ThrowIfNull(logger);
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
        _partitionService = partitionService;
        _userService = userService;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    /// <summary>
    /// Logs a warning message when a user identity has no email.
    /// This method is generated using the LoggerMessage attribute for high performance.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="provider">The identity provider.</param>
    /// <param name="name">The user name.</param>
    [LoggerMessage(EventId = 1, Level = LogLevel.Warning, Message = "User identity '{Name}' with ID '{UserId}' and provider '{Provider}' has no email.")]
    public static partial void LogUserIdentityNoEmailWarning(ILogger logger, string userId, string provider, string name);

    /// <inheritdoc/>
    public async Task CloseAsync(string id, CancellationToken cancellationToken)
        => await _cache.RemoveAsync(id, cancellationToken);

    /// <inheritdoc/>
    public async Task<SessionInformation?> GetAsync(string id, CancellationToken cancellationToken)
    {
        SessionInformation data = await _cache.GetOrCreateAsync(
            id,
            (token) => ValueTask.FromResult(new SessionInformation(
                string.Empty,
                string.Empty,
                new UserInformation(string.Empty, string.Empty, string.Empty, false, []),
                new ContactInformation(string.Empty, string.Empty, string.Empty),
                DateTime.MinValue,
                TimeSpan.Zero)),
            cancellationToken: cancellationToken);
        return data.Id != id ? null : data;
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
            string userName = user.GetUserName();
            if (email == null)
            {
                // Log a warning if the user identity has no email
                LogUserIdentityNoEmailWarning(_logger, userId, provider, userName);
                return null;
            }

            // Add a new user identity if not found
            identity = await _userService.AddAsync(userId, provider, userName, email, cancellationToken);
        }

        // Open a new session actor
        await sessionId.SessionActor().OpenAsync(partitionId, userId, provider);

        // Create session information
        SessionInformation session = new(
            sessionId,
            partitionId,
            new UserInformation(
                identity.Id,
                identity.Provider,
                identity.Name,
                identity.IsGlobalAdministrator,
                user.GetPartitionRoles(partitionId)),
            new ContactInformation(
                identity.Id,
                identity.Email,
                identity.Name),
            _timeProvider.GetLocalNow(),
            TimeSpan.FromDays(1));

        // Store session information in the cache
        await _cache.SetAsync(
            sessionId,
            session,
            cancellationToken: cancellationToken);

        return session;
    }
}