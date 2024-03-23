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

namespace Hexalith.Domain.InventoryItems.Aggregates;

using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.InventoryItems.Entities;
using Hexalith.Domain.InventoryItems.Events;
using Hexalith.Domain.ValueObjects;

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
[DataContract]
public record InventoryItem(
    [property: DataMember] string PartitionId,
    [property: DataMember] string CompanyId,
    [property: DataMember] string OriginId,
    [property: DataMember] string Id,
    [property: DataMember] IEnumerable<DimensionValue>? Dimensions,
    [property: DataMember] string Name,
    [property: DataMember] string? Description,
    [property: DataMember] IEnumerable<InventoryItemBarcode>? Barcodes) : Aggregate
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
              inventoryItem.Dimensions,
              inventoryItem.Name,
              inventoryItem.Description,
              [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItem"/> class.
    /// </summary>
    public InventoryItem()
        : this(string.Empty, string.Empty, string.Empty, string.Empty, null, string.Empty, null, null)
    {
    }

    /// <inheritdoc/>
    public override (IAggregate Aggregate, IEnumerable<BaseEvent> Events) Apply(BaseEvent domainEvent)
    {
        return (domainEvent switch
        {
            InventoryItemBarcodeEvent barcodeEvent => this.ApplyBarcodeEvent(barcodeEvent),
            InventoryItemDescriptionChanged changed => this with { Name = changed.Name, Description = changed.Description },
            InventoryItemAdded => throw new InvalidAggregateEventException(this, domainEvent, true),
            _ => throw new InvalidAggregateEventException(this, domainEvent, false),
        }, []);
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
    public static string GetAggregateName() => nameof(InventoryItem);

    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => GetAggregateId(PartitionId, CompanyId, OriginId, Id);
}