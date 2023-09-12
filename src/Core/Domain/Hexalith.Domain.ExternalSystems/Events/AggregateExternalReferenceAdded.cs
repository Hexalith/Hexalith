// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="AggregateExternalReferenceAdded.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class AggregateExternalReferenceAdded.
/// Implements the <see cref="Hexalith.Domain.Events.AggregateExternalReferenceEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.AggregateExternalReferenceEvent" />
[DataContract]
public class AggregateExternalReferenceAdded : AggregateExternalReferenceEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateExternalReferenceAdded"/> class.
    /// </summary>
    /// <param name="referenceAggregateName">Name of the reference aggregate.</param>
    /// <param name="referenceAggregateId">The reference aggregate identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    [JsonConstructor]
    public AggregateExternalReferenceAdded(
        string referenceAggregateName,
        string referenceAggregateId,
        string systemId,
        string externalId)
        : base(referenceAggregateName, referenceAggregateId)
    {
        SystemId = systemId;
        ExternalId = externalId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateExternalReferenceAdded" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public AggregateExternalReferenceAdded() => SystemId = ExternalId = string.Empty;

    /// <summary>
    /// Gets the external identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 11)]
    [JsonPropertyOrder(11)]
    public string ExternalId { get; private set; }

    /// <summary>
    /// Gets the system identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public string SystemId { get; private set; }
}