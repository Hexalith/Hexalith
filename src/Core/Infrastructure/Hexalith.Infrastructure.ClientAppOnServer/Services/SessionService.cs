// <copyright file="SessionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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
    public Task<string> GetContactIdAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

    /// <inheritdoc/>
    public Task<string> GetPartitionIdAsync(CancellationToken cancellationToken) => Task.FromResult("PART1");

    /// <inheritdoc/>
    public Task<string> GetSessionIdAsync(CancellationToken cancellationToken) => Task.FromResult(UniqueIdHelper.GenerateUniqueStringId());
}