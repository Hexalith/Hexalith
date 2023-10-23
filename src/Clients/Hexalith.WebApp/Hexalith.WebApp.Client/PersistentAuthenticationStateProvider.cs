// ***********************************************************************
// Assembly         : Hexalith.WebApp.Client
// Author           : Jérôme Piquot
// Created          : 10-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-22-2023
// ***********************************************************************
// <copyright file="PersistentAuthenticationStateProvider.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.WebApp.Client;

using System.Security.Claims;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

/// <summary>
/// Class PersistentAuthenticationStateProvider.
/// Implements the <see cref="AuthenticationStateProvider" />.
/// </summary>
/// <seealso cref="AuthenticationStateProvider" />
public class PersistentAuthenticationStateProvider(PersistentComponentState persistentState) : AuthenticationStateProvider
{
    /// <summary>
    /// The unauthenticated task.
    /// </summary>
    private static readonly Task<AuthenticationState> _unauthenticatedTask =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    /// <summary>
    /// Gets the authentication state asynchronous.
    /// </summary>
    /// <returns>System.Threading.Tasks.Task&lt;Microsoft.AspNetCore.Components.Authorization.AuthenticationState&gt;.</returns>
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!persistentState.TryTakeFromJson<UserInfo>(nameof(UserInfo), out UserInfo? userInfo) || userInfo is null)
        {
            return _unauthenticatedTask;
        }

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
            new Claim(ClaimTypes.Name, userInfo.Email),
            new Claim(ClaimTypes.Email, userInfo.Email)];

        return Task.FromResult(
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
                claims,
                authenticationType: nameof(PersistentAuthenticationStateProvider)))));
    }
}