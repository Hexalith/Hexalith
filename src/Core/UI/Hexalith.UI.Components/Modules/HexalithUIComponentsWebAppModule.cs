// <copyright file="HexalithUIComponentsWebAppModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Modules;

using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;
using Hexalith.Extensions.Configuration;
using Hexalith.UI.Components.Configurations;
using Hexalith.UI.Components.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents the HexalithWeb module for customer management.
/// </summary>
public class HexalithUIComponentsWebAppModule : IWebAppApplicationModule
{
    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "Hexalith Fluent UI Web App module";

    /// <inheritdoc/>
    public string Id => "Hexalith.UI.Components.WebApp";

    /// <inheritdoc/>
    public string Name => "Hexalith Fluent UI Web App";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Path => "hexalith";

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies
        => [GetType().Assembly];

    /// <inheritdoc/>
    public string Version => "1.0.0";

    /// <summary>
    /// Adds services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services
           .AddFluentUITheme(configuration)
           .ConfigureSettings<FluentUIThemeSettings>(configuration)
           .AddDataGridEntityFrameworkAdapter();
    }

    /// <inheritdoc/>
    public void UseModule(object builder)
    {
    }
}