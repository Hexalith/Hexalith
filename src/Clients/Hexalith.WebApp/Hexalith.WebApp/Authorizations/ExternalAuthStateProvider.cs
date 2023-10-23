// ***********************************************************************
// Assembly         : Hexalith.WebApp
// Author           : Jérôme Piquot
// Created          : 10-22-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-22-2023
// ***********************************************************************
// <copyright file="ExternalAuthStateProvider.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.WebApp.Authorizations;

using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Authorization;

/// <summary>
/// Class ExternalAuthStateProvider.
/// Implements the <see cref="AuthenticationStateProvider" />.
/// </summary>
/// <seealso cref="AuthenticationStateProvider" />
public class ExternalAuthStateProvider : AuthenticationStateProvider
{
    /// <summary>
    /// The current user.
    /// </summary>
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    /// <inheritdoc/>
    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(new AuthenticationState(_currentUser));

    /// <summary>
    /// Logs the in asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    public Task LogInAsync()
    {
        Task<AuthenticationState> loginTask = LogInAsyncCore();
        NotifyAuthenticationStateChanged(loginTask);

        return loginTask;

        async Task<AuthenticationState> LogInAsyncCore()
        {
            ClaimsPrincipal user = await LoginWithExternalProviderAsync().ConfigureAwait(false);
            _currentUser = user;

            return new AuthenticationState(_currentUser);
        }
    }

    /// <summary>
    /// Logouts this instance.
    /// </summary>
    public void Logout()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(_currentUser)));
    }

    /// <summary>
    /// Logins the with external provider asynchronous.
    /// </summary>
    /// <returns>Task&lt;ClaimsPrincipal&gt;.</returns>
    private static Task<ClaimsPrincipal> LoginWithExternalProviderAsync()
    {
        /*
            Provide OpenID/MSAL code to authenticate the user. See your identity
            provider's documentation for details.

            Return a new ClaimsPrincipal based on a new ClaimsIdentity.
        */
        ClaimsPrincipal authenticatedUser = new(new ClaimsIdentity());

        return Task.FromResult(authenticatedUser);
    }
}