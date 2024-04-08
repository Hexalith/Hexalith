// <copyright file="Dynamics365LogisticsPartnerCatalogItemNameChanged.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.BusinessEvents;

using System.Runtime.Serialization;

using Hexalith.Application.Commands;
using Hexalith.Application.Inventories.PartnerInventoryItems.Commands;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories;

/// <summary>
/// This event is sent each time the item name changes in Dynamics 365 for finance and operations.
/// </summary>
[DataContract]
public class Dynamics365FinancePartnerInventoryItemNameChanged : Dynamics365FinancePartnerInventoryItemEvent
{
    /// <summary>
    /// Gets or sets the new item name.
    /// </summary>
    [DataMember]
    public string? NewName { get; set; }

    /// <summary>
    /// Gets or sets the previous item name.
    /// </summary>
    [DataMember]
    public string? PreviousName { get; set; }

    /// <inheritdoc/>
    public override IEnumerable<BaseCommand> ToCommands()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(BusinessEventLegalEntity);
        ArgumentException.ThrowIfNullOrWhiteSpace(LogisticsPartnerId);
        ArgumentException.ThrowIfNullOrWhiteSpace(BarCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(ItemId);
        ArgumentException.ThrowIfNullOrWhiteSpace(RetailVariantId);
        ArgumentException.ThrowIfNullOrWhiteSpace(NewName);
        ArgumentException.ThrowIfNullOrWhiteSpace(PartitionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(OriginId);
        return [new ChangePartnerInventoryItemName(
            PartitionId,
            BusinessEventLegalEntity,
            OriginId,
            Dynamics365FinanceInventoriesConstants.PartnerType,
            LogisticsPartnerId,
            BarCode,
            NewName)
            ];
    }

    /// <inheritdoc/>
    protected override string DefaultTypeName() => "FFYLogisticsPartnerCatalogItemNameChanged";
}