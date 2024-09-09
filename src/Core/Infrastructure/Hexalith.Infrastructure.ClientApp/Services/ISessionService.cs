// <copyright file="ISessionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
    /// <summary>
    /// Retrieves the session.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The session ID as a string.</returns>
    Task<string> GetSessionIdAsync(CancellationToken cancellationToken);
}