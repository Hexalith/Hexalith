// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="ExternalSystemEvent.cs" company="Fiveforty SAS Paris France">
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
/// Class CustomerEvent.
/// Implements the <see cref="Hexalith.Domain.Events.BaseEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.BaseEvent" />
[DataContract]
public abstract class ExternalSystemEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemEvent" /> class.
    /// </summary>
    /// <param name="systemId">The identifier.</param>
    /// <param name="referenceAggregateName">Type of the aggregate.</param>
    /// <param name="externalId">The external identifier.</param>
    [JsonConstructor]
    protected ExternalSystemEvent(
        string systemId,
        string externalId,
        string referenceAggregateName)
    {
        SystemId = systemId;
        ReferenceAggregateName = referenceAggregateName;
        ExternalId = externalId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemEvent" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    protected ExternalSystemEvent() => SystemId = ReferenceAggregateName = ExternalId = string.Empty;

    /// <summary>
    /// Gets or sets the external identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string ExternalId { get; set; }

    /// <summary>
    /// Gets or sets the aggregate type name.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string ReferenceAggregateName { get; set; }

    /// <summary>
    /// Gets or sets the system identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string SystemId { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => base.DefaultAggregateId() + Separator + SystemId + Separator + ReferenceAggregateName + Separator + ExternalId;

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => nameof(ExternalSystemReference);
}