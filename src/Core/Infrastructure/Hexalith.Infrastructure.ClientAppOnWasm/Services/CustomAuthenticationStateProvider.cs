namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Authorization;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ISessionManager _sessionManager;

    public CustomAuthenticationStateProvider(ISessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var state = await _sessionManager.GetCurrentSessionAsync();
        return new AuthenticationState(state);
    }
}
