// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="InventoryItemStockEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Class InventoryItemStockEvent.
/// Implements the <see cref="Hexalith.Domain.Events.BaseEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.BaseEvent" />
[DataContract]
public abstract class InventoryItemStockEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemStockEvent" /> class.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="locationId">The location identifier.</param>
    /// <param name="inventoryItemId">The inventory item identifier.</param>
    [JsonConstructor]
    protected InventoryItemStockEvent(string companyId, string locationId, string inventoryItemId)
    {
        CompanyId = companyId;
        LocationId = locationId;
        InventoryItemId = inventoryItemId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemStockEvent" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    protected InventoryItemStockEvent() => CompanyId = LocationId = InventoryItemId = string.Empty;

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