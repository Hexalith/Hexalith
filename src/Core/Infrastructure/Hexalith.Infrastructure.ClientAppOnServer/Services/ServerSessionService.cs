// <copyright file="ServerSessionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Services;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Partitions.Models;
using Hexalith.Application.Partitions.Services;
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
    private readonly IPartitionService _partitionService;
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
    /// <param name="partitionService"></param>
    /// <param name="timeProvider">The time provider.</param>
    public ServerSessionService(
        IHttpContextAccessor httpContextAccessor,
        IUserPartitionService userPartitionService,
        IPartitionService partitionService,
        TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(userPartitionService);
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(partitionService);
        _httpContextAccessor = httpContextAccessor;
        _userPartitionService = userPartitionService;
        _partitionService = partitionService;
        _timeProvider = timeProvider;
    }

    /// <inheritdoc/>
    public async Task<SessionInformation> GetAsync(string userId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        try
        {
            if (_httpContextAccessor.HttpContext?.Session is ISession session)
            {
                await session.LoadAsync(cancellationToken);
                string? sessionId = session.GetString(nameof(_sessionId));
                string? user = session.GetString(nameof(_userId));
                string? partitionId = session.GetString(nameof(_partitionId));
                if (!string.IsNullOrWhiteSpace(sessionId) && !string.IsNullOrWhiteSpace(partitionId) && user == userId)
                {
                    _sessionId = sessionId;
                    _userId = user;
                    _partitionId = partitionId;
                    return new SessionInformation(_sessionId, _userId, _partitionId, _timeProvider.GetLocalNow());
                }
            }
        }
        catch (InvalidOperationException)
        {
            // Session is not available or response has already started
            // Continue with in-memory values
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
            await foreach (Partition partitionId in _partitionService.GetAllAsync(cancellationToken))
            {
                // Partitions exist in the system. The administrator must assign a partition to the user.
                throw new InvalidOperationException("No partition found for the user. Ask the administrator to assign a partition to the user.");
            }

            // No partition found in the system. Create a new one with the user domain name.
            await _partitionService.AddAsync(new Partition(nameof(Hexalith), nameof(Hexalith), false), cancellationToken);
            _partitionId = nameof(Hexalith);
        }

        try
        {
            if (_httpContextAccessor.HttpContext?.Session is ISession session)
            {
                session.SetString(nameof(_sessionId), _sessionId);
                session.SetString(nameof(_userId), _userId);
                session.SetString(nameof(_partitionId), _partitionId);
            }
        }
        catch (InvalidOperationException)
        {
            // Session is not available or response has already started
            // Continue with in-memory values
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