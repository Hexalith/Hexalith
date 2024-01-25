// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Sales
// Author           : Jérôme Piquot
// Created          : 10-09-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-31-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceSalesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.Helpers;

using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Runtime;

using FluentValidation;

using Hexalith.Application.Events;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class SalesHelper.
/// </summary>
public static class Dynamics365FinanceSalesHelper
{
    /// <summary>
    /// Adds the dynamics365 finance projections.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ActorRegistrationCollection AddDynamics365FinanceProjections([NotNull] this ActorRegistrationCollection actors, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(actors);
        return actors;
    }

    /// <summary>
    /// Adds the dynamics365 finance customers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddDynamics365FinanceSalesInvoices(this IServiceCollection services, IConfiguration configuration, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrEmpty(applicationName);
        _ = services.ConfigureSettings<Dynamics365FinanceSalesSettings>(configuration);
        return services
            .AddDynamics365FinanceSalesInvoicesClient(configuration)
            .AddDynamics365FinanceSalesInvoicesBusinessEvents(configuration, applicationName);
    }

    /// <summary>
    /// Adds the dynamics365 finance customers business events.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddDynamics365FinanceSalesInvoicesBusinessEvents(this IServiceCollection services, IConfiguration configuration, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrEmpty(applicationName);
        services
            .AddDynamics365FinanceBusinessEvents(configuration)
        services.TryAddSingleton<IValidator<Dynamics365FinanceSalesInvoiceRegistered>, Dynamics365FinanceSalesInvoiceRegisteredValidator>();
        services.TryAddScoped<IIntegrationEventHandler<Dynamics365FinanceSalesInvoiceRegistered>, Dynamics365FinanceSalesInvoiceRegisteredHandler>();
        _ = services.ConfigureSettings<Dynamics365FinanceSalesSettings>(configuration);
        _ = services
            .AddControllers()
            .AddApplicationPart(typeof(Dynamics365FinanceSalesInvoiceBindingController).Assembly)
            .AddDapr();
        return services;
    }

    /// <summary>
    /// Adds the dynamics365 finance customers client.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddDynamics365FinanceSalesInvoicesClient(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        _ = services
            .AddDynamics365FinanceClient(configuration)
            .AddHttpClient<IDynamics365FinanceClient<SalesInvoiceExternalSystemCode>, Dynamics365FinanceClient<SalesInvoiceExternalSystemCode>>();
        services.TryAddScoped<IDynamics365FinanceSalesInvoiceService, Dynamics365FinanceSalesInvoiceService>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<SalesInvoiceV3>, Dynamics365FinanceClient<SalesInvoiceV3>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<SalesInvoiceBase>, Dynamics365FinanceClient<SalesInvoiceBase>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<RetailStore>, Dynamics365FinanceClient<RetailStore>>();

        return services;
    }
}