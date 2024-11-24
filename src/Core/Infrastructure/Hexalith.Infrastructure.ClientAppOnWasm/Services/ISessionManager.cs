// <copyright file="ISessionManager.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System.Security.Claims;
using System.Threading.Tasks;

/// <summary>
/// Interface for managing user sessions.
/// </summary>
public interface ISessionManager
{
    /// <summary>
    /// Gets the current session asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the current <see cref="ClaimsPrincipal"/>.</returns>
    Task<ClaimsPrincipal> GetCurrentSessionAsync();

    /// <summary>
    /// Signs out the current user asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous sign-out operation.</returns>
    Task SignOutAsync();
}