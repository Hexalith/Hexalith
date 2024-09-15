// <copyright file="CommonServicesHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Helpers;

using System.Text.Json;

using Blazored.SessionStorage;

using Hexalith.Application.Modules.Routes;
using Hexalith.Application.Organizations.Helpers;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.Emails.SendGrid.Helpers;
using Hexalith.PolymorphicSerialization;

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
    public static IServiceCollection AddHexalithClientApp(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMemoryCache()
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .AddCascadingAuthenticationState()
            .AddOrganizations(configuration)
            .AddSendGridEmail(configuration)
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IDateTimeService, DateTimeService>()
            .AddSingleton<IRouteManager, RouteManager>()
            .AddSingleton(services =>
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    TypeInfoResolver = new PolymorphicSerializationResolver(services.GetServices<IPolymorphicSerializationMapper>()),
                })
            .AddBlazoredSessionStorage()
            .AddFluentUIComponents();
    }
}