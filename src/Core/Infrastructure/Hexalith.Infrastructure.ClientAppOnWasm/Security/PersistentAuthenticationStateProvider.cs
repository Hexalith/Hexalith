// <copyright file="PersistentAuthenticationStateProvider.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Security;

using System.Security.Claims;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

/// <summary>
/// Class PersistentAuthenticationStateProvider.
/// Implements the <see cref="AuthenticationStateProvider" />.
/// </summary>
/// <seealso cref="AuthenticationStateProvider" />
public class PersistingRevalidatingAuthenticationStateProvider : AuthenticationStateProvider
{
    /// <summary>
    /// The default unauthenticated task.
    /// </summary>
    private static readonly Task<AuthenticationState> _defaultUnauthenticatedTask =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    /// <summary>
    /// The authentication state task.
    /// </summary>
    private readonly Task<AuthenticationState> _authenticationStateTask = _defaultUnauthenticatedTask;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistingRevalidatingAuthenticationStateProvider"/> class.
    /// </summary>
    /// <param name="state">The state.</param>
    public PersistingRevalidatingAuthenticationStateProvider(PersistentComponentState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        if (!state.TryTakeFromJson(nameof(UserInfo), out UserInfo? userInfo) || userInfo is null)
        {
            return;
        }

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, userInfo.Id),
            new Claim(ClaimTypes.Name, userInfo.Name),
            new Claim(ClaimTypes.Email, userInfo.Email)];

        _authenticationStateTask = Task.FromResult(
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
                claims,
                authenticationType: nameof(PersistingRevalidatingAuthenticationStateProvider)))));
    }

    /// <summary>
    /// Gets the authentication state.
    /// </summary>
    /// <returns>System.Threading.Tasks.Task&lt;Microsoft.AspNetCore.Components.Authorization.AuthenticationState&gt;.</returns>
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        => await _authenticationStateTask.ConfigureAwait(true);

#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
}