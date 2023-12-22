// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 10-09-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-31-2023
// ***********************************************************************
// <copyright file="Dynamics365FinancePartiesHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Helpers;

using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Runtime;

using FluentValidation;

using Hexalith.Application.Events;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Controller;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Services;
using Hexalith.Infrastructure.Dynamics365Finance.Retail.Stores.Entities;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class PartiesHelper.
/// </summary>
public static class Dynamics365FinancePartiesHelper
{
    /// <summary>
    /// Adds the dynamics365 finance customers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddDynamics365FinanceCustomers(this IServiceCollection services, IConfiguration configuration, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrEmpty(applicationName);
        _ = services.ConfigureSettings<Dynamics365FinancePartiesSettings>(configuration);
        return services
            .AddDynamics365FinanceCustomersClient(configuration)
            .AddDynamics365FinanceCustomersBusinessEvents(configuration, applicationName);
    }

    /// <summary>
    /// Adds the dynamics365 finance customers business events.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddDynamics365FinanceCustomersBusinessEvents(this IServiceCollection services, IConfiguration configuration, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrEmpty(applicationName);
        services
            .AddDynamics365FinanceBusinessEvents(configuration)
            .TryAddSingleton<IValidator<Dynamics365FinanceCustomerChanged>, Dynamics365FinanceCustomerChangedValidator>();
        services.TryAddSingleton<IValidator<Dynamics365FinanceCustomerRegistered>, Dynamics365FinanceCustomerRegisteredValidator>();
        services.TryAddScoped<IIntegrationEventHandler<Dynamics365FinanceCustomerChanged>, Dynamics365FinanceCustomerChangedHandler>();
        services.TryAddScoped<IIntegrationEventHandler<Dynamics365FinanceCustomerRegistered>, Dynamics365FinanceCustomerRegisteredHandler>();
        _ = services.ConfigureSettings<Dynamics365FinancePartiesSettings>(configuration);
        _ = services
            .AddControllers()
            .AddApplicationPart(typeof(Dynamics365FinanceCustomerBindingController).Assembly)
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
    public static IServiceCollection AddDynamics365FinanceCustomersClient(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        _ = services
            .AddDynamics365FinanceClient(configuration)
            .AddHttpClient<IDynamics365FinanceClient<CustomerExternalSystemCode>, Dynamics365FinanceClient<CustomerExternalSystemCode>>();
        services.TryAddScoped<IDynamics365FinanceCustomerService, Dynamics365FinanceCustomerService>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<CustomerV3>, Dynamics365FinanceClient<CustomerV3>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<CustomerBase>, Dynamics365FinanceClient<CustomerBase>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<RetailStore>, Dynamics365FinanceClient<RetailStore>>();

        return services;
    }

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
}