﻿// <copyright file="ActorMessageEnvelope.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

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
/// <param name="messageJson">The message in JSON.</param>
/// <param name="metadataJson">The metadata in JSON.</param>
/// <param name="isRecord">Indicates whether the message is a record or a class.</param>
[DataContract]
[method: JsonConstructor]
public class ActorMessageEnvelope(string messageJson, string metadataJson, bool isRecord)
{
    /// <summary>
    /// Gets or sets a value indicating whether the message is a record or a class.
    /// </summary>
    /// <value>Is a record or not.</value>
    [DataMember(Name = nameof(IsRecord))]
    [JsonPropertyName(nameof(IsRecord))]
    public bool IsRecord { get; set; } = isRecord;

    /// <summary>
    /// Gets or sets the messages json.
    /// </summary>
    /// <value>The messages json.</value>
    [DataMember(Name = nameof(Message))]
    [JsonPropertyName(nameof(Message))]
    public string Message { get; set; } = messageJson;

    /// <summary>
    /// Gets or sets the metadatas json.
    /// </summary>
    /// <value>The metadatas json.</value>
    [DataMember(Name = nameof(Metadata))]
    [JsonPropertyName(nameof(Metadata))]
    public string Metadata { get; set; } = metadataJson;

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
        return metadata.Message.Aggregate.Name != messageAggregate.Name || metadata.Message.Aggregate.Id != messageAggregate.Id
            ? throw new ArgumentException($"Metadata and message aggregate details don't match. Metadata: {metadata.Message.Aggregate.Name}/{metadata.Message.Aggregate.Id}, Message: {messageAggregate.Name}/{messageAggregate.Id}", nameof(metadata))
            : new ActorMessageEnvelope(
            JsonSerializer.Serialize(message, serializerOptions),
            JsonSerializer.Serialize(metadata, serializerOptions),
            isRecord);
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