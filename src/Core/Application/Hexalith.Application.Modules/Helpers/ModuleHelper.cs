// <copyright file="ModuleHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Helpers;

using System.Linq;

using Hexalith.Application.Modules.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for managing application modules.
/// </summary>
public static class ModuleHelper
{
    /// <summary>
    /// Adds the client services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddModuleClientServices(this IServiceCollection services, IConfiguration configuration)
        => services.AddModuleServices(configuration, ModuleType.Client);

    /// <summary>
    /// Adds the server services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddModuleServerServices(this IServiceCollection services, IConfiguration configuration)
        => services.AddModuleServices(configuration, ModuleType.Server);

    /// <summary>
    /// Adds the services of the specified module type to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="moduleType">The module type.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddModuleServices(this IServiceCollection services, IConfiguration configuration, ModuleType moduleType)
    {
        foreach (IApplicationModule? module in ModuleManager
            .Modules
            .Where(p => p.Value.ModuleType == moduleType)
            .Select(p => p.Value)
            .OrderByDescending(p => p.OrderWeight))
        {
            module.AddServices(services, configuration);
        }

        return services;
    }

    /// <summary>
    /// Adds the shared services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddModuleSharedServices(this IServiceCollection services, IConfiguration configuration)
        => services.AddModuleServices(configuration, ModuleType.Shared);

    /// <summary>
    /// Adds the store application services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddModuleStoreAppServices(this IServiceCollection services, IConfiguration configuration)
        => services.AddModuleServices(configuration, ModuleType.StoreApp);
}