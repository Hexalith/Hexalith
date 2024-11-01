// <copyright file="ISessionManager.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System.Security.Claims;
using System.Threading.Tasks;

// ISessionManager.cs (Client)
public interface ISessionManager
{
    Task<ClaimsPrincipal> GetCurrentSessionAsync();

    Task SignOutAsync();
}