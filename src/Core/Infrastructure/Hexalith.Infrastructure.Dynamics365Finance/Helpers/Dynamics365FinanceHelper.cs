// <copyright file="Dynamics365FinanceHelper.cs" company="Fiveforty SAS Paris France">
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

namespace Hexalith.Infrastructure.Dynamics365Finance.Helpers;

using FluentValidation;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Configurations;
using Hexalith.Infrastructure.Dynamics365Finance.Dispatchers;
using Hexalith.Infrastructure.Dynamics365Finance.Security;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Helper class to configure Dynamics 365 Finance and Operations client.
/// </summary>
public static class Dynamics365FinanceHelper
{
    /// <summary>
    /// Adds the dynamics365 finance and operations business events services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDynamics365FinanceBusinessEvents(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddScoped<IDynamics365FinanceIntegrationEventProcessor, Dynamics365FinanceIntegrationEventProcessor>();
        services.TryAddSingleton<IValidator<Dynamics365BusinessEventBase>, Dynamics365BusinessEventValidator>();
        return services;
    }

    /// <summary>
    /// Adds a Dynamics 365 Finance and Operations client to the service collection.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration containing the client settings values.</param>
    /// <returns>The services collection.</returns>
    public static IServiceCollection AddDynamics365FinanceClient(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services
            .AddHttpClient()
            .ConfigureSettings<Dynamics365FinanceClientSettings>(configuration);
        services.TryAddSingleton<IDynamics365FinanceSecurityContext, Dynamics365FinanceSecurityContext>();
        services.TryAddSingleton(typeof(IDynamics365FinanceClient<>), typeof(Dynamics365FinanceClient<>));
        return services;
    }
}