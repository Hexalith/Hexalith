// <copyright file="PartnerInventoryItemNameChanged.cs" company="Fiveforty SAS Paris France">
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
public class PartnerInventoryItemNameChanged : PartnerInventoryItemEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerInventoryItemNameChanged"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="partnerType">Type of the partner.</param>
    /// <param name="partnerId">The partner identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    public PartnerInventoryItemNameChanged(
        string partitionId,
        string companyId,
        string originId,
        string partnerType,
        string partnerId,
        string id,
        string? name)
        : base(partitionId, companyId, originId, partnerType, partnerId, id) => Name = name;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerInventoryItemNameChanged" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public PartnerInventoryItemNameChanged()
    {
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 20)]
    public string? Name { get; set; }
}