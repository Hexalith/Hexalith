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

namespace Hexalith.Application.ExternalSystems.Commands;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class AggregateExternalReferenceAdded.
/// Implements the <see cref="AggregateExternalReferenceCommand" />.
/// </summary>
/// <seealso cref="AggregateExternalReferenceCommand" />
[DataContract]
public class AddAggregateExternalReference : AggregateExternalReferenceCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddAggregateExternalReference" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    [JsonConstructor]
    public AddAggregateExternalReference(
        string id,
        string systemId,
        string externalId)
        : base(id)
    {
        SystemId = systemId;
        ExternalId = externalId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddAggregateExternalReference" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public AddAggregateExternalReference() => SystemId = ExternalId = string.Empty;

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