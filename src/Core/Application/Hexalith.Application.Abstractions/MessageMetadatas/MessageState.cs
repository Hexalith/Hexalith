// <copyright file="MessageState.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.MessageMetadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions.Common;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents the state of a message.
/// </summary>
/// <param name="RecordObject">The record object.</param>
/// <param name="ClassObject">The class object.</param>
/// <param name="Metadata">The metadata.</param>
[DataContract]
[method: JsonConstructor]
public record MessageState(
    [property:JsonPropertyOrder(1)]
    [property: DataMember(Order = 1)]
    PolymorphicRecordBase? RecordObject,
    [property:JsonPropertyOrder(2)]
    [property: DataMember(Order = 2)]
    PolymorphicClassBase? ClassObject,
    [property: DataMember(Order = 3)]
    [property: JsonPropertyOrder(3)]
    Metadata Metadata) : IIdempotent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageState"/> class.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="metadata">The metadata.</param>
    public MessageState(object data, Metadata metadata)
        : this(data as PolymorphicRecordBase, data as PolymorphicClassBase, metadata)
    {
    }

    /// <inheritdoc/>
    [JsonIgnore]
    [IgnoreDataMember]
    public string IdempotencyId => Metadata.Message.Id;

    /// <summary>
    /// Gets the message.
    /// </summary>
    [JsonIgnore]
    [IgnoreDataMember]
    public object Message => (object?)RecordObject ?? ClassObject ?? throw new InvalidOperationException("The polymorphic class and record objects are null.");
}