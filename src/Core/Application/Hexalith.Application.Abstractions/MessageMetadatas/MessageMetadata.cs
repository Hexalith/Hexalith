// <copyright file="MessageMetadata.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.MessageMetadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents the metadata of a message.
/// </summary>
/// <param name="Id">The message identifier.</param>
/// <param name="Name">The message name.</param>
/// <param name="Version">The message version.</param>
/// <param name="Aggregate">The aggregate metadata.</param>
/// <param name="CreatedDate">The message creation date.</param>
[DataContract]
public record MessageMetadata(
    [property : DataMember(Order = 1)]
    [property : JsonPropertyOrder(1)]
    string Id,
    [property : DataMember(Order = 2)]
    [property : JsonPropertyOrder(2)]
    string Name,
    [property : DataMember(Order = 3)]
    [property : JsonPropertyOrder(3)]
    int Version,
    [property:DataMember(Order = 4)]
    [property : JsonPropertyOrder(4)]
    AggregateMetadata Aggregate,
    [property : DataMember(Order = 5)]
    [property : JsonPropertyOrder(5)]
    DateTimeOffset CreatedDate)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public MessageMetadata()
        : this(string.Empty, string.Empty, 0, new AggregateMetadata(), DateTimeOffset.MinValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="dateTimeOffset">The creation date of the message.</param>
    public MessageMetadata(object message, DateTimeOffset dateTimeOffset)
        : this(UniqueIdHelper.GenerateUniqueStringId(), string.Empty, 1, new AggregateMetadata(message), dateTimeOffset)
    {
        (string name, string _, int version) = message.GetPolymorphicTypeDiscriminator();
        Name = name;
        Version = version;
    }
}