// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryItemBarcodes.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.InventoryItems.Entities;

using System.Collections.Immutable;

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
public static class InventoryItemBarcodes
{
    /// <inheritdoc/>
    public static InventoryItem ApplyBarcodeEvent(this InventoryItem aggregate, InventoryItemBarcodeEvent barcodeEvent)
    {
        ArgumentNullException.ThrowIfNull(barcodeEvent);
        ArgumentNullException.ThrowIfNull(aggregate);
        ImmutableList<InventoryItemBarcode> barcodesWithoutCurrent = aggregate.Barcodes.Where(p => p.Id == barcodeEvent.Barcode).ToImmutableList();
        if (barcodeEvent is InventoryItemBarcodeDisassociated)
        {
            return aggregate with
            {
                Barcodes = barcodesWithoutCurrent,
            };
        }

        if (barcodeEvent is InventoryItemBarcodeAssigned assigned)
        {
            return aggregate with
            {
                Barcodes = barcodesWithoutCurrent.Add(new InventoryItemBarcode(assigned)),
            };
        }

        InventoryItemBarcode? current = aggregate.Barcodes.FirstOrDefault(p => p.Id == barcodeEvent.Barcode);
        return current is null
            ? throw new InvalidAggregateEventException(
                aggregate,
                barcodeEvent,
                false,
                $"The barcode {barcodeEvent.Barcode} does not exist for inventory item {barcodeEvent.Id}.")
            : aggregate with
            {
                Barcodes = barcodesWithoutCurrent.Add(current.Apply(aggregate, barcodeEvent)),
            };
    }
}