// <copyright file="DaprHandlersHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprHandlers.Helpers;

using Hexalith.Application.Abstractions.Commands;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class to configure Dynamics 365 Finance and Operations client.
/// </summary>
public static class DaprHandlersHelper
{
    /// <summary>
    /// Adds a Dynamics 365 Finance and Operations client to the service collection.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration containing the client settings values.</param>
    /// <returns>The services collection.</returns>
    public static IServiceCollection AddDaprHandlers(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton<ICommandProcessor, ConventionNamingCommandProcessor>();
    }
}