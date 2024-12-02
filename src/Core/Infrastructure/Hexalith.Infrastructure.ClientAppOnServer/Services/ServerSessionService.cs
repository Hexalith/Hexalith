// <copyright file="ServerSessionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Services;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Sessions.Models;
using Hexalith.Application.Sessions.Services;
using Hexalith.Extensions.Helpers;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Service to manage server sessions.
/// </summary>
public class ServerSessionService : ISessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TimeProvider _timeProvider;
    private readonly IUserPartitionService _userPartitionService;
    private string? _partitionId;
    private string? _sessionId;
    private string? _userId;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerSessionService"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="userPartitionService">The user partition service.</param>
    /// <param name="timeProvider">The time provider.</param>
    public ServerSessionService(
        IHttpContextAccessor httpContextAccessor,
        IUserPartitionService userPartitionService,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(userPartitionService);
        ArgumentNullException.ThrowIfNull(timeProvider);
        _httpContextAccessor = httpContextAccessor;
        _userPartitionService = userPartitionService;
        _timeProvider = timeProvider;
    }

    /// <inheritdoc/>
    public async Task<SessionInformation> GetAsync(string userId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        if (_httpContextAccessor.HttpContext is not null)
        {
            await _httpContextAccessor.HttpContext.Session.LoadAsync(cancellationToken);
            string? sessionId = _httpContextAccessor.HttpContext.Session.GetString(nameof(_sessionId));
            string? user = _httpContextAccessor.HttpContext.Session.GetString(nameof(_userId));
            string? partitionId = _httpContextAccessor.HttpContext.Session.GetString(nameof(_partitionId));
            if (string.IsNullOrWhiteSpace(sessionId) && string.IsNullOrWhiteSpace(_partitionId) && _userId == userId)
            {
                _sessionId = sessionId;
                _userId = user;
                _partitionId = partitionId;
            }
        }

        if (string.IsNullOrWhiteSpace(_sessionId) || _userId != userId)
        {
            _sessionId = UniqueIdHelper.GenerateUniqueStringId();
            _userId = userId;
            _partitionId = null;
        }

        if (string.IsNullOrWhiteSpace(_partitionId))
        {
            _partitionId = await _userPartitionService.GetDefaultPartitionAsync(userId, cancellationToken);
        }

        if (string.IsNullOrWhiteSpace(_partitionId))
        {
            throw new InvalidOperationException("No partition found for the user.");
        }

        if (_httpContextAccessor.HttpContext is not null)
        {
            _httpContextAccessor.HttpContext.Session.SetString(nameof(_sessionId), _sessionId);
            _httpContextAccessor.HttpContext.Session.SetString(nameof(_userId), _userId);
            _httpContextAccessor.HttpContext.Session.SetString(nameof(_partitionId), _partitionId);
        }

        return new SessionInformation(_sessionId, _userId, _partitionId, _timeProvider.GetLocalNow());
    }

    /// <inheritdoc/>
    public async Task SetCurrentPartitionAsync(string userId, string partitionId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(partitionId);
        _ = await GetAsync(userId, cancellationToken);
        if (await _userPartitionService.InPartitionAsync(userId, partitionId, cancellationToken))
        {
            _partitionId = partitionId;
            _httpContextAccessor.HttpContext?.Session.SetString(nameof(_partitionId), _partitionId);
        }
    }
}