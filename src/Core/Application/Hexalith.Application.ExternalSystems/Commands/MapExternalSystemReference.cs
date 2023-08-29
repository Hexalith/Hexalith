// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="MapExternalSystemReference.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.ExternalSystems.Commands;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class ExternalSystemReferenceMapped.
/// Implements the <see cref="ExternalSystemReferenceCommand" />.
/// </summary>
/// <seealso cref="ExternalSystemReferenceCommand" />
[DataContract]
public class MapExternalSystemReference : ExternalSystemReferenceCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MapExternalSystemReference" /> class.
    /// </summary>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <param name="referenceAggregateName">Type of the aggregate.</param>
    /// <param name="referenceAggregateId">The identifier.</param>
    [JsonConstructor]
    public MapExternalSystemReference(
        string systemId,
        string externalId,
        string referenceAggregateName,
        string referenceAggregateId)
        : base(systemId, externalId, referenceAggregateName) => ReferenceAggregateId = referenceAggregateId;

    /// <summary>
    /// Initializes a new instance of the <see cref="MapExternalSystemReference" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public MapExternalSystemReference() => ReferenceAggregateId = string.Empty;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public string ReferenceAggregateId { get; set; }
}