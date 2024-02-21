// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.Parties
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="PartiesWebApiHelpers.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.InventoriesEvents.Helpers;

using Hexalith.Application.Inventories.InventoryItems.Projections;
using Hexalith.Application.Projection;
using Hexalith.Domain.InventoryItems.Events;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.WebApis.InventoriesEvents.Controllers;
using Hexalith.Infrastructure.WebApis.InventoriesEvents.Projections;

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
    public static IServiceCollection AddInventoryItemProjections(this IServiceCollection services, string appName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(appName);
        services.TryAddScoped<IProjectionUpdateHandler<InventoryItemInformationChanged>, InventoryItemInformationChangedProjectionUpdateHandler>();
        services.TryAddScoped<IProjectionUpdateHandler<InventoryItemAdded>, InventoryItemAddedProjectionUpdateHandler>();
        _ = services.AddActorProjectionFactory<InventoryItemDetailsProjection>(appName);
        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(InventoryItemIntegrationEventsController).Assembly)
         .AddDapr();
        return services;
    }
}