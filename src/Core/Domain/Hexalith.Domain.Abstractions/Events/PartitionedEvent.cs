// <copyright file="PartitionnedEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public abstract class PartitionedEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionedEvent"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    [JsonConstructor]
    protected PartitionedEvent(string partitionId) => PartitionId = partitionId;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionedEvent"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    protected PartitionedEvent() => PartitionId = string.Empty;

    /// <summary>
    /// Gets the partition identifier.
    /// </summary>
    /// <value>The partition identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string PartitionId { get; }
}