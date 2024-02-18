// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="AddPartnerInventoryItem.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.PartnerInventoryItems.Commands;

using System.Runtime.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class AddPartnerInventoryItem.
/// Implements the <see cref="PartnerInventoryItemCommand" />.
/// </summary>
/// <seealso cref="PartnerInventoryItemCommand" />
[DataContract]
[Serializable]
public class AddPartnerInventoryItem : PartnerInventoryItemCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddPartnerInventoryItem" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="partnerType">Type of the partner.</param>
    /// <param name="partnerId">The partner identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="inventoryItemId">The inventory item identifier.</param>
    /// <param name="unitId">The unit identifier.</param>
    public AddPartnerInventoryItem(
        string partitionId,
        string companyId,
        string originId,
        string partnerType,
        string partnerId,
        string id,
        string inventoryItemId,
        string unitId)
        : base(partitionId, companyId, originId, partnerType, partnerId, id)
    {
        InventoryItemId = inventoryItemId;
        UnitId = unitId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddPartnerInventoryItem" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public AddPartnerInventoryItem() => InventoryItemId = UnitId = string.Empty;

    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    /// <value>The date.</value>
    [DataMember(Order = 20)]
    public string InventoryItemId { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 21)]
    public string UnitId { get; set; }
}