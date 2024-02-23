// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="Product.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Products.Aggregates;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.InventoryItems.Entities;
using Hexalith.Domain.InventoryItems.Events;

/// <summary>
/// Class InventoryItem.
/// Implements the <see cref="Aggregate" />
/// Implements the <see cref="IAggregate" />
/// Implements the <see cref="IEquatable{Aggregate}" />
/// Implements the <see cref="IEquatable{InventoryItem}" />.
/// </summary>
/// <seealso cref="Aggregate" />
/// <seealso cref="IAggregate" />
/// <seealso cref="IEquatable{Aggregate}" />
/// <seealso cref="IEquatable{InventoryItem}" />
public record Product(
    string PartitionId,
    string CompanyId,
    string OriginId,
    string Id,
    string Name,
    string? Description,
    IEnumerable<InventoryItemBarcode> Barcodes) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Product" /> class.
    /// </summary>
    /// <param name="inventoryItem">The InventoryItem.</param>
    public Product(InventoryItemAdded inventoryItem)
        : this(
              (inventoryItem ?? throw new ArgumentNullException(nameof(inventoryItem))).PartitionId,
              inventoryItem.CompanyId,
              inventoryItem.OriginId,
              inventoryItem.Id,
              inventoryItem.Name,
              inventoryItem.Description,
              [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    public Product()
        : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Product" /> class.
    /// </summary>
    /// <param name="inventoryItem">The InventoryItem.</param>
    public Product(InventoryItemInformationChanged inventoryItem)
        : this(
              (inventoryItem ?? throw new ArgumentNullException(nameof(inventoryItem))).PartitionId,
              inventoryItem.CompanyId,
              inventoryItem.OriginId,
              inventoryItem.Id,
              inventoryItem.Name,
              inventoryItem.Description,
              [])
    {
    }

    /// <inheritdoc/>
    public override IAggregate Apply(BaseEvent domainEvent)
    {
        return domainEvent switch
        {
            InventoryItemBarcodeEvent barcodeEvent => this.ApplyBarcodeEvent(barcodeEvent),
            InventoryItemInformationChanged changed => new Product(changed),
            InventoryItemAdded => throw new InvalidAggregateEventException(this, domainEvent, true),
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        };
    }

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
    public static string GetAggregateName() => nameof(Product);

    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => GetAggregateId(PartitionId, CompanyId, OriginId, Id);
}