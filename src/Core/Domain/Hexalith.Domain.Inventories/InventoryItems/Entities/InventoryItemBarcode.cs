// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryItemBarcode.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.InventoryItems.Entities;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.InventoryItems.Aggregates;
using Hexalith.Domain.InventoryItems.Events;

/// <summary>
/// Class InventoryUnitConversion.
/// Implements the <see cref="Aggregate" />
/// Implements the <see cref="IAggregate" />
/// Implements the <see cref="IEquatable{Aggregate}" />
/// Implements the <see cref="IEquatable{InventoryUnitConversion}" />.
/// </summary>
/// <seealso cref="Aggregate" />
/// <seealso cref="IAggregate" />
/// <seealso cref="IEquatable{Aggregate}" />
/// <seealso cref="IEquatable{InventoryUnitConversion}" />
public record InventoryItemBarcode(
    string Id,
    string UnitId,
    decimal Quantity,
    bool IsDefaultBarcode)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemBarcode" /> class.
    /// </summary>
    /// <param name="barcode">The InventoryUnitConversion.</param>
    public InventoryItemBarcode(InventoryItemBarcodeAssigned barcode)
        : this(
              (barcode ?? throw new ArgumentNullException(nameof(barcode))).Id,
              barcode.UnitId,
              barcode.Quantity,
              barcode.IsDefaultBarcode)
    {
    }

    /// <summary>
    /// Applies the specified inventory item.
    /// </summary>
    /// <param name="inventoryItem">The inventory item.</param>
    /// <param name="barcodeEvent">The barcode event.</param>
    /// <returns>InventoryItemBarcode.</returns>
    public InventoryItemBarcode Apply(InventoryItem inventoryItem, InventoryItemBarcodeEvent barcodeEvent)
    {
        return barcodeEvent switch
        {
            InventoryItemDefaultBarcodeAssigned => this with { IsDefaultBarcode = true },
            InventoryItemDefaultBarcodeCleared => this with { IsDefaultBarcode = false },
            InventoryItemBarcodeRatioAdjusted adjusted => this with
            {
                UnitId = adjusted.UnitId,
                Quantity = adjusted.Quantity,
            },
            _ => throw new InvalidAggregateEventException(inventoryItem, barcodeEvent, false),
        };
    }
}