// <copyright file="SessionIdService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Services;

using System.Threading.Tasks;

using Hexalith.Infrastructure.ClientApp.Services;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Service for retrieving session IDs from the server-side HTTP context.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SessionIdService"/> class.
/// </remarks>
/// <param name="httpContextAccessor">The HTTP context accessor instance.</param>
public class SessionIdService(IHttpContextAccessor httpContextAccessor) : ISessionIdService
{
    /// <inheritdoc/>
    public Task<string?> GetSessionIdAsync() => Task.FromResult(httpContextAccessor.HttpContext?.Session.Id);
}