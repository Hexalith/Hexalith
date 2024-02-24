// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryItemConversionEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.InventoryUnitConversions.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Events;
using Hexalith.Domain.InventoryUnitConversions.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Class InventoryUnitConversionEvent.
/// Implements the <see cref="Domain.Events.CompanyEntityEvent" />.
/// </summary>
/// <seealso cref="Domain.Events.CompanyEntityEvent" />
[DataContract]
[Serializable]
public abstract class InventoryUnitConversionEvent : CompanyEntityEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnitConversionEvent" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="toUnitId">To unit identifier.</param>
    /// <param name="inventoryItemId">The inventory item identifier.</param>
    [JsonConstructor]
    protected InventoryUnitConversionEvent(
        string partitionId,
        string companyId,
        string originId,
        string id,
        string toUnitId,
        string? inventoryItemId)
        : base(partitionId, companyId, originId, id)
    {
        ToUnitId = toUnitId;
        InventoryItemId = inventoryItemId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnitConversionEvent" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected InventoryUnitConversionEvent() => ToUnitId = string.Empty;

    /// <summary>
    /// Gets the inventory item identifier.
    /// </summary>
    /// <value>The inventory item identifier.</value>
    [DataMember(Order = 11)]
    public string? InventoryItemId { get; }

    /// <summary>
    /// Gets converts to unitId.
    /// </summary>
    /// <value>To unit identifier.</value>
    [DataMember(Order = 10)]
    public string ToUnitId { get; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => InventoryUnitConversion.GetAggregateId(PartitionId, CompanyId, OriginId, Id, ToUnitId, InventoryItemId);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => InventoryUnitConversion.GetAggregateName();
}