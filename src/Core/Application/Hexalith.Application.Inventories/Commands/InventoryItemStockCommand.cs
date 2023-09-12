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

namespace Hexalith.Application.Inventories.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Commands;
using Hexalith.Domain.Aggregates;

/// <summary>
/// Class InventoryItemStockCommand.
/// Implements the <see cref="Domain.Commands.BaseCommand" />.
/// </summary>
/// <seealso cref="Domain.Commands.BaseCommand" />
[DataContract]
public abstract class InventoryItemStockCommand : BaseCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemStockCommand" /> class.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="locationId">The location identifier.</param>
    /// <param name="inventoryItemId">The inventory item identifier.</param>
    [JsonConstructor]
    protected InventoryItemStockCommand(string companyId, string locationId, string inventoryItemId)
    {
        CompanyId = companyId;
        LocationId = locationId;
        InventoryItemId = inventoryItemId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemStockCommand" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    protected InventoryItemStockCommand() => CompanyId = LocationId = InventoryItemId = string.Empty;

    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string CompanyId { get; set; }

    /// <summary>
    /// Gets the inventory item identifier.
    /// </summary>
    /// <value>The inventory item identifier.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string InventoryItemId { get; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string LocationId { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => InventoryItemStock.GetAggregateId(CompanyId, LocationId, InventoryItemId);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => InventoryItemStock.GetAggregateName();
}