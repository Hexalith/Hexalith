// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Inventories
// Author           : Jérôme Piquot
// Created          : 10-09-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-31-2023
// ***********************************************************************
// <copyright file="Dynamics365FinancePartnerCatalogHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.Helpers;

using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Runtime;

using FluentValidation;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.Validators;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class InventoriesHelper.
/// </summary>
public static class Dynamics365FinancePartnerCatalogHelper
{
    /// <summary>
    /// Adds the dynamics365 finance partner catalog items.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static IServiceCollection AddDynamics365FinancePartnerCatalogItems(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        _ = services.ConfigureSettings<Dynamics365FinanceInventoriesSettings>(configuration);
        return services
            .AddDynamics365FinancePartnerCatalogItemsClient(configuration)
            .AddDynamics365FinancePartnerCatalogItemsBusinessEvents(configuration);
    }

    /// <summary>
    /// Adds the dynamics365 finance partner catalog items business events.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static IServiceCollection AddDynamics365FinancePartnerCatalogItemsBusinessEvents(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        _ = services
            .AddDynamics365FinanceBusinessEvents(configuration)
            .ConfigureSettings<Dynamics365FinanceInventoriesSettings>(configuration);
        services.TryAddSingleton<IValidator<Dynamics365LogisticsPartnerCatalogItemAdded>, Dynamics365LogisticsPartnerCatalogItemAddedValidator>();
        services.TryAddSingleton<IValidator<Dynamics365LogisticsPartnerCatalogItemRemoved>, Dynamics365LogisticsPartnerCatalogItemRemovedValidator>();
        services.TryAddSingleton<IValidator<Dynamics365LogisticsPartnerCatalogItemNameChanged>, Dynamics365LogisticsPartnerCatalogItemNameChangedValidator>();
        services.TryAddSingleton<IValidator<Dynamics365LogisticsPartnerCatalogItemPriceChanged>, Dynamics365LogisticsPartnerCatalogItemPriceChangedValidator>();
        _ = services
            .AddControllers()

            // .AddApplicationPart(typeof(Dynamics365FinanceProductBarcodeBindingController).Assembly)
            .AddDapr();
        return services;
    }

    /// <summary>
    /// Adds the dynamics365 finance partner catalog items client.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static IServiceCollection AddDynamics365FinancePartnerCatalogItemsClient(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        _ = services
            .AddDynamics365FinanceClient(configuration);

        return services;
    }

    /// <summary>
    /// Adds the dynamics365 finance projections.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static ActorRegistrationCollection AddDynamics365FinanceProjections([NotNull] this ActorRegistrationCollection actors)
    {
        ArgumentNullException.ThrowIfNull(actors);
        return actors;
    }
}