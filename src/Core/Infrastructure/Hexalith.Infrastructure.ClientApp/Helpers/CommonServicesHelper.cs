// <copyright file="CommonServicesHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Helpers;

using Blazored.SessionStorage;

using Hexalith.Application.Modules.Routes;
using Hexalith.Application.Organizations.Helpers;
using Hexalith.Infrastructure.ClientApp.Services;
using Hexalith.Infrastructure.Emails.SendGrid.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Helper class for adding common services to the service collection.
/// </summary>
public static class CommonServicesHelper
{
    /// <summary>
    /// Adds common services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddHexalithClientApp(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddMemoryCache()
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .AddCascadingAuthenticationState()
            .AddOrganizations(configuration)
            .AddScoped<IUserSessionService, UserSessionService>()
            .AddSendGridEmail(configuration)
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IRouteManager, RouteManager>()
            .AddBlazoredSessionStorage()
            .AddFluentUIComponents();
}