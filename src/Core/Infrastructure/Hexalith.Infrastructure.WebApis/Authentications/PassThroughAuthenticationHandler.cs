// <copyright file="PassThroughAuthenticationHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Authentications;

using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class implementing "authentication" that lets all requests pass through.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PassThroughAuthenticationHandler" /> class.
/// Constructor.
/// </remarks>
/// <param name="options">The options.</param>
/// <param name="loggerFactory">The logger factory.</param>
/// <param name="encoder">The encoder.</param>
public class PassThroughAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory loggerFactory,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, loggerFactory, encoder)
{
    /// <summary>
    /// The authentication scheme.
    /// </summary>
    public const string AuthenticationScheme = "PassThrough";

    /// <summary>
    /// Allows derived types to handle authentication.
    /// </summary>
    /// <returns>The <see cref="AuthenticateResult" />.</returns>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        ClaimsPrincipal principal = new(new ClaimsIdentity(AuthenticationScheme));
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}