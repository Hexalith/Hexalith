// <copyright file="ApiKeyAuthenticationHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Authentications;

using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

/// <summary>
/// Class implementing API key authentication.
/// </summary>
public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
{
    public const string ApiKeyHeaderName = "x-sk-api-key";
    public const string AuthenticationScheme = "ApiKey";

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiKeyAuthenticationHandler"/> class.
    /// Constructor.
    /// </summary>
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, loggerFactory, encoder, clock)
    {
    }

    /// <inheritdoc/>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Logger.LogInformation("Checking API key");

        if (string.IsNullOrWhiteSpace(Options.ApiKey))
        {
            const string ErrorMessage = "API key not configured on server";

            Logger.LogError(ErrorMessage);

            return Task.FromResult(AuthenticateResult.Fail(ErrorMessage));
        }

        if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out StringValues apiKeyFromHeader))
        {
            const string WarningMessage = "No API key provided";

            Logger.LogWarning(WarningMessage);

            return Task.FromResult(AuthenticateResult.Fail(WarningMessage));
        }

        if (!string.Equals(apiKeyFromHeader, Options.ApiKey, StringComparison.Ordinal))
        {
            const string WarningMessage = "Incorrect API key";

            Logger.LogWarning(WarningMessage);

            return Task.FromResult(AuthenticateResult.Fail(WarningMessage));
        }

        ClaimsPrincipal principal = new(new ClaimsIdentity(AuthenticationScheme));
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        Logger.LogInformation("Request authorized by API key");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}