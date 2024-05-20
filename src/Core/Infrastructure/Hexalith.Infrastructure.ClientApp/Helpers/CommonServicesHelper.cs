// <copyright file="CommonServicesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Helpers;

using Blazored.SessionStorage;

using Hexalith.Application.Organizations.Helpers;
using Hexalith.Application.Routes;
using Hexalith.Extensions.Common;
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
    public static IServiceCollection AddHexalithClientApp(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMemoryCache()
            .AddLocalization()
            .AddOrganizations(configuration)
            .AddSendGridEmail(configuration)
            .AddSingleton(TimeProvider.System)
            .AddSingleton<IDateTimeService, DateTimeService>()
            .AddSingleton<IRouteManager, RouteManager>()
            .AddBlazoredSessionStorage()
            .AddFluentUIComponents();
    }
}