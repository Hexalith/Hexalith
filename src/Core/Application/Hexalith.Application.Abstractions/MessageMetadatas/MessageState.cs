// <copyright file="MessageState.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.MessageMetadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions.Common;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents the state of a message, including its content and metadata.
/// </summary>
/// <param name="RecordObject">The record object representing the message content.</param>
/// <param name="ClassObject">The class object representing the message content.</param>
/// <param name="Metadata">The metadata associated with the message.</param>
[DataContract]
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
    /// <inheritdoc/>
    [JsonIgnore]
    [IgnoreDataMember]
    public string IdempotencyId => Metadata.Message.Id;

    /// <summary>
    /// Gets the message content.
    /// </summary>
    /// <remarks>
    /// This property returns the RecordObject if it's not null, otherwise it returns the ClassObject.
    /// If both are null, it throws an InvalidOperationException.
    /// </remarks>
    /// <exception cref="InvalidOperationException">Thrown when both RecordObject and ClassObject are null.</exception>
    [JsonIgnore]
    [IgnoreDataMember]
    public object Message => (object?)RecordObject ?? ClassObject ?? throw new InvalidOperationException("The polymorphic class and record objects are null.");

    /// <summary>
    /// Creates a new instance of the MessageState class.
    /// </summary>
    /// <param name="data">The message data, which can be either a PolymorphicRecordBase or a PolymorphicClassBase.</param>
    /// <param name="metadata">The metadata associated with the message.</param>
    /// <returns>A new MessageState instance.</returns>
    public static MessageState Create(object data, Metadata metadata)
        => new(data as PolymorphicRecordBase, data as PolymorphicClassBase, metadata);
}
