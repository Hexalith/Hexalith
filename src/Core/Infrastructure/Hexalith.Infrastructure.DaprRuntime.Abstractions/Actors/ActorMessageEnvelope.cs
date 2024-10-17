// <copyright file="ActorMessageEnvelope.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

using Hexalith.Application;
using Hexalith.Application.MessageMetadatas;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Actor method object uses Data Contract Serialization that can't serialize and deserialize complex polymorphic objects.
/// This class is used to serialize and deserialize messages and metadatas into JSON strings before the actor proxy serialization.
/// Implements the <see cref="IEquatable{ActorMessageEnvelope}" />.
/// </summary>
/// <seealso cref="IEquatable{ActorMessageEnvelope}" />
/// <remarks>
/// Initializes a new instance of the <see cref="ActorMessageEnvelope"/> class.
/// </remarks>
/// <param name="Message">The message in JSON format.</param>
/// <param name="Metadata">The metadata in JSON format.</param>
/// <param name="IsRecord">Indicates if the message is a record.</param>
[DataContract]
public record ActorMessageEnvelope(
    [property: DataMember(Order = 1)]
    string Message,
    [property: DataMember(Order = 2)]
    string Metadata,
    [property: DataMember(Order = 3)]
    bool IsRecord)
{
    /// <summary>
    /// Creates a new instance of the <see cref="ActorMessageEnvelope"/> class by encoding the message and metadata fields to Base64.
    /// </summary>
    /// <param name="message">The message object to be serialized and encoded.</param>
    /// <param name="metadata">The metadata object to be serialized and encoded.</param>
    /// <returns>A new instance of the <see cref="ActorMessageEnvelope"/> class.</returns>
    /// <exception cref="ArgumentException">Thrown when the message is not derived from PolymorphicRecordBase or PolymorphicClassBase.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the message or metadata is null.</exception>
    public static ActorMessageEnvelope Create([NotNull] object message, [NotNull] Metadata metadata)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        bool isRecord = message is PolymorphicRecordBase;

        if (!isRecord && message is not PolymorphicClassBase)
        {
            throw new ArgumentException(
                $"Only objects derived from {nameof(PolymorphicRecordBase)} or {nameof(PolymorphicClassBase)} can be serialized by the {nameof(ActorMessageEnvelope)}.",
                nameof(message));
        }

        AggregateMetadata messageAggregate = new(message);
        if (metadata.Message.Aggregate.Name != messageAggregate.Name || metadata.Message.Aggregate.Id != messageAggregate.Id)
        {
            throw new ArgumentException($"Metadata and message aggregate details don't match. Metadata: {metadata.Message.Aggregate.Name}/{metadata.Message.Aggregate.Id}, Message: {messageAggregate.Name}/{messageAggregate.Id}", nameof(metadata));
        }

        // Serialize the message and metadata to JSON
        string messageJson = isRecord
            ? JsonSerializer.Serialize((PolymorphicRecordBase)message, PolymorphicHelper.DefaultJsonSerializerOptions)
            : JsonSerializer.Serialize((PolymorphicClassBase)message, PolymorphicHelper.DefaultJsonSerializerOptions);
        string metadataJson = JsonSerializer.Serialize(metadata, PolymorphicHelper.DefaultJsonSerializerOptions);

        // Encode the JSON strings to Base64
        string encodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(messageJson));
        string encodedMetadata = Convert.ToBase64String(Encoding.UTF8.GetBytes(metadataJson));

        return new ActorMessageEnvelope(encodedMessage, encodedMetadata, isRecord);
    }

    /// <summary>
    /// Deserializes the Base64 encoded message and metadata fields.
    /// </summary>
    /// <returns>A tuple containing the deserialized message object and metadata.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the message or metadata cannot be deserialized.</exception>
    public (object Message, Metadata Metadata) Deserialize()
    {
        // Decode the Base64 encoded metadata and deserialize it
        string decodedMetadataJson = Encoding.UTF8.GetString(Convert.FromBase64String(Metadata));
        Metadata? metadata = JsonSerializer.Deserialize<Metadata>(decodedMetadataJson, PolymorphicHelper.DefaultJsonSerializerOptions)
            ?? throw new InvalidOperationException("The message metadata could not be deserialized. JSON : " + Metadata);

        // Decode the Base64 encoded message and deserialize it
        string decodedMessageJson = Encoding.UTF8.GetString(Convert.FromBase64String(Message));
        object? message = (IsRecord
            ? JsonSerializer.Deserialize<PolymorphicRecordBase>(decodedMessageJson, PolymorphicHelper.DefaultJsonSerializerOptions) as object
            : JsonSerializer.Deserialize<PolymorphicClassBase>(decodedMessageJson, PolymorphicHelper.DefaultJsonSerializerOptions))
            ?? throw new InvalidOperationException("The message could not be deserialized. JSON : " + Message + " Metadata : " + Metadata);

        return (message, metadata);
    }
}