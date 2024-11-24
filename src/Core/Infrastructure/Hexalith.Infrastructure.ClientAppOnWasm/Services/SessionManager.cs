// <copyright file="SessionManager.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

/// <summary>
/// Manages user sessions by handling authentication tokens and user sign-out.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SessionManager"/> class.
/// </remarks>
/// <param name="tokenProvider">The access token provider.</param>
/// <param name="navigationManager">The navigation manager.</param>
/// <param name="httpClient">The HTTP client.</param>
public class SessionManager(
    IAccessTokenProvider tokenProvider,
    NavigationManager navigationManager,
    HttpClient httpClient) : ISessionManager
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly NavigationManager _navigationManager = navigationManager;
    private readonly IAccessTokenProvider _tokenProvider = tokenProvider;

    /// <inheritdoc/>
    /// <summary>
    /// Gets the current session asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the current <see cref="ClaimsPrincipal"/>.</returns>
    public async Task<ClaimsPrincipal> GetCurrentSessionAsync()
    {
        try
        {
            AccessTokenResult tokenResult = await _tokenProvider.RequestAccessToken();
            if (tokenResult.TryGetToken(out AccessToken? token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Value);

                HttpResponseMessage response = await _httpClient.GetAsync("api/session/validate");
                if (response.IsSuccessStatusCode)
                {
                    ClaimsPrincipal principal = await GetUserPrincipalFromTokenAsync(token.Value);
                    return principal;
                }
            }
        }
        catch (Exception)
        {
            // Log error
        }

        return new ClaimsPrincipal(new ClaimsIdentity());
    }

    /// <inheritdoc/>
    /// <summary>
    /// Signs out the current user asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous sign-out operation.</returns>
    public async Task SignOutAsync()
    {
        try
        {
            _ = await _httpClient.PostAsync("api/session/signout", null);
        }
        finally
        {
            _navigationManager.NavigateTo("authentication/logout");
        }
    }

    /// <summary>
    /// Gets the user principal from the token asynchronously.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ClaimsPrincipal"/>.</returns>
    private Task<ClaimsPrincipal> GetUserPrincipalFromTokenAsync(string token)
    {
        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

        ClaimsIdentity identity = new(jwtToken.Claims, "Bearer");
        return Task.FromResult(new ClaimsPrincipal(identity));
    }
}