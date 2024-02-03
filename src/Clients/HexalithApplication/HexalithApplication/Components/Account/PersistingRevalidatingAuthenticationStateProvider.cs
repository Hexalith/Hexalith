// ***********************************************************************
// Assembly         : HexalithApplication
// Author           : Jérôme Piquot
// Created          : 01-14-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-14-2024
// ***********************************************************************
// <copyright file="PersistingRevalidatingAuthenticationStateProvider.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace HexalithApplication.Components.Account;

using System.Diagnostics;
using System.Security.Claims;

using Hexalith.Infrastructure.ClientApp.Security;

using HexalithApplication.Client;
using HexalithApplication.Data;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

// This is a server-side AuthenticationStateProvider that revalidates the security stamp for the connected user
// every 30 minutes an interactive circuit is connected. It also uses PersistentComponentState to flow the
// authentication state to the client which is then fixed for the lifetime of the WebAssembly application.

/// <summary>
/// Class PersistingRevalidatingAuthenticationStateProvider. This class cannot be inherited.
/// Implements the <see cref="RevalidatingServerAuthenticationStateProvider" />.
/// </summary>
/// <seealso cref="RevalidatingServerAuthenticationStateProvider" />
internal sealed class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
    /// <summary>
    /// The options.
    /// </summary>
    private readonly IdentityOptions _options;

    /// <summary>
    /// The scope factory.
    /// </summary>
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// The state.
    /// </summary>
    private readonly PersistentComponentState _state;

    /// <summary>
    /// The subscription.
    /// </summary>
    private readonly PersistingComponentStateSubscription _subscription;

    /// <summary>
    /// The authentication state task.
    /// </summary>
    private Task<AuthenticationState>? _authenticationStateTask;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistingRevalidatingAuthenticationStateProvider"/> class.
    /// </summary>
    /// <param name="loggerFactory">The logger factory.</param>
    /// <param name="serviceScopeFactory">The service scope factory.</param>
    /// <param name="persistentComponentState">State of the persistent component.</param>
    /// <param name="optionsAccessor">The options accessor.</param>
    public PersistingRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory serviceScopeFactory,
        PersistentComponentState persistentComponentState,
        IOptions<IdentityOptions> optionsAccessor)
        : base(loggerFactory)
    {
        _scopeFactory = serviceScopeFactory;
        _state = persistentComponentState;
        _options = optionsAccessor.Value;

        AuthenticationStateChanged += OnAuthenticationStateChanged;
        _subscription = _state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
    }

    /// <summary>
    /// Gets the interval between revalidation attempts.
    /// </summary>
    /// <value>The revalidation interval.</value>
    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        _subscription.Dispose();
        AuthenticationStateChanged -= OnAuthenticationStateChanged;
        base.Dispose(disposing);
    }

    /// <summary>
    /// Validate authentication state as an asynchronous operation.
    /// </summary>
    /// <param name="authenticationState">The current <see cref="AuthenticationState" />.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while performing the operation.</param>
    /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        // Get the user manager from a new scope to ensure it fetches fresh data
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
        UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        return await ValidateSecurityStampAsync(userManager, authenticationState.User).ConfigureAwait(false);
    }

    /// <summary>
    /// Called when [authentication state changed].
    /// </summary>
    /// <param name="task">The task.</param>
    private void OnAuthenticationStateChanged(Task<AuthenticationState> task) => _authenticationStateTask = task;

    /// <summary>
    /// On persisting as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="System.Diagnostics.UnreachableException">Authentication state not set in {nameof(OnPersistingAsync)}().</exception>
    private async Task OnPersistingAsync()
    {
        if (_authenticationStateTask is null)
        {
            throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");
        }

#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
        AuthenticationState authenticationState = await _authenticationStateTask.ConfigureAwait(false);
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
        ClaimsPrincipal principal = authenticationState.User;

        if (principal.Identity?.IsAuthenticated == true)
        {
            string? userId = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
            string? email = principal.FindFirst(_options.ClaimsIdentity.EmailClaimType)?.Value;

            if (userId != null && email != null)
            {
                _state.PersistAsJson(nameof(UserInfo), new UserInfo
                {
                    UserId = userId,
                    Email = email,
                });
            }
        }
    }

    /// <summary>
    /// Validate security stamp as an asynchronous operation.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="principal">The principal.</param>
    /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
    private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
    {
        ApplicationUser? user = await userManager.GetUserAsync(principal).ConfigureAwait(false);
        if (user is null)
        {
            return false;
        }
        else if (!userManager.SupportsUserSecurityStamp)
        {
            return true;
        }
        else
        {
            string? principalStamp = principal.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType);
            string userStamp = await userManager.GetSecurityStampAsync(user).ConfigureAwait(false);
            return principalStamp == userStamp;
        }
    }
}