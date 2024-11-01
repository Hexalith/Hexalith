// <copyright file="IUserSessionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Services;

using System.Security.Claims;
using System.Threading.Tasks;

public interface IUserSessionService
{
    Task<string> CreateSessionAsync(ClaimsPrincipal user);

    Task<UserSession?> GetSessionAsync(string sessionId);

    Task InvalidateSessionAsync(string sessionId);

    Task<bool> ValidateSessionAsync(string sessionId);
}