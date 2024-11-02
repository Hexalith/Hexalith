// <copyright file="IUserSessionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Services;

using System.Security.Claims;
using System.Threading.Tasks;

/// <summary>
/// Interface for user session service.
/// </summary>
public interface IUserSessionService
{
    /// <summary>
    /// Creates a new session for the specified user.
    /// </summary>
    /// <param name="user">The user for whom to create the session.</param>
    /// <returns>The session ID of the created session.</returns>
    Task<string> CreateSessionAsync(ClaimsPrincipal user);

    /// <summary>
    /// Gets the active sessions for the specified user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A collection of active user sessions.</returns>
    Task<IEnumerable<UserSession>> GetActiveSessionsAsync(string userId);

    /// <summary>
    /// Gets the session for the specified session ID.
    /// </summary>
    /// <param name="sessionId">The session ID.</param>
    /// <returns>The user session, or null if not found.</returns>
    Task<UserSession?> GetSessionAsync(string sessionId);

    /// <summary>
    /// Invalidates the session for the specified session ID.
    /// </summary>
    /// <param name="sessionId">The session ID.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InvalidateSessionAsync(string sessionId);

    /// <summary>
    /// Validates the session for the specified session ID.
    /// </summary>
    /// <param name="sessionId">The session ID.</param>
    /// <returns>True if the session is valid; otherwise, false.</returns>
    Task<bool> ValidateSessionAsync(string sessionId);
}