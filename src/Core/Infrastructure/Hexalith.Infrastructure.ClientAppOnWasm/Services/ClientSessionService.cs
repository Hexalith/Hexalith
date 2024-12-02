// <copyright file="ClientSessionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Sessions.Models;
using Hexalith.Application.Sessions.Services;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Service for retrieving session IDs from client-side cookies in a WASM application.
/// </summary>
public class ClientSessionService : ISessionService
{
    private readonly TimeProvider _timeProvider;
    private readonly IUserPartitionService _userPartitionService;
    private string? _partitionId;
    private string? _sessionId;
    private string? _userId;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientSessionService"/> class.
    /// </summary>
    /// <param name="userPartitionService">The user partition service.</param>
    /// <param name="timeProvider">The time provider.</param>
    public ClientSessionService(IUserPartitionService userPartitionService, TimeProvider timeProvider)
    {
        ArgumentNullException.ThrowIfNull(userPartitionService);
        ArgumentNullException.ThrowIfNull(timeProvider);
        _userPartitionService = userPartitionService;
        _timeProvider = timeProvider;
    }

    /// <inheritdoc/>
    public async Task<SessionInformation> GetAsync(string userId, CancellationToken cancellationToken)
    {
        if (_sessionId is null || _userId != userId)
        {
            _sessionId = UniqueIdHelper.GenerateUniqueStringId();
            _userId = userId;
            _partitionId = null;
        }

        _partitionId ??= await _userPartitionService.GetDefaultPartitionAsync(userId, cancellationToken);
        return _partitionId is null
            ? throw new InvalidOperationException("No partition found for the user.")
            : new SessionInformation(_sessionId, _userId, _partitionId, _timeProvider.GetLocalNow());
    }

    /// <inheritdoc/>
    public async Task SetCurrentPartitionAsync(string userId, string partitionId, CancellationToken cancellationToken)
        => _partitionId = await _userPartitionService.InPartitionAsync(userId, partitionId, cancellationToken) ? partitionId : null;
}