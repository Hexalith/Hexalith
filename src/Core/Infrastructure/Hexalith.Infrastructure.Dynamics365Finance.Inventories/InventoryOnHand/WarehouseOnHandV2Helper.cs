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

using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers;

/// <summary>
/// Class CustomersV3Helper.
/// </summary>
public static class WarehouseOnHandV2Helper
{
    /// <summary>
    /// Converts to inventory item stock event.
    /// </summary>
    /// <param name="onHand">The on hand.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="date">The date.</param>
    /// <returns>InventoryItemStockEvent.</returns>
    public static InventoryItemStockEvent ToInventoryItemStockEvent(this WarehouseOnHandV2 onHand, decimal quantity, DateTimeOffset date)
    {
        return quantity < onHand.AvailableOnHandQuantity
            ? new InventoryItemStockDecreased(
                onHand.DataAreaId,
                onHand.InventorySiteId + "-" + onHand.InventoryWarehouseId,
                onHand.ItemNumber,
                Math.Abs(quantity - (onHand.AvailableOnHandQuantity == null ? 0 : onHand.AvailableOnHandQuantity.Value)),
                date)
            : new InventoryItemStockIncreased(
            onHand.DataAreaId,
            onHand.InventorySiteId + "-" + onHand.InventoryWarehouseId,
            onHand.ItemNumber,
            Math.Abs((onHand.AvailableOnHandQuantity == null ? 0 : onHand.AvailableOnHandQuantity.Value) - quantity),
            date);
    }
}