// <copyright file="Dynamics365FinancePartnerInventoryItemAdded.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.BusinessEvents;

using System.Runtime.Serialization;

using Hexalith.Application.Commands;
using Hexalith.Application.Inventories.PartnerInventoryItems.Commands;

/// <summary>
/// This event is sent each time a new item is added to the external logistics partner catalog.
/// </summary>
[DataContract]
public class Dynamics365FinancePartnerInventoryItemAdded : Dynamics365FinancePartnerInventoryItemEvent
{
    /// <summary>
    /// Gets or sets the item color.
    /// </summary>
    [DataMember]
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets country of origin ISO 3166-1-alpha-3 code. The origin is where the product was “wholly
    /// obtained” or “substantially transformed”.
    /// </summary>
    [DataMember]
    public string? CountryOfOriginCode { get; set; }

    /// <summary>
    /// Gets or sets hTS (Harmonized Tariff Schedule) codes are product classification codes between 8-10 digits.
    /// The first six digits are an HS code, and the countries of import assign the subsequent
    /// digits to provide additional classification. U.S. HTS codes are 10 digits and are
    /// administered by the U.S. International Trade Commission.
    /// </summary>
    [DataMember]
    public string? HarmonizedTariffScheduleCode { get; set; }

    /// <summary>
    /// Gets or sets the item name.
    /// </summary>
    [DataMember]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the item price.
    /// </summary>
    [DataMember]
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets product type.
    /// </summary>
    [DataMember]
    public string? ProductType { get; set; }

    /// <summary>
    /// Gets or sets product size.
    /// </summary>
    [DataMember]
    public string? Size { get; set; }

    /// <summary>
    /// Gets or sets product style.
    /// </summary>
    [DataMember]
    public string? Style { get; set; }

    /// <inheritdoc/>
    public override IEnumerable<BaseCommand> ToCommands()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(BusinessEventLegalEntity);
        ArgumentException.ThrowIfNullOrWhiteSpace(LogisticsPartnerId);
        ArgumentException.ThrowIfNullOrWhiteSpace(BarCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(RetailVariantId);
        ArgumentException.ThrowIfNullOrWhiteSpace(Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(CountryOfOriginCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(HarmonizedTariffScheduleCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(PartitionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(OriginId);
        return [new AddOrChangePartnerInventoryItem(
            PartitionId,
            BusinessEventLegalEntity,
            OriginId,
            Dynamics365FinanceInventoriesConstants.PartnerType,
            LogisticsPartnerId,
            BarCode,
            RetailVariantId,
            Name,
            string.Empty,
            Price,
            CountryOfOriginCode,
            HarmonizedTariffScheduleCode,
            ProductType)];
    }

    /// <inheritdoc/>
    protected override string DefaultTypeName() => "FFYLogisticsPartnerCatalogItemAdded";
}