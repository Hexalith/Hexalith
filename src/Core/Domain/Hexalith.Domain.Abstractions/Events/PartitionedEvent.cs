// ***********************************************************************
// Assembly         : Hexalith.Domain.Organizations
// Author           : Jérôme Piquot
// Created          : 10-16-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="PartitionedEvent.cs" company="Jérôme Piquot">
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
/// Class PartitionedEvent.
/// Implements the <see cref="BaseEvent" />.
/// </summary>
/// <seealso cref="BaseEvent" />
[DataContract]
public class PartitionedEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionedEvent" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    [JsonConstructor]
    protected PartitionedEvent(string partitionId) => PartitionId = partitionId;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionedEvent" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected PartitionedEvent() => PartitionId = string.Empty;

    /// <summary>
    /// Gets or sets the partition identifier.
    /// </summary>
    /// <value>The partition identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string PartitionId { get; set; }
}