// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis
// Author           : Jérôme Piquot
// Created          : 05-30-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-30-2023
// ***********************************************************************
// <copyright file="PassThroughAuthenticationHandler.cs" company="Fiveforty">
//     Copyright (c) Fiveforty S.A.S.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.WebApis.Authentications;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Copyright (c) Microsoft. All rights reserved.

using System.Security.Claims;
using System.Text.Encodings.Web;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class implementing "authentication" that lets all requests pass through.
/// </summary>
public class PassThroughAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    /// <summary>
    /// The authentication scheme
    /// </summary>
    public const string AuthenticationScheme = "PassThrough";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    /// <param name="encoder">The encoder.</param>
    /// <param name="clock">The clock.</param>
    public PassThroughAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, loggerFactory, encoder, clock)
    {
    }

    /// <summary>
    /// Allows derived types to handle authentication.
    /// </summary>
    /// <returns>The <see cref="T:Microsoft.AspNetCore.Authentication.AuthenticateResult" />.</returns>
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        this.Logger.LogInformation("Allowing request to pass through");

        var principal = new ClaimsPrincipal(new ClaimsIdentity(AuthenticationScheme));
        var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}