// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="AggregateExternalReferenceCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.ExternalSystems.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Organizations.Commands;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;

/// <summary>
/// Class AggregateExternalReferenceEvent.
/// Implements the <see cref="BaseEvent" />.
/// </summary>
/// <seealso cref="BaseEvent" />
[DataContract]
public abstract class AggregateExternalReferenceCommand : PartitionedCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateExternalReferenceCommand" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="referenceAggregateName">Name of the reference aggregate.</param>
    /// <param name="referenceAggregateId">The reference aggregate identifier.</param>
    [JsonConstructor]
    protected AggregateExternalReferenceCommand(
        string partitionId,
        string referenceAggregateName,
        string referenceAggregateId)
        : base(partitionId)
    {
        ReferenceAggregateName = referenceAggregateName;
        ReferenceAggregateId = referenceAggregateId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateExternalReferenceCommand" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    protected AggregateExternalReferenceCommand() => ReferenceAggregateName = ReferenceAggregateId = string.Empty;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string ReferenceAggregateId { get; set; }

    /// <summary>
    /// Gets the name of the reference aggregate.
    /// </summary>
    /// <value>The name of the reference aggregate.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string ReferenceAggregateName { get; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => AggregateExternalReference.GetAggregateId(PartitionId, ReferenceAggregateId);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => AggregateExternalReference.GetAggregateName();
}