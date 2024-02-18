// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="RemovePartnerInventoryItem.cs" company="Fiveforty SAS Paris France">
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
/// Class RemovePartnerInventoryItem.
/// Implements the <see cref="PartnerInventoryItemCommand" />.
/// </summary>
/// <seealso cref="PartnerInventoryItemCommand" />
[DataContract]
[Serializable]
public class RemovePartnerInventoryItem : PartnerInventoryItemCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemovePartnerInventoryItem" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="partnerType">Type of the partner.</param>
    /// <param name="partnerId">The partner identifier.</param>
    /// <param name="id">The identifier.</param>
    public RemovePartnerInventoryItem(
        string partitionId,
        string companyId,
        string originId,
        string partnerType,
        string partnerId,
        string id)
        : base(partitionId, companyId, originId, partnerType, partnerId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemovePartnerInventoryItem" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public RemovePartnerInventoryItem()
    {
    }
}