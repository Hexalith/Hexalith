// <copyright file="PersistingRevalidatingAuthenticationStateProvider.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Security;

using System.Diagnostics;
using System.Security.Claims;

using Hexalith.Infrastructure.Security.Abstractions.Models;
using Hexalith.UI.Authentications.ViewModels;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// This is a server-side AuthenticationStateProvider that revalidates the security stamp for the connected user
// every 30 minutes an interactive circuit is connected. It also uses PersistentComponentState to flow the
// authentication state to the client which is then fixed for the lifetime of the WebAssembly application.
internal sealed class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
	private readonly IdentityOptions _options;
	private readonly IServiceScopeFactory _scopeFactory;
	private readonly PersistentComponentState _state;
	private readonly PersistingComponentStateSubscription _subscription;

	private Task<AuthenticationState>? _authenticationStateTask;

	/// <summary>
	/// Initializes a new instance of the <see cref="PersistingRevalidatingAuthenticationStateProvider"/> class.
	/// </summary>
	/// <param name="loggerFactory"></param>
	/// <param name="serviceScopeFactory"></param>
	/// <param name="persistentComponentState"></param>
	/// <param name="optionsAccessor"></param>
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

	/// <inheritdoc/>
	protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		_subscription.Dispose();
		AuthenticationStateChanged -= OnAuthenticationStateChanged;
		base.Dispose(disposing);
	}

	/// <inheritdoc/>
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

	private void OnAuthenticationStateChanged(Task<AuthenticationState> task) => _authenticationStateTask = task;

	private async Task OnPersistingAsync()
	{
		if (_authenticationStateTask is null)
		{
			throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");
		}

#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
		AuthenticationState authenticationState = await _authenticationStateTask;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
		ClaimsPrincipal principal = authenticationState.User;

		if (principal.Identity?.IsAuthenticated == true)
		{
			string? userId = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
			string? email = principal.FindFirst(_options.ClaimsIdentity.EmailClaimType)?.Value;

			if (userId != null && email != null)
			{
				_state.PersistAsJson(nameof(UserViewModel), new UserViewModel
				{
					Id = userId,
					Email = email,
				});
			}
		}
	}

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