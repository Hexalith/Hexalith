// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="InventoryItem.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Aggregates;

using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class InventoryItem.
/// Implements the <see cref="Hexalith.Domain.Aggregates.Aggregate" />
/// Implements the <see cref="Hexalith.Domain.Aggregates.IAggregate" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.InventoryItem}" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Aggregates.Aggregate" />
/// <seealso cref="Hexalith.Domain.Aggregates.IAggregate" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.InventoryItem}" />
public record InventoryItem(
    string PartitionId,
    string CompanyId,
    string OriginId,
    string Id,
    string Name,
    IEnumerable<ItemBarcode>? Barcodes,
    DateTimeOffset Date) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItem" /> class.
    /// </summary>
    /// <param name="inventoryItem">The InventoryItem.</param>
    public InventoryItem(InventoryItemAdded inventoryItem)
        : this(
              (inventoryItem ?? throw new ArgumentNullException(nameof(inventoryItem))).PartitionId,
              inventoryItem.CompanyId,
              inventoryItem.OriginId,
              inventoryItem.Id,
              inventoryItem.Name,
              null,
              inventoryItem.Date)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItem" /> class.
    /// </summary>
    /// <param name="inventoryItem">The InventoryItem.</param>
    public InventoryItem(InventoryItemInformationChanged inventoryItem)
        : this(
              (inventoryItem ?? throw new ArgumentNullException(nameof(inventoryItem))).PartitionId,
              inventoryItem.CompanyId,
              inventoryItem.OriginId,
              inventoryItem.Id,
              inventoryItem.Name,
              null,
              inventoryItem.Date)
    {
    }

    /// <inheritdoc/>
    public override IAggregate Apply(BaseEvent domainEvent)
    {
        return domainEvent switch
        {
            InventoryItemInformationChanged changed => new InventoryItem(changed),
            InventoryItemBarcodeAdded barcodeAdded => AddBarcode(barcodeAdded),
            InventoryItemBarcodeRemoved barcodeRemoved => RemoveBarcode(barcodeRemoved),
            InventoryItemAdded => throw new InvalidAggregateEventException(this, domainEvent, true),
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        };
    }

    private InventoryItem RemoveBarcode(InventoryItemBarcodeRemoved barcodeRemoved)
        => this with { Barcodes = Barcodes?.Where(p => p.Code != barcodeRemoved.Barcode).ToList() };

    private InventoryItem AddBarcode(InventoryItemBarcodeAdded barcodeAdded)
    {
        // If the barcode is already in the list, we remove it
        IEnumerable<ItemBarcode> barcodes = (Barcodes == null)
            ? barcodeAdded.Barcode.IntoArray()
            : Barcodes
                .Where(p => p.Code != barcodeAdded.Barcode.Code)
                    .ToList()
                    .Union(barcodeAdded.Barcode.IntoArray());

        // If the barcode is set as default, we remove the previous default one
        if (barcodeAdded.Barcode.IsDefault == true)
        {
            barcodes = barcodes

                // Get list of previous barcodes without the default one
                .Where(p => p.IsDefault == false && p.Code != barcodeAdded.Barcode.Code)

                // Add the previous default one as not default
                .Union(barcodes
                    .Where(p => p.IsDefault == true && p.Code != barcodeAdded.Barcode.Code)
                    .Select(w => new ItemBarcode(w.Code, false)))
                .ToList();
        }

        return this with { Barcodes = barcodes };
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => GetAggregateId(PartitionId, CompanyId, OriginId, Id);

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <returns>string.</returns>
    public static string GetAggregateId(string partitionId, string companyId, string originId, string id)
        => Normalize(GetAggregateName() + Separator + partitionId + Separator + companyId + Separator + originId + Separator + id);

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <returns>System.String.</returns>
#pragma warning disable CA1024 // Use properties where appropriate
    public static string GetAggregateName() => nameof(InventoryItem);
#pragma warning restore CA1024 // Use properties where appropriate
}