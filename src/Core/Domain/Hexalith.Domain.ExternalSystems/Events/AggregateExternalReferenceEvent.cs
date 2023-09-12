// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-04-2023
// ***********************************************************************
// <copyright file="AggregateExternalReferenceEvent.cs" company="Fiveforty SAS Paris France">
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
/// Class AggregateExternalReferenceEvent.
/// Implements the <see cref="Hexalith.Domain.Events.BaseEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.BaseEvent" />
[DataContract]
public abstract class AggregateExternalReferenceEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateExternalReferenceEvent"/> class.
    /// </summary>
    /// <param name="referenceAggregateName">Name of the reference aggregate.</param>
    /// <param name="referenceAggregateId">The reference aggregate identifier.</param>
    [JsonConstructor]
    protected AggregateExternalReferenceEvent(string referenceAggregateName, string referenceAggregateId)
    {
        ReferenceAggregateName = referenceAggregateName;
        ReferenceAggregateId = referenceAggregateId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateExternalReferenceEvent" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    protected AggregateExternalReferenceEvent() => ReferenceAggregateId = ReferenceAggregateName = string.Empty;

    /// <summary>
    /// Gets or sets the reference aggregate identifier.
    /// </summary>
    /// <value>The reference aggregate identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string ReferenceAggregateId { get; set; }

    /// <summary>
    /// Gets the name of the reference aggregate.
    /// </summary>
    /// <value>The name of the reference aggregate.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string ReferenceAggregateName { get; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => AggregateExternalReference.GetAggregateId(ReferenceAggregateId);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => AggregateExternalReference.GetAggregateName();
}