// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="InventoryItemStockCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Inventories.InventoryItemStocks.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Organizations.Commands;
using Hexalith.Application.Organizations.Notifications;
using Hexalith.Domain.InventoryItemStocks.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Class InventoryItemStockCommand.
/// Implements the <see cref="CompanyCommand" />.
/// </summary>
/// <seealso cref="CompanyCommand" />
[DataContract]
[Serializable]
public abstract class InventoryItemStockCommand : CompanyEntityCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemStockCommand"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="locationId">The location identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected InventoryItemStockCommand(
        string partitionId,
        string companyId,
        string originId,
        string locationId,
        string id)
        : base(partitionId, companyId, originId, id) => LocationId = locationId;

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemStockCommand" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected InventoryItemStockCommand() => LocationId = string.Empty;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public string LocationId { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => InventoryItemStock.GetAggregateId(PartitionId, CompanyId, OriginId, LocationId, Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => InventoryItemStock.GetAggregateName();
}