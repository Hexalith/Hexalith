// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-17-2024
// ***********************************************************************
// <copyright file="PartnerInventoryItemEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.PartnerInventoryItems.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Organizations.Events;
using Hexalith.Domain.PartnerInventoryItems.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Class PartnerInventoryItemEvent.
/// Implements the <see cref="Domain.Events.CompanyEntityEvent" />.
/// </summary>
/// <seealso cref="Domain.Events.CompanyEntityEvent" />
[DataContract]
[Serializable]
public abstract class PartnerInventoryItemEvent : CompanyEntityEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerInventoryItemEvent" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="partnerId">The partner identifier.</param>
    /// <param name="partnerType">Name of the partner.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected PartnerInventoryItemEvent(
        string partitionId,
        string companyId,
        string originId,
        string partnerType,
        string partnerId,
        string id)
        : base(partitionId, companyId, originId, id)
    {
        PartnerId = partnerId;
        PartnerType = partnerType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerInventoryItemEvent" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected PartnerInventoryItemEvent()
    {
        PartnerId = string.Empty;
        PartnerType = string.Empty;
    }

    /// <summary>
    /// Gets or sets the partner identifier.
    /// </summary>
    /// <value>The partner identifier.</value>
    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public string PartnerId { get; set; }

    /// <summary>
    /// Gets or sets the type of the partner.
    /// </summary>
    /// <value>The type of the partner.</value>
    [DataMember(Order = 11)]
    [JsonPropertyOrder(11)]
    public string PartnerType { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => PartnerInventoryItem.GetAggregateId(PartitionId, CompanyId, OriginId, PartnerType, PartnerId, Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => PartnerInventoryItem.GetAggregateName();
}