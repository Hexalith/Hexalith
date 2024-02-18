// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
// ***********************************************************************
// <copyright file="InventoryItemEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.InventoryItem.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.InventoryItems.Aggregates;
using Hexalith.Domain.Organizations.Events;
using Hexalith.Extensions;

/// <summary>
/// Class InventoryItemEvent.
/// Implements the <see cref="Domain.Events.CompanyEntityEvent" />.
/// </summary>
/// <seealso cref="Domain.Events.CompanyEntityEvent" />
[DataContract]
[Serializable]
public abstract class InventoryItemEvent : CompanyEntityEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemEvent"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected InventoryItemEvent(string partitionId, string companyId, string originId, string id)
        : base(partitionId, companyId, originId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemEvent" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected InventoryItemEvent()
    {
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => InventoryItem.GetAggregateId(PartitionId, CompanyId, OriginId, Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => InventoryItem.GetAggregateName();
}