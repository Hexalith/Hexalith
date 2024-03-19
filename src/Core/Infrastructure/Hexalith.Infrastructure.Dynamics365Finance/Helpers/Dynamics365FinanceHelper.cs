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

using System.Net.Http.Headers;

using FluentValidation;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Configurations;
using Hexalith.Infrastructure.Dynamics365Finance.Dispatchers;
using Hexalith.Infrastructure.Dynamics365Finance.Models;
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
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDynamics365FinanceBusinessEvents(this IServiceCollection services)
    {
        services.TryAddScoped<IDynamics365FinanceIntegrationEventProcessor, Dynamics365FinanceIntegrationEventProcessor>();
        services.TryAddSingleton<IValidator<Dynamics365BusinessEventBase>, Dynamics365BusinessEventValidator>();
        return services;
    }

    /// <summary>
    /// Adds the dynamics365 finance client.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddDynamics365FinanceClient<TEntity>(this IServiceCollection services, IConfiguration configuration)
        where TEntity : class, IODataCommon
    {
        _ = services
            .ConfigureSettings<Dynamics365FinanceClientSettings>(configuration);
        _ = services.AddHttpClient<IDynamics365FinanceClient<TEntity>, Dynamics365FinanceClient<TEntity>>(
                "Dynamics365FinanceClient",
                client =>
            {
                string settingsName = Dynamics365FinanceClientSettings.ConfigurationName();
                string? instance = configuration[$"{settingsName}:{nameof(Dynamics365FinanceClientSettings.Instance)}"];
                SettingsException<Dynamics365FinanceClientSettings>.ThrowIfNullOrWhiteSpace(instance, nameof(Dynamics365FinanceClientSettings.Instance));
                client.BaseAddress = new Uri(instance);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                client.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations = *");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });

        // .AddResilienceHandler(
        // "no-retry",
        // b => b.AddFallback(new FallbackStrategyOptions<HttpResponseMessage>()
        // {
        //    FallbackAction = _ => Outcome.FromResultAsValueTask(
        //        new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)),
        // })
        // .AddConcurrencyLimiter(1000)
        // .AddRetry(new HttpRetryStrategyOptions { MaxRetryAttempts = 1 })
        // .AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions())
        // .AddTimeout(new HttpTimeoutStrategyOptions { Timeout = TimeSpan.FromSeconds(20) }));
        services.TryAddSingleton<IDynamics365FinanceSecurityContext, Dynamics365FinanceSecurityContext>();
        return services;
    }
}