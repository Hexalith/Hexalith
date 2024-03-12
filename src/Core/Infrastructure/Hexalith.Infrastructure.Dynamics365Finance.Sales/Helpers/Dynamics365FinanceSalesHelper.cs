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

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Retail.Stores.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Sales.PackingSlips.Models;
using Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Controller;
using Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.IntegrationEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

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
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="actors"/> is null.</exception>
    public static ActorRegistrationCollection AddDynamics365FinanceProjections([NotNull] this ActorRegistrationCollection actors)
    {
        ArgumentNullException.ThrowIfNull(actors);
        return actors;
    }

    /// <summary>
    /// Adds the dynamics365 finance sales invoices.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="configuration"/> is null.</exception>
    /// <exception cref="System.ArgumentException">Thrown when <paramref name="applicationName"/> is null or whitespace.</exception>
    public static IServiceCollection AddDynamics365FinanceSalesInvoices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        _ = services.ConfigureSettings<Dynamics365FinanceSalesSettings>(configuration);
        return services
            .AddDynamics365FinanceSalesInvoicesClient(configuration)
            .AddDynamics365FinanceSalesInvoicesBusinessEvents(configuration);
    }

    /// <summary>
    /// Adds the dynamics365 finance sales invoices business events.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="configuration"/> is null.</exception>
    /// <exception cref="System.ArgumentException">Thrown when <paramref name="applicationName"/> is null or whitespace.</exception>
    public static IServiceCollection AddDynamics365FinanceSalesInvoicesBusinessEvents(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        services
            .AddDynamics365FinanceBusinessEvents(configuration)
            .TryAddSingleton<IValidator<SalesInvoicePostedBusinessEvent>, SalesInvoicePostedValidator>();
        _ = services.ConfigureSettings<Dynamics365FinanceSalesSettings>(configuration);
        _ = services
            .AddControllers()
            .AddApplicationPart(typeof(Dynamics365FinanceSalesInvoiceBindingController).Assembly)
            .AddDapr();
        return services;
    }

    /// <summary>
    /// Adds the dynamics365 finance sales invoices client.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="configuration"/> is null.</exception>
    public static IServiceCollection AddDynamics365FinanceSalesInvoicesClient(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        _ = services
            .AddDynamics365FinanceClient(configuration)
            .AddHttpClient<IDynamics365FinanceClient<SalesInvoiceExternalSystemCode>, Dynamics365FinanceClient<SalesInvoiceExternalSystemCode>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<SalesInvoiceV3>, Dynamics365FinanceClient<SalesInvoiceV3>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<SalesInvoiceBase>, Dynamics365FinanceClient<SalesInvoiceBase>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<SalesInvoices.Models.SalesInvoiceLine>, Dynamics365FinanceClient<SalesInvoices.Models.SalesInvoiceLine>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<RetailStore>, Dynamics365FinanceClient<RetailStore>>();

        return services;
    }

    /// <summary>
    /// Adds the dynamics365 finance sales orders client.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="configuration"/> is null.</exception>
    public static IServiceCollection AddDynamics365FinanceSalesOrdersClient(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        _ = services
            .AddDynamics365FinanceClient(configuration);
        _ = services.AddHttpClient<IDynamics365FinanceClient<SalesOrderHeader>, Dynamics365FinanceClient<SalesOrderHeader>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<SalesOrderHeaderAdditional>, Dynamics365FinanceClient<SalesOrderHeaderAdditional>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<SalesOrderHeaderCharge>, Dynamics365FinanceClient<SalesOrderHeaderCharge>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<SalesOrderHeaderEdiInformation>, Dynamics365FinanceClient<SalesOrderHeaderEdiInformation>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<SalesOrderLine>, Dynamics365FinanceClient<SalesOrderLine>>();
        _ = services.AddHttpClient<IDynamics365FinanceClient<SalesOrderLineAdditional>, Dynamics365FinanceClient<SalesOrderLineAdditional>>();

        return services;
    }

    /// <summary>
    /// Adds the dynamics365 finance sales packing slips client.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="services"/> or <paramref name="configuration"/> is null.</exception>
    public static IServiceCollection AddDynamics365FinanceSalesPackingSlipsClient(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        _ = services
            .AddDynamics365FinanceClient(configuration);
        _ = services.AddHttpClient<IDynamics365FinanceClient<PackingSlipTrackingInformation>, Dynamics365FinanceClient<PackingSlipTrackingInformation>>();

        return services;
    }
}