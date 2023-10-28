// <copyright file="CompanyEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class CompanyEvent.
/// Implements the <see cref="Hexalith.Domain.Events.PartitionedEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.PartitionedEvent" />
[DataContract]
public abstract class CompanyEvent : PartitionedEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyEvent" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    [JsonConstructor]
    protected CompanyEvent(string partitionId, string companyId)
        : base(partitionId)
            => CompanyId = companyId;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyEvent"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected CompanyEvent() => CompanyId = string.Empty;

    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string CompanyId { get; set; }
}