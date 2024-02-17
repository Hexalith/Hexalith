// <copyright file="IdentityComponentsEndpointRouteBuilderExtensions.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnServer.Security;

using System.Security.Claims;
using System.Text.Json;

using Hexalith.Infrastructure.Security.Abstractions.Models;
using Hexalith.UI.Authentications.Components.Account.Pages;
using Hexalith.UI.Authentications.Components.Account.Pages.Manage;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

public static class IdentityComponentsEndpointRouteBuilderExtensions
{
	// These endpoints are required by the Identity Razor components defined in the /Components/Account/Pages directory of this project.
	public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
	{
		ArgumentNullException.ThrowIfNull(endpoints);

		RouteGroupBuilder accountGroup = endpoints.MapGroup("/Account");

		_ = accountGroup.MapPost("/PerformExternalLogin", (
			HttpContext context,
			[FromServices] SignInManager<ApplicationUser> signInManager,
			[FromForm] string provider,
			[FromForm] string returnUrl) =>
		{
			IEnumerable<KeyValuePair<string, StringValues>> query = [
				new("ReturnUrl", returnUrl),
				new("Action", ExternalLogin.LoginCallbackAction)];

			string redirectUrl = UriHelper.BuildRelative(
				context.Request.PathBase,
				"/Account/ExternalLogin",
				QueryString.Create(query));

			AuthenticationProperties properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
			return TypedResults.Challenge(properties, [provider]);
		});

		_ = accountGroup.MapPost("/Logout", async (
			ClaimsPrincipal user,
			SignInManager<ApplicationUser> signInManager,
			[FromForm] string returnUrl) =>
		{
			await signInManager.SignOutAsync().ConfigureAwait(false);
			return TypedResults.LocalRedirect($"~/{returnUrl}");
		});

		RouteGroupBuilder manageGroup = accountGroup.MapGroup("/Manage").RequireAuthorization();

		_ = manageGroup.MapPost("/LinkExternalLogin", async (
			HttpContext context,
			[FromServices] SignInManager<ApplicationUser> signInManager,
			[FromForm] string provider) =>
		{
			// Clear the existing external cookie to ensure a clean login process
			await context.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

			string redirectUrl = UriHelper.BuildRelative(
				context.Request.PathBase,
				"/Account/Manage/ExternalLogins",
				QueryString.Create("Action", ExternalLogins.LinkLoginCallbackAction));

			AuthenticationProperties properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, signInManager.UserManager.GetUserId(context.User));
			return TypedResults.Challenge(properties, [provider]);
		});

		ILoggerFactory loggerFactory = endpoints.ServiceProvider.GetRequiredService<ILoggerFactory>();
		ILogger downloadLogger = loggerFactory.CreateLogger("DownloadPersonalData");

		_ = manageGroup.MapPost("/DownloadPersonalData", async (
			HttpContext context,
			[FromServices] UserManager<ApplicationUser> userManager,
			[FromServices] AuthenticationStateProvider authenticationStateProvider) =>
		{
			ApplicationUser? user = await userManager.GetUserAsync(context.User).ConfigureAwait(false);
			if (user is null)
			{
				return Results.NotFound($"Unable to load user with ID '{userManager.GetUserId(context.User)}'.");
			}

			string userId = await userManager.GetUserIdAsync(user).ConfigureAwait(false);
			downloadLogger.LogInformation("User with ID '{UserId}' asked for their personal data.", userId);

			// Only include personal data for download
			Dictionary<string, string> personalData = [];
			IEnumerable<System.Reflection.PropertyInfo> personalDataProps = typeof(ApplicationUser).GetProperties().Where(
				prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
			foreach (System.Reflection.PropertyInfo? p in personalDataProps)
			{
				personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
			}

			IList<UserLoginInfo> logins = await userManager.GetLoginsAsync(user).ConfigureAwait(false);
			foreach (UserLoginInfo l in logins)
			{
				personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
			}

			personalData.Add("Authenticator Key", (await userManager.GetAuthenticatorKeyAsync(user).ConfigureAwait(false))!);
			byte[] fileBytes = JsonSerializer.SerializeToUtf8Bytes(personalData);

			_ = context.Response.Headers.TryAdd("Content-Disposition", "attachment; filename=PersonalData.json");
			return TypedResults.File(fileBytes, contentType: "application/json", fileDownloadName: "PersonalData.json");
		});

		return accountGroup;
	}
}