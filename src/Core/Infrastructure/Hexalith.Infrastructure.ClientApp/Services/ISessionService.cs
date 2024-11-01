// <copyright file="ISessionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Services;

using System.Threading.Tasks;

/// <summary>
/// Represents a service for managing user sessions.
/// </summary>
/// <remarks>
/// This service provides methods for retrieving session IDs.
/// </remarks>
public interface ISessionService
{
    Task<string> GetContactIdAsync(CancellationToken cancellationToken);

    Task<string> GetPartitionIdAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the session.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The session ID as a string.</returns>
    Task<string> GetSessionIdAsync(CancellationToken cancellationToken);
}