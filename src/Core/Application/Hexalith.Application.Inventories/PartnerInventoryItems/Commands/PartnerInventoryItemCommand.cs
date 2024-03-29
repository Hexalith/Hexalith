// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="PartnerInventoryItemCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.PartnerInventoryItems.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Organizations.Commands;
using Hexalith.Domain.PartnerInventoryItems.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Class PartnerInventoryItemCommand.
/// Implements the <see cref="Domain.Commands.CompanyEntityCommand" />.
/// </summary>
/// <seealso cref="Domain.Commands.CompanyEntityCommand" />
[DataContract]
[Serializable]
public abstract class PartnerInventoryItemCommand : CompanyEntityCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerInventoryItemCommand" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="partnerType">Name of the partner.</param>
    /// <param name="partnerId">The partner identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected PartnerInventoryItemCommand(
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
    /// Initializes a new instance of the <see cref="PartnerInventoryItemCommand" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected PartnerInventoryItemCommand()
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