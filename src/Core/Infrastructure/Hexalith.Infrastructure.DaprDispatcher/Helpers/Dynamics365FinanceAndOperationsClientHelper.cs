// <copyright file="Dynamics365FinanceAndOperationsClientHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

namespace Hexalith.Infrastructure.DaprDispatcher.Helpers;

using Hexalith.Application.Abstractions.Commands;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class to configure Dynamics 365 Finance and Operations client.
/// </summary>
public static class Dynamics365FinanceAndOperationsClientHelper
{
    /// <summary>
    /// Adds a Dynamics 365 Finance and Operations client to the service collection.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration containing the client settings values.</param>
    /// <returns>The services collection.</returns>
    public static IServiceCollection AddDaprDispatchers(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton<ICommandDispatcher, ConventionNamingCommandDispatcher>();
    }
}
