// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryUnitEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.InventoryUnits.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.InventoryUnits.Aggregates;
using Hexalith.Domain.Organizations.Events;
using Hexalith.Extensions;

/// <summary>
/// Class InventoryUnitConversionEvent.
/// Implements the <see cref="Domain.Events.CompanyEntityEvent" />.
/// </summary>
/// <seealso cref="Domain.Events.CompanyEntityEvent" />
[DataContract]
[Serializable]
public abstract class InventoryUnitEvent : CompanyEntityEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnitEvent" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected InventoryUnitEvent(
        string partitionId,
        string companyId,
        string originId,
        string id)
        : base(partitionId, companyId, originId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnitEvent" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected InventoryUnitEvent()
    {
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => InventoryUnit.GetAggregateId(PartitionId, CompanyId, OriginId, Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => InventoryUnit.GetAggregateName();
}