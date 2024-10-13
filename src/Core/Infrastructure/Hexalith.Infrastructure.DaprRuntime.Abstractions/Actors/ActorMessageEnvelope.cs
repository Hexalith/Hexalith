// <copyright file="ActorMessageEnvelope.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Application.MessageMetadatas;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Actor method object uses Data Contract Serialization that can't serialize and deserialize complex polymorphic objects. This class is used to serialize and deserialize messages and metadatas into JSON strings before the actor proxy serialization.
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
    [property:DataMember(Order =1)]
    string Message,
    [property: DataMember(Order =2)]
    string Metadata,
    [property: DataMember(Order =3)]
    bool IsRecord)
{
    /// <summary>
    /// Creates a new instance of the <see cref="ActorMessageEnvelope"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="serializerOptions">The serializer options.</param>
    /// <returns>A new instance of the <see cref="ActorMessageEnvelope"/> class.</returns>
    public static ActorMessageEnvelope Create([NotNull] object message, [NotNull] Metadata metadata, JsonSerializerOptions serializerOptions)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        bool isRecord = false;
        if (message is PolymorphicRecordBase)
        {
            isRecord = true;
        }
        else
        if (message is not PolymorphicClassBase)
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

        string messageJson = isRecord
            ? JsonSerializer.Serialize((PolymorphicRecordBase)message, serializerOptions)
            : JsonSerializer.Serialize((PolymorphicClassBase)message, serializerOptions);
        string metadataJson = JsonSerializer.Serialize(metadata, serializerOptions);
        ActorMessageEnvelope envelope = new(messageJson, metadataJson, isRecord);
        return envelope;
    }

    /// <summary>
    /// Deserializes the message and metadata.
    /// </summary>
    /// <param name="serializerOptions">The serializer options with the polymorphic type resolver.</param>
    /// <returns>The message and metadata.</returns>
    public (object Message, Metadata Metadata) Deserialize(JsonSerializerOptions serializerOptions)
    {
        Metadata? metadata = JsonSerializer.Deserialize<Metadata>(Metadata, serializerOptions)
            ?? throw new InvalidOperationException("The message metadata could not be deserialized. JSON : " + Metadata);
        object? message = (IsRecord
            ? JsonSerializer.Deserialize<PolymorphicRecordBase>(Message, serializerOptions) as object
            : JsonSerializer.Deserialize<PolymorphicClassBase>(Message, serializerOptions))
            ?? throw new InvalidOperationException("The message could not be deserialized. JSON : " + Message + " Metadata : " + Metadata);

        return (message, metadata);
    }
}