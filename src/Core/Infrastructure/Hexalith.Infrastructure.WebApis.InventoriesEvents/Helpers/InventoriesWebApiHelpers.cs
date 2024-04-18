// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.Parties
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="InventoriesWebApiHelpers.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.InventoriesEvents.Helpers;

using Hexalith.Application.Projections;
using Hexalith.Domain.Events;
using Hexalith.Domain.InventoryItems.Aggregates;
using Hexalith.Domain.InventoryItems.Events;
using Hexalith.Domain.PartnerInventoryItems.Aggregates;
using Hexalith.Domain.PartnerInventoryItems.Events;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Projections;
using Hexalith.Infrastructure.WebApis.InventoriesEvents.Controllers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class PartiesWebApiHelpers.
/// </summary>
public static class InventoriesWebApiHelpers
{
    /// <summary>
    /// Adds the customer projections.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="appName">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static IServiceCollection AddInventoryItemsProjections(this IServiceCollection services, string appName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(appName);
        services.TryAddScoped<IProjectionUpdateHandler<InventoryItemDescriptionChanged>, AggregateProjectionUpdateEventHandler<InventoryItemDescriptionChanged, InventoryItem>>();
        services.TryAddScoped<IProjectionUpdateHandler<InventoryItemAdded>, AggregateProjectionUpdateEventHandler<InventoryItemAdded, InventoryItem>>();
        services.TryAddScoped<IProjectionUpdateHandler<InventoryItemBarcodeAssigned>, AggregateProjectionUpdateEventHandler<InventoryItemBarcodeAssigned, InventoryItem>>();
        services.TryAddScoped<IProjectionUpdateHandler<InventoryItemBarcodeDisassociated>, AggregateProjectionUpdateEventHandler<InventoryItemBarcodeDisassociated, InventoryItem>>();
        services.TryAddScoped<IProjectionUpdateHandler<InventoryItemBarcodeRatioAdjusted>, AggregateProjectionUpdateEventHandler<InventoryItemBarcodeRatioAdjusted, InventoryItem>>();
        services.TryAddScoped<IProjectionUpdateHandler<InventoryItemDefaultBarcodeAssigned>, AggregateProjectionUpdateEventHandler<InventoryItemDefaultBarcodeAssigned, InventoryItem>>();
        services.TryAddScoped<IProjectionUpdateHandler<InventoryItemDefaultBarcodeCleared>, AggregateProjectionUpdateEventHandler<InventoryItemDefaultBarcodeCleared, InventoryItem>>();
        services.TryAddScoped<IProjectionUpdateHandler<SnapshotEvent>, AggregateProjectionUpdateEventHandler<SnapshotEvent, InventoryItem>>();
        _ = services.AddActorProjectionFactory<InventoryItem>(appName);
        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(InventoryItemIntegrationEventsController).Assembly)
         .AddDapr();
        return services;
    }

    /// <summary>
    /// Adds the customer projections.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="applicationId">Name of the application.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">null.</exception>
    public static IServiceCollection AddPartnerInventoryItemsProjections(this IServiceCollection services, string applicationId)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationId);
        services.TryAddScoped<IProjectionUpdateHandler<PartnerInventoryItemNameChanged>, AggregateProjectionUpdateEventHandler<PartnerInventoryItemNameChanged, PartnerInventoryItem>>();
        services.TryAddScoped<IProjectionUpdateHandler<PartnerInventoryItemPriceChanged>, AggregateProjectionUpdateEventHandler<PartnerInventoryItemPriceChanged, PartnerInventoryItem>>();
        services.TryAddScoped<IProjectionUpdateHandler<PartnerInventoryItemChanged>, AggregateProjectionUpdateEventHandler<PartnerInventoryItemChanged, PartnerInventoryItem>>();
        services.TryAddScoped<IProjectionUpdateHandler<PartnerInventoryItemAdded>, AggregateProjectionUpdateEventHandler<PartnerInventoryItemAdded, PartnerInventoryItem>>();
        services.TryAddScoped<IProjectionUpdateHandler<PartnerInventoryItemRemoved>, AggregateProjectionUpdateEventHandler<PartnerInventoryItemRemoved, PartnerInventoryItem>>();
        services.TryAddScoped<IProjectionUpdateHandler<SnapshotEvent>, AggregateProjectionUpdateEventHandler<SnapshotEvent, PartnerInventoryItem>>();
        _ = services.AddActorProjectionFactory<PartnerInventoryItem>(applicationId);
        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(PartnerInventoryItemIntegrationEventsController).Assembly)
         .AddDapr();
        return services;
    }
}