// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-16-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="CompanyEntityEvent.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
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
/// Class CompanyEntityEvent.
/// Implements the <see cref="CompanyEntityEvent" />.
/// </summary>
/// <seealso cref="CompanyEntityEvent" />
[DataContract]
public class CompanyEntityEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyEntityEvent"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected CompanyEntityEvent(string partitionId, string companyId, string originId, string id)
    {
        PartitionId = partitionId;
        OriginId = originId;
        Id = id;
        CompanyId = companyId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyEntityEvent"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected CompanyEntityEvent() => PartitionId = CompanyId = OriginId = Id = string.Empty;

    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public string CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string OriginId { get; set; }

    /// <summary>
    /// Gets or sets the partition identifier.
    /// </summary>
    /// <value>The partition identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string PartitionId { get; set; }
}