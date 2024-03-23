// <copyright file="Dynamics365FinancePartnerInventoryItemEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.BusinessEvents;

using System.Runtime.Serialization;

using Hexalith.Domain.PartnerInventoryItems.Aggregates;
using Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;

/// <summary>
/// This is the base class for logistics partner catalog events from Dynamics 365 for finance and operations.
/// </summary>
[DataContract]
public abstract class Dynamics365FinancePartnerInventoryItemEvent : Dynamics365BusinessEventBase
{
    /// <inheritdoc/>
    public override string AggregateId => PartnerInventoryItem.GetAggregateId(
        PartitionId ?? string.Empty,
        BusinessEventLegalEntity ?? string.Empty,
        OriginId ?? string.Empty,
        "Logistics",
        LogisticsPartnerId ?? string.Empty,
        BarCode ?? string.Empty);

    /// <inheritdoc/>
    public override string AggregateName => PartnerInventoryItem.GetAggregateName();

    /// <summary>
    /// Gets or sets the item primary barcode.
    /// </summary>
    /// <value>The bar code.</value>
    [DataMember]
    public string? BarCode { get; set; }

    /// <summary>
    /// Gets or sets the item identifier.
    /// </summary>
    /// <value>The item identifier.</value>
    [DataMember]
    public string? ItemId { get; set; }

    /// <summary>
    /// Gets or sets the logistics partner identifier.
    /// </summary>
    /// <value>The logistics partner identifier.</value>
    public string? LogisticsPartnerId { get; set; }

    /// <summary>
    /// Gets or sets the item color identifier.
    /// </summary>
    /// <value>The product color identifier.</value>
    [DataMember]
    public string? ProductColorId { get; set; }

    /// <summary>
    /// Gets or sets the item size identifier.
    /// </summary>
    /// <value>The product size identifier.</value>
    [DataMember]
    public string? ProductSizeId { get; set; }

    /// <summary>
    /// Gets or sets the item style identifier.
    /// </summary>
    /// <value>The product style identifier.</value>
    [DataMember]
    public string? ProductStyleId { get; set; }

    /// <summary>
    /// Gets or sets the item variant linked to the barcode.
    /// </summary>
    /// <value>The retail variant identifier.</value>
    [DataMember]
    public string? RetailVariantId { get; set; }
}