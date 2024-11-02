// <copyright file="CustomAuthenticationStateProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Authorization;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ISessionManager _sessionManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomAuthenticationStateProvider"/> class.
    /// </summary>
    /// <param name="sessionManager"></param>
    public CustomAuthenticationStateProvider(ISessionManager sessionManager) => _sessionManager = sessionManager;

    /// <inheritdoc/>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        System.Security.Claims.ClaimsPrincipal state = await _sessionManager.GetCurrentSessionAsync();
        return new AuthenticationState(state);
    }
}