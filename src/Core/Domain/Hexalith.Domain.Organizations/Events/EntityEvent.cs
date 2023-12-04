// ***********************************************************************
// Assembly         : Hexalith.Domain.Organizations
// Author           : Jérôme Piquot
// Created          : 11-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-21-2023
// ***********************************************************************
// <copyright file="EntityEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Organizations.Events;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class EntityEvent.
/// Implements the <see cref="PartitionedEvent" />.
/// </summary>
/// <seealso cref="PartitionedEvent" />
[DataContract]
[Serializable]
public abstract class EntityEvent : PartitionedEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityEvent" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected EntityEvent(string partitionId, string originId, string id)
        : base(partitionId)
    {
        OriginId = originId;
        Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityEvent" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected EntityEvent() => OriginId = Id = string.Empty;

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
}