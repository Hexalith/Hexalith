// <copyright file="IApplicationClientModule.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Represents an application client module.
/// </summary>
public interface IApplicationClientModule : IApplicationModule
{
    /// <summary>
    /// Adds the application client services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    void AddClientServices(IServiceCollection services, IConfiguration configuration);
}