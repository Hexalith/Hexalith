// <copyright file="Dynamics365FinancePartnerCatalogHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.Helpers;

using FluentValidation;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.BusinessEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.Controllers;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.Validators;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class Dynamics365FinancePartnerCatalogHelper.
/// </summary>
public static class Dynamics365FinancePartnerCatalogHelper
{
    /// <summary>
    /// Adds the necessary services for Dynamics 365 Finance partner inventory items.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing the configuration settings.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddDynamics365FinancePartnerInventoryItems(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        _ = services.ConfigureSettings<Dynamics365FinanceInventoriesSettings>(configuration);
        return services
            .AddDynamics365FinancePartnerInventoryItemsClient(configuration)
            .AddDynamics365FinancePartnerInventoryItemsBusinessEvents(configuration);
    }

    /// <summary>
    /// Adds the necessary services for Dynamics 365 Finance partner inventory item business events.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing the configuration settings.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddDynamics365FinancePartnerInventoryItemsBusinessEvents(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        services
            .AddDynamics365FinanceBusinessEvents()
            .TryAddSingleton<IValidator<Dynamics365FinancePartnerInventoryItemAdded>, Dynamics365FinancePartnerInventoryItemAddedValidator>();
        services.TryAddSingleton<IValidator<Dynamics365FinancePartnerInventoryItemPriceChanged>, Dynamics365FinancePartnerInventoryItemPriceChangedValidator>();
        services.TryAddSingleton<IValidator<Dynamics365FinancePartnerInventoryItemNameChanged>, Dynamics365FinancePartnerInventoryItemNameChangedValidator>();
        services.TryAddSingleton<IValidator<Dynamics365FinancePartnerInventoryItemRemoved>, Dynamics365FinancePartnerInventoryItemRemovedValidator>();
        _ = services.ConfigureSettings<Dynamics365FinanceInventoriesSettings>(configuration);
        _ = services
            .AddControllers()
            .AddApplicationPart(typeof(Dynamics365FinancePartnerInventoryItemBindingController).Assembly)
            .AddDapr();
        return services;
    }

    /// <summary>
    /// Adds the necessary services for Dynamics 365 Finance partner inventory item client.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing the configuration settings.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddDynamics365FinancePartnerInventoryItemsClient(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        return services;
    }
}