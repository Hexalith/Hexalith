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
using Hexalith.Extensions;

/// <summary>
/// Class CustomerEvent.
/// Implements the <see cref="Hexalith.Domain.Events.BaseEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.BaseEvent" />
[DataContract]
[Serializable]
public abstract class ExternalSystemEvent : PartitionedEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemEvent"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="referenceAggregateName">Name of the reference aggregate.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <param name="referenceAggregateId">The reference aggregate identifier.</param>
    [JsonConstructor]
    protected ExternalSystemEvent(
        string partitionId,
        string companyId,
        string systemId,
        string referenceAggregateName,
        string externalId,
        string referenceAggregateId)
        : base(partitionId)
    {
        SystemId = systemId;
        ReferenceAggregateName = referenceAggregateName;
        ReferenceAggregateId = referenceAggregateId;
        ExternalId = externalId;
        CompanyId = companyId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalSystemEvent" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected ExternalSystemEvent() => CompanyId = SystemId = ReferenceAggregateId = ReferenceAggregateName = ExternalId = string.Empty;

    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the external identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    public string ExternalId { get; set; }

    /// <summary>
    /// Gets or sets the reference aggregate identifier.
    /// </summary>
    /// <value>The reference aggregate identifier.</value>
    [DataMember(Order = 6)]
    [JsonPropertyOrder(6)]
    public string ReferenceAggregateId { get; set; }

    /// <summary>
    /// Gets or sets the name of the reference aggregate.
    /// </summary>
    /// <value>The name of the reference aggregate.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public string ReferenceAggregateName { get; set; }

    /// <summary>
    /// Gets or sets the system identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string SystemId { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => ExternalSystemReference.GetAggregateId(PartitionId, CompanyId, SystemId, ReferenceAggregateName, ExternalId);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => ExternalSystemReference.GetAggregateName();
}