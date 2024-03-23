// <copyright file="Dynamics365FinancePartnerInventoryItemRemoved.cs" company="Fiveforty SAS Paris France">
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
/// This event is sent each time an item is removed from the external logistics partner catalog in
/// Dynamics 365 for finance and operations.
/// </summary>
[DataContract]
public class Dynamics365FinancePartnerInventoryItemRemoved : Dynamics365FinancePartnerInventoryItemEvent
{
    /// <inheritdoc/>
    public override IEnumerable<BaseCommand> ToCommands()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(BusinessEventLegalEntity);
        ArgumentException.ThrowIfNullOrWhiteSpace(LogisticsPartnerId);
        ArgumentException.ThrowIfNullOrWhiteSpace(BarCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(ItemId);
        ArgumentException.ThrowIfNullOrWhiteSpace(RetailVariantId);
        ArgumentException.ThrowIfNullOrWhiteSpace(PartitionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(OriginId);

        return [new RemovePartnerInventoryItem(
            PartitionId,
            BusinessEventLegalEntity,
            OriginId,
            Dynamics365FinanceInventoriesConstants.PartnerType,
            LogisticsPartnerId,
            BarCode)
            ];
    }

    /// <inheritdoc/>
    protected override string DefaultTypeName() => "FFYLogisticsPartnerCatalogItemRemoved";
}