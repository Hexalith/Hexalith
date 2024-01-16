// ***********************************************************************
// Assembly         : HexalithApplication.Client
// Author           : Jérôme Piquot
// Created          : 01-14-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-14-2024
// ***********************************************************************
// <copyright file="PersistentAuthenticationStateProvider.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace HexalithApplication.Client;

using System.Security.Claims;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

// This is a client-side AuthenticationStateProvider that determines the user's authentication state by
// looking for data persisted in the page when it was rendered on the server. This authentication state will
// be fixed for the lifetime of the WebAssembly application. So, if the user needs to log in or out, a full
// page reload is required.
//
// This only provides a user name and email for display purposes. It does not actually include any tokens
// that authenticate to the server when making subsequent requests. That works separately using a
// cookie that will be included on HttpClient requests to the server.

/// <summary>
/// Class PersistentAuthenticationStateProvider.
/// Implements the <see cref="AuthenticationStateProvider" />.
/// </summary>
/// <seealso cref="AuthenticationStateProvider" />
internal class PersistentAuthenticationStateProvider : AuthenticationStateProvider
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
    /// Initializes a new instance of the <see cref="PersistentAuthenticationStateProvider"/> class.
    /// </summary>
    /// <param name="state">The state.</param>
    public PersistentAuthenticationStateProvider(PersistentComponentState state)
    {
        if (!state.TryTakeFromJson<UserInfo>(nameof(UserInfo), out UserInfo? userInfo) || userInfo is null)
        {
            return;
        }

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
            new Claim(ClaimTypes.Name, userInfo.Email),
            new Claim(ClaimTypes.Email, userInfo.Email)];

        _authenticationStateTask = Task.FromResult(
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
                claims,
                authenticationType: nameof(PersistentAuthenticationStateProvider)))));
    }

    /// <summary>
    /// Gets the authentication state asynchronous.
    /// </summary>
    /// <returns>System.Threading.Tasks.Task&lt;Microsoft.AspNetCore.Components.Authorization.AuthenticationState&gt;.</returns>
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks

    public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authenticationStateTask;

#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
}