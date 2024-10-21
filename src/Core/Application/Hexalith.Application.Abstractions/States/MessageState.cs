// <copyright file="MessageState.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.States;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Common;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents the state of a message, including its content and metadata.
/// </summary>
/// <param name="Message">The record object representing the message content.</param>
/// <param name="Metadata">The metadata associated with the message.</param>
[DataContract]
public record MessageState(
    [property:JsonPropertyOrder(1)]
    [property: DataMember(Order = 1)]
    PolymorphicRecordBase Message,
    [property: DataMember(Order = 2)]
    [property: JsonPropertyOrder(2)]
    Metadata Metadata) : IIdempotent
{
    /// <inheritdoc/>
    [JsonIgnore]
    [IgnoreDataMember]
    public string IdempotencyId => Metadata.PartitionKey;
}