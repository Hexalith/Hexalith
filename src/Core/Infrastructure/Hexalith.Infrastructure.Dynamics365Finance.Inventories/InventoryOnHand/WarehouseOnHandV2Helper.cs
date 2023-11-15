// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 09-06-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-06-2023
// ***********************************************************************
// <copyright file="WarehouseOnHandV2Helper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.InventoryOnHand;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Domain.Events;

/// <summary>
/// Class CustomersV3Helper.
/// </summary>
public static class WarehouseOnHandV2Helper
{
    /// <summary>
    /// Converts to inventoryitemstockevent.
    /// </summary>
    /// <param name="onHand">The on hand.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="date">The date.</param>
    /// <returns>InventoryItemStockEvent.</returns>
    public static InventoryItemStockEvent ToInventoryItemStockEvent([NotNull] this WarehouseOnHandV2 onHand, string partitionId, decimal quantity, DateTimeOffset date)
    {
        ArgumentNullException.ThrowIfNull(onHand);
        return quantity < onHand.AvailableOnHandQuantity
            ? new InventoryItemStockDecreased(
                partitionId,
                onHand.DataAreaId,
                onHand.InventorySiteId + "-" + onHand.InventoryWarehouseId,
                onHand.ItemNumber,
                Math.Abs(quantity - (onHand.AvailableOnHandQuantity == null ? 0 : onHand.AvailableOnHandQuantity.Value)),
                date)
            : new InventoryItemStockIncreased(
                partitionId,
                onHand.DataAreaId,
                onHand.InventorySiteId + "-" + onHand.InventoryWarehouseId,
                onHand.ItemNumber,
                Math.Abs((onHand.AvailableOnHandQuantity == null ? 0 : onHand.AvailableOnHandQuantity.Value) - quantity),
                date);
    }
}