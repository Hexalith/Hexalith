// <copyright file="ComponentsHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Helpers;

using Hexalith.DaprIdentityStore.Client.Services;
using Hexalith.DaprIdentityStore.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.UI.Components.Configurations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Helper class for adding Fluent UI theme to the service collection.
/// </summary>
public static class ComponentsHelper
{
    /// <summary>
    /// Adds Fluent UI theme to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration properties.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddFluentUITheme(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.ConfigureSettings<FluentUIThemeSettings>(configuration);

        services.TryAddScoped<ILoginRedirectUrlService, LoginRedirectUrlService>();
        _ = services.AddFluentUIComponents();
        return services;
    }
}