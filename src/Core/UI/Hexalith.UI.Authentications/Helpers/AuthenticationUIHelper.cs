// <copyright file="AuthenticationUIHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Authentications.Helpers;

using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class AuthenticationUIHelper
{
    public static IServiceCollection AddAuthenticationUI(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services
        .AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddBearerToken(IdentityConstants.BearerScheme)
        .AddMicrosoftAccount(microsoftOptions =>
        {
            string? clientId = configuration["Authentication:Microsoft:ClientId"];
            string? clientSecret = configuration["Authentication:Microsoft:ClientSecret"];
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new InvalidOperationException("Authentication:Microsoft:ClientId must be set in the application settings.");
            }

            if (string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new InvalidOperationException("Authentication:Microsoft:ClientSecret must be set in the application settings.");
            }

            microsoftOptions.ClientId = clientId;
            microsoftOptions.ClientSecret = clientSecret;
        })
        .AddIdentityCookies();

        return services;
    }
}