// <copyright file="PartnerInventoryItemChanged.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.PartnerInventoryItems.Events;

using System.Runtime.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class PartnerInventoryItemInformationChanged.
/// Implements the <see cref="PartnerInventoryItemEvent" />.
/// </summary>
/// <seealso cref="PartnerInventoryItemEvent" />
[DataContract]
[Serializable]
public class PartnerInventoryItemChanged : PartnerInventoryItemEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerInventoryItemChanged" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="partnerType">Type of the partner.</param>
    /// <param name="partnerId">The partner identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="inventoryItemId">The inventory item identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="unitId">The unit identifier.</param>
    /// <param name="price">The price.</param>
    /// <param name="countryOfOriginId">The country of origin identifier.</param>
    /// <param name="harmonizedTariffScheduleCode">The harmonized tariff schedule code.</param>
    /// <param name="productType">Type of the product.</param>
    public PartnerInventoryItemChanged(
        string partitionId,
        string companyId,
        string originId,
        string partnerType,
        string partnerId,
        string id,
        string inventoryItemId,
        string unitId,
        string? name,
        decimal? price,
        string? countryOfOriginId,
        string? harmonizedTariffScheduleCode,
        string? productType)
        : base(partitionId, companyId, originId, partnerType, partnerId, id)
    {
        InventoryItemId = inventoryItemId;
        UnitId = unitId;
        Name = name;
        Price = price;
        CountryOfOriginId = countryOfOriginId;
        HarmonizedTariffScheduleCode = harmonizedTariffScheduleCode;
        ProductType = productType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerInventoryItemChanged" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public PartnerInventoryItemChanged() => InventoryItemId = UnitId = string.Empty;

    /// <summary>
    /// Gets or sets the country of origin identifier.
    /// </summary>
    /// <value>The country of origin identifier.</value>
    [DataMember(Order = 24)]
    public string? CountryOfOriginId { get; set; }

    /// <summary>
    /// Gets or sets the harmonized tariff schedule code.
    /// </summary>
    /// <value>The harmonized tariff schedule code.</value>
    [DataMember(Order = 25)]
    public string? HarmonizedTariffScheduleCode { get; set; }

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    /// <value>The date.</value>
    [DataMember(Order = 21)]
    public string InventoryItemId { get; set; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 20)]
    public string? Name { get; }

    /// <summary>
    /// Gets the price.
    /// </summary>
    /// <value>The price.</value>
    [DataMember(Order = 23)]
    public decimal? Price { get; }

    /// <summary>
    /// Gets or sets the type of the product.
    /// </summary>
    /// <value>The type of the product.</value>
    [DataMember(Order = 26)]
    public string? ProductType { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 22)]
    public string UnitId { get; set; }
}