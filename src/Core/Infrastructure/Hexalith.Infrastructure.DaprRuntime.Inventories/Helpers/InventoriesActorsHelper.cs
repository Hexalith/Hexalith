// <copyright file="InventoriesActorsHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Inventories.Helpers;

using System.Diagnostics.CodeAnalysis;

using Dapr.Actors.Runtime;

using Hexalith.Application.Inventories.InventoryItems.Projections;
using Hexalith.Application.Inventories.InventoryItemStocks.Projections;
using Hexalith.Application.Inventories.InventoryUnitConversions.Projections;
using Hexalith.Application.Inventories.InventoryUnits.Projections;
using Hexalith.Application.Inventories.PartnerInventoryItems.Projections;
using Hexalith.Domain.InventoryItems.Aggregates;
using Hexalith.Domain.InventoryItemStocks.Aggregates;
using Hexalith.Domain.InventoryUnitConversions.Aggregates;
using Hexalith.Domain.InventoryUnits.Aggregates;
using Hexalith.Domain.PartnerInventoryItems.Aggregates;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

/// <summary>
/// Class InventoriesHelper.
/// </summary>
public static class InventoriesActorsHelper
{
    /// <summary>
    /// Adds the parties.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ActorRegistrationCollection AddInventoriesAggregates([NotNull] this ActorRegistrationCollection actors)
    {
        ArgumentNullException.ThrowIfNull(actors);
        actors.RegisterActor<AggregateActor>(AggregateActor.GetAggregateActorName(InventoryItem.GetAggregateName()));
        actors.RegisterActor<AggregateActor>(AggregateActor.GetAggregateActorName(InventoryItemStock.GetAggregateName()));
        actors.RegisterActor<AggregateActor>(AggregateActor.GetAggregateActorName(InventoryUnit.GetAggregateName()));
        actors.RegisterActor<AggregateActor>(AggregateActor.GetAggregateActorName(InventoryUnitConversion.GetAggregateName()));
        actors.RegisterActor<AggregateActor>(AggregateActor.GetAggregateActorName(PartnerInventoryItem.GetAggregateName()));
        return actors;
    }

    /// <summary>
    /// Adds the parties projections.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ActorRegistrationCollection AddInventoriesProjections([NotNull] this ActorRegistrationCollection actors, string applicationName)
    {
        ArgumentNullException.ThrowIfNull(actors);
        actors.RegisterProjectionActor<InventoryItemDetailsProjection>(applicationName);
        actors.RegisterProjectionActor<InventoryItemStockProjection>(applicationName);
        actors.RegisterProjectionActor<InventoryUnitConversionDetailsProjection>(applicationName);
        actors.RegisterProjectionActor<InventoryUnitDetailsProjection>(applicationName);
        actors.RegisterProjectionActor<InventoryItemToPartnerItemProjection>(applicationName);
        actors.RegisterProjectionActor<PartnerInventoryItemProjection>(applicationName);
        return actors;
    }
}