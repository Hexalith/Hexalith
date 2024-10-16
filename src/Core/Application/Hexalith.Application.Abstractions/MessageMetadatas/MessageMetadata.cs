// <copyright file="MessageMetadata.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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
[DataContract]
public record MessageMetadata(
    [property: DataMember(Order = 1)]
    [property: JsonPropertyOrder(1)]
    string Id,
    [property: DataMember(Order = 2)]
    [property: JsonPropertyOrder(2)]
    string Name,
    [property: DataMember(Order = 3)]
    [property: JsonPropertyOrder(3)]
    int Version,
    [property: DataMember(Order = 4)]
    [property: JsonPropertyOrder(4)]
    AggregateMetadata Aggregate,
    [property: DataMember(Order = 5)]
    [property: JsonPropertyOrder(5)]
    DateTimeOffset CreatedDate)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// This constructor is for serialization purposes only and should not be used directly.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public MessageMetadata()
        : this(string.Empty, string.Empty, 0, new AggregateMetadata(), DateTimeOffset.MinValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// </summary>
    /// <param name="message">The message object.</param>
    /// <param name="dateTimeOffset">The creation date of the message.</param>
    public MessageMetadata(object message, DateTimeOffset dateTimeOffset)
        : this(UniqueIdHelper.GenerateUniqueStringId(), string.Empty, 1, new AggregateMetadata(message), dateTimeOffset)
    {
        (string name, string _, int version) = message.GetPolymorphicTypeDiscriminator();
        Name = name;
        Version = version;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="MessageMetadata"/> class for a specific command type.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <param name="aggregateName">The name of the aggregate.</param>
    /// <param name="aggregateId">The ID of the aggregate.</param>
    /// <param name="dateTimeOffset">The creation date of the message.</param>
    /// <returns>A new instance of <see cref="MessageMetadata"/>.</returns>
    public static MessageMetadata Create<TCommand>(string aggregateName, string aggregateId, DateTimeOffset dateTimeOffset)
    {
        (string name, string _, int version) = typeof(TCommand).GetPolymorphicTypeDiscriminator();
        return new MessageMetadata(
            UniqueIdHelper.GenerateUniqueStringId(),
            name,
            version,
            new AggregateMetadata(aggregateName, aggregateId),
            dateTimeOffset);
    }
}