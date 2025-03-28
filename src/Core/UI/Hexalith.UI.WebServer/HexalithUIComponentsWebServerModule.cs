﻿// <copyright file="HexalithUIComponentsWebServerModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.WebServer;

using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;
using Hexalith.Extensions.Configuration;
using Hexalith.UI.Components.Configurations;
using Hexalith.UI.Components.Helpers;
using Hexalith.UI.WebApp;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents the HexalithWeb module for customer management.
/// </summary>
public class HexalithUIComponentsWebServerModule : IWebServerApplicationModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => new Dictionary<string, AuthorizationPolicy>();

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "Hexalith Fluent UI Web Server module";

    /// <inheritdoc/>
    public string Id => "Hexalith.UI.Components.WebServer";

    /// <inheritdoc/>
    public string Name => "Hexalith Fluent UI Web Server";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Path => "hexalith";

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies
        => [GetType().Assembly, typeof(Components._Imports).Assembly, typeof(HexalithUIComponentsWebAppModule).Assembly];

    /// <inheritdoc/>
    public string Version => "1.0.0";

    /// <summary>
    /// Adds services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        _ = services
            .AddControllers()
            .AddApplicationPart(typeof(HexalithUIComponentsWebServerModule).Assembly)
            .AddApplicationPart(typeof(Components._Imports).Assembly);
        services
           .AddFluentUITheme(configuration)
           .ConfigureSettings<FluentUIThemeSettings>(configuration)
           .AddDataGridEntityFrameworkAdapter();
    }

    /// <inheritdoc/>
    public void UseModule(object builder)
    {
    }

    /// <inheritdoc/>
    public void UseSecurity(object application)
    {
    }
}