// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
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
    /// Initializes a new instance of the <see cref="AggregateExternalReferenceEvent" /> class.
    /// </summary>
    /// <param name="name">The aggregate type name.</param>
    /// <param name="id">The aggregate identifier.</param>
    [JsonConstructor]
    protected AggregateExternalReferenceEvent(string id) => Id = id;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateExternalReferenceEvent" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    protected AggregateExternalReferenceEvent() => Id = string.Empty;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string Id { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => base.DefaultAggregateId() + Separator + Id;

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => nameof(AggregateExternalReference);
}