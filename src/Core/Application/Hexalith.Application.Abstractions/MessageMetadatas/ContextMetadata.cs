// <copyright file="ContextMetadata.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.MessageMetadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// The context metadata.
/// </summary>
/// <param name="CorrelationId">The correlation identifier.</param>
/// <param name="UserId">The user identifier.</param>
/// <param name="PartitionId">The partition identifier.</param>
/// <param name="ReceivedDate">The received date.</param>
/// <param name="SequenceNumber">The sequence number.</param>
/// <param name="SessionId">The session identifier.</param>
/// <param name="Scopes">The scopes.</param>
[DataContract]
public record ContextMetadata(
    [property:DataMember(Order = 1)]
    [property:JsonPropertyOrder(1)]
    string CorrelationId,
    [property:DataMember(Order = 2)]
    [property:JsonPropertyOrder(2)]
    string UserId,
    [property:DataMember(Order = 2)]
    [property:JsonPropertyOrder(2)]
    string PartitionId,
    [property:DataMember(Order = 3)]
    [property:JsonPropertyOrder(3)]
    DateTimeOffset? ReceivedDate,
    [property:DataMember(Order = 4)]
    [property:JsonPropertyOrder(4)]
    long? SequenceNumber,
    [property:DataMember(Order = 5)]
    [property:JsonPropertyOrder(5)]
    string? SessionId,
    [property: DataMember(Order = 6)]
    [property: JsonPropertyOrder(6)]
    IEnumerable<string> Scopes)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContextMetadata"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public ContextMetadata()
        : this(string.Empty, string.Empty, string.Empty, null, null, null, [])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContextMetadata"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="receivedDate">The received date.</param>
    /// <exception cref="ArgumentNullException">context is null.</exception>
    public ContextMetadata(ContextMetadata context, DateTimeOffset receivedDate)
        : this(
              (context ?? throw new ArgumentNullException(nameof(context))).CorrelationId,
              context.UserId,
              context.PartitionId,
              receivedDate,
              context.SequenceNumber,
              context.SessionId,
              context.Scopes)
    {
    }
}