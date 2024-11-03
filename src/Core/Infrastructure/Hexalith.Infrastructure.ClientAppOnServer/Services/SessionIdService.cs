// <copyright file="SessionIdService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Services;

using System.Threading.Tasks;

using Hexalith.Application.Sessions;
using Hexalith.Application.Sessions.Helpers;
using Hexalith.Application.Sessions.Services;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Service for retrieving session IDs from the server-side HTTP context.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SessionIdService"/> class.
/// </remarks>
/// <param name="httpContextAccessor">The HTTP context accessor instance.</param>
/// <param name="timeProvider">The time provider instance.</param>
public class SessionIdService(IHttpContextAccessor httpContextAccessor, TimeProvider timeProvider) : ISessionIdService
{
    /// <inheritdoc/>
    public Task<string?> GetSessionIdAsync()
    {
        string? sessionId = httpContextAccessor.HttpContext?.Session.GetString(SessionConstants.SessionIdName);
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            return Task.FromResult<string?>(null);
        }

        int? expiration = httpContextAccessor.HttpContext?.Session.GetInt32(SessionConstants.SessionExpirationName);
        return expiration.HasExpired(timeProvider.GetLocalNow())
            ? Task.FromResult<string?>(null)
            : Task.FromResult(httpContextAccessor.HttpContext?.Session.GetString(SessionConstants.SessionIdName));
    }
}