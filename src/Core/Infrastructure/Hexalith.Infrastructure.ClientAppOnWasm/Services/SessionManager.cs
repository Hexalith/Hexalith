namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

using Microsoft.AspNetCore.Components;

// SessionManager.cs (Client)
public class SessionManager : ISessionManager
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;
    private readonly IAccessTokenProvider _tokenProvider;

    public SessionManager(
        IAccessTokenProvider tokenProvider,
        NavigationManager navigationManager,
        HttpClient httpClient)
    {
        _tokenProvider = tokenProvider;
        _navigationManager = navigationManager;
        _httpClient = httpClient;
    }

    public async Task<ClaimsPrincipal> GetCurrentSessionAsync()
    {
        try
        {
            var tokenResult = await _tokenProvider.RequestAccessToken();
            if (tokenResult.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token.Value);

                var response = await _httpClient.GetAsync("api/session/validate");
                if (response.IsSuccessStatusCode)
                {
                    var principal = await GetUserPrincipalFromToken(token.Value);
                    return principal;
                }
            }
        }
        catch (Exception ex)
        {
            // Log error
        }

        return new ClaimsPrincipal(new ClaimsIdentity());
    }

    public async Task SignOutAsync()
    {
        try
        {
            await _httpClient.PostAsync("api/session/signout", null);
        }
        finally
        {
            _navigationManager.NavigateTo("authentication/logout");
        }
    }

    private Task<ClaimsPrincipal> GetUserPrincipalFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var identity = new ClaimsIdentity(jwtToken.Claims, "Bearer");
        return Task.FromResult(new ClaimsPrincipal(identity));
    }
}