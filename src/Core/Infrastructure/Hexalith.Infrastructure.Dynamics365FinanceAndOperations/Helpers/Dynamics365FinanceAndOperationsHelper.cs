// <copyright file="Dynamics365FinanceAndOperationsHelper.cs" company="Fiveforty SAS Paris France">
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

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Helpers;

using FluentValidation;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Dispatchers;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class to configure Dynamics 365 Finance and Operations client.
/// </summary>
public static class Dynamics365FinanceAndOperationsHelper
{
    /// <summary>
    /// Adds the dynamics365 finance and operations business events services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDynamics365FinanceAndOperationsBusinessEvents(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton<IDynamics365FinanceIntegrationEventProcessor, Dynamics365FinanceIntegrationEventProcessor>()
            .AddSingleton<IValidator<Dynamics365BusinessEventBase>, Dynamics365BusinessEventValidator>();
    }

    /// <summary>
    /// Adds a Dynamics 365 Finance and Operations client to the service collection.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Configuration containing the client settings values.</param>
    /// <returns>The services collection.</returns>
    public static IServiceCollection AddDynamics365FinanceAndOperationsClient(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddHttpClient()
            .ConfigureSettings<Dynamics365FinanceAndOperationsClientSettings>(configuration)
            .AddSingleton<IDynamics365FinanceAndOperationsSecurityContext, Dynamics365FinanceAndOperationsSecurityContext>()
            .AddScoped(typeof(IDynamics365FinanceAndOperationsClient<>), typeof(Dynamics365FinanceAndOperationsClient<>));
    }
}
