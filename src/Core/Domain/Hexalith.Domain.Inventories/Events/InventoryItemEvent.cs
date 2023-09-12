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

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Class InventoryItemEvent.
/// Implements the <see cref="Hexalith.Domain.Events.BaseEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.BaseEvent" />
[DataContract]
public abstract class InventoryItemEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemEvent"/> class.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected InventoryItemEvent(string companyId, string id)
    {
        CompanyId = companyId;
        Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemEvent" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    protected InventoryItemEvent() => CompanyId = Id = string.Empty;

    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string Id { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => InventoryItem.GetAggregateId(CompanyId, Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => InventoryItem.GetAggregateName();
}