// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-02-2023
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

using Hexalith.Application.Commands;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;

/// <summary>
/// Class AggregateExternalReferenceEvent.
/// Implements the <see cref="BaseEvent" />.
/// </summary>
/// <seealso cref="BaseEvent" />
[DataContract]
public abstract class AggregateExternalReferenceCommand : BaseCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateExternalReferenceCommand" /> class.
    /// </summary>
    /// <param name="referenceAggregateId">The reference aggregate identifier.</param>
    [JsonConstructor]
    protected AggregateExternalReferenceCommand(string referenceAggregateId) => ReferenceAggregateId = referenceAggregateId;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateExternalReferenceCommand" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    protected AggregateExternalReferenceCommand() => ReferenceAggregateId = string.Empty;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string ReferenceAggregateId { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => AggregateExternalReference.GetAggregateId(ReferenceAggregateId);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => AggregateExternalReference.GetAggregateName();
}