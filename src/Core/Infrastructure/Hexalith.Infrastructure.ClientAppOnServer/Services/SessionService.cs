﻿// <copyright file="SessionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Services;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.ClientApp.Services;

/// <summary>
/// Represents a service for managing user sessions.
/// </summary>
public class SessionService : ISessionService
{
    /// <inheritdoc/>
    public Task<string> GetSessionIdAsync(CancellationToken cancellationToken) => Task.FromResult(UniqueIdHelper.GenerateUniqueStringId());
    /// <inheritdoc/>
    public Task<string> GetPartitionIdAsync(CancellationToken cancellationToken) => Task.FromResult("PART1");
}