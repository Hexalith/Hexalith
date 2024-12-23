// <copyright file="HexalithUIComponentsApiServerModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.ApiServer;

using System.Collections.Generic;

using Hexalith.Application.Modules.Modules;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents the HexalithWeb module for customer management.
/// </summary>
public class HexalithUIComponentsApiServerModule : IApiServerApplicationModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => new Dictionary<string, AuthorizationPolicy>();

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "Hexalith Fluent UI API Server module";

    /// <inheritdoc/>
    public string Id => "Hexalith.UI.Components.ApiServer";

    /// <inheritdoc/>
    public string Name => "Hexalith Fluent UI API Server";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Path => "hexalith";

    /// <inheritdoc/>
    public string Version => "1.0.0";

    /// <summary>
    /// Adds services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
    }

    /// <inheritdoc/>
    public void UseModule(object builder)
    {
    }
}