// <copyright file="PartnerInventoryItem.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.PartnerInventoryItems.Aggregates;

using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.PartnerInventoryItems.Events;

/// <summary>
/// Class PartnerInventoryItem.
/// Implements the <see cref="Aggregate" />
/// Implements the <see cref="IAggregate" />
/// Implements the <see cref="IEquatable{Aggregate}" />
/// Implements the <see cref="IEquatable{PartnerInventoryItem}" />.
/// </summary>
/// <seealso cref="Aggregate" />
/// <seealso cref="IAggregate" />
/// <seealso cref="IEquatable{Aggregate}" />
/// <seealso cref="IEquatable{PartnerInventoryItem}" />
[DataContract]
public record PartnerInventoryItem(
    [property: DataMember] string PartitionId,
    [property: DataMember] string CompanyId,
    [property: DataMember] string OriginId,
    [property: DataMember] string PartnerType,
    [property: DataMember] string PartnerId,
    [property: DataMember] string Id,
    [property: DataMember] string InventoryItemId,
    [property: DataMember] string UnitId,
    [property: DataMember] string? Name,
    [property: DataMember] decimal? Price,
    [property: DataMember] string? CountryOfOriginId,
    [property: DataMember] string? HarmonizedTariffScheduleCode,
    [property: DataMember] string? ProductType,
    [property: DataMember] bool Disabled) : Aggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerInventoryItem" /> class.
    /// </summary>
    public PartnerInventoryItem()
        : this(
              string.Empty,
              string.Empty,
              string.Empty,
              string.Empty,
              string.Empty,
              string.Empty,
              string.Empty,
              string.Empty,
              null,
              null,
              null,
              null,
              null,
              false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerInventoryItem" /> class.
    /// </summary>
    /// <param name="inventoryItem">The PartnerInventoryItem.</param>
    public PartnerInventoryItem(PartnerInventoryItemAdded inventoryItem)
        : this(
              (inventoryItem ?? throw new ArgumentNullException(nameof(inventoryItem))).PartitionId,
              inventoryItem.CompanyId,
              inventoryItem.OriginId,
              inventoryItem.PartnerType,
              inventoryItem.PartnerId,
              inventoryItem.Id,
              inventoryItem.InventoryItemId,
              inventoryItem.UnitId,
              inventoryItem.Name,
              inventoryItem.Price,
              inventoryItem.CountryOfOriginId,
              inventoryItem.HarmonizedTariffScheduleCode,
              inventoryItem.ProductType,
              false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerInventoryItem" /> class.
    /// </summary>
    /// <param name="inventoryItem">The PartnerInventoryItem.</param>
    public PartnerInventoryItem(PartnerInventoryItemChanged inventoryItem)
        : this(
            (inventoryItem ?? throw new ArgumentNullException(nameof(inventoryItem))).PartitionId,
            inventoryItem.CompanyId,
            inventoryItem.OriginId,
            inventoryItem.PartnerType,
            inventoryItem.PartnerId,
            inventoryItem.Id,
            inventoryItem.InventoryItemId,
            inventoryItem.UnitId,
            inventoryItem.Name,
            inventoryItem.Price,
            inventoryItem.CountryOfOriginId,
            inventoryItem.HarmonizedTariffScheduleCode,
            inventoryItem.ProductType,
            false)
    {
    }

    /// <inheritdoc/>
    public override (IAggregate Aggregate, IEnumerable<BaseEvent> Events) Apply(BaseEvent domainEvent)
    {
        return domainEvent switch
        {
            PartnerInventoryItemChanged changed => (new PartnerInventoryItem(changed), [domainEvent]),
            PartnerInventoryItemRemoved => (this with { Disabled = true }, [domainEvent]),
            PartnerInventoryItemAdded added => (IsInitialized()
                ? throw new InvalidAggregateEventException(this, domainEvent, true)
                : new PartnerInventoryItem(added), [domainEvent]),
            _ => base.Apply(domainEvent),
        };
    }

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="partnerType">Type of the partner.</param>
    /// <param name="partnerId">The partner identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <returns>System.String.</returns>
    public static string GetAggregateId(
        string partitionId,
        string companyId,
        string originId,
        string partnerType,
        string partnerId,
        string id)
        => Normalize(GetAggregateName() + Separator + partitionId + Separator + companyId + Separator + originId + Separator + partnerType + Separator + partnerId + Separator + id);

    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string GetAggregateName() => nameof(PartnerInventoryItem);

    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Id) && Disabled;

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => GetAggregateId(PartitionId, CompanyId, OriginId, PartnerType, PartnerId, Id);
}