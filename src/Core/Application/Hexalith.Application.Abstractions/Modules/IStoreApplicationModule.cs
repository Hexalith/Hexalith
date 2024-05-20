// <copyright file="IStoreApplicationModule.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents the interface for a store application module.
/// </summary>
public interface IStoreApplicationModule : IApplicationModule
{
    /// <summary>
    /// Adds the store application (MAUI) services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    void AddStoreAppServices(IServiceCollection services, IConfiguration configuration);
}