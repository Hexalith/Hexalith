// <copyright file="IApplicationServerModule.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents an application server module.
/// </summary>
public interface IApplicationServerModule : IApplicationModule
{
    /// <summary>
    /// Adds the application server services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    void AddServerServices(IServiceCollection services, IConfiguration configuration);
}