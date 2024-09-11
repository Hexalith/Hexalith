// <copyright file="ActorMessageEnvelope.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Application.MessageMetadatas;

/// <summary>
/// Actor method object uses Data Contract Serialization that can't serialize and deserialize complex polymorphic objects. This class is used to serialize and deserialize messages and metadatas into JSON strings before the actor proxy serialization.
/// Implements the <see cref="IEquatable{ActorMessageEnvelope}" />.
/// </summary>
/// <seealso cref="IEquatable{ActorMessageEnvelope}" />
[DataContract]
public class ActorMessageEnvelope : IJsonOnSerializing, IJsonOnDeserialized
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActorMessageEnvelope"/> class.
    /// </summary>
    /// <param name="messageJson">The message in JSON.</param>
    /// <param name="metadataJson">The metadata in JSON.</param>
    [Obsolete("Only used for serialization", true)]
    [JsonConstructor]
    public ActorMessageEnvelope(string messageJson, string metadataJson)
    {
        MessageJson = messageJson;
        MetadataJson = metadataJson;
        Message = string.Empty;
        Metadata = new();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorMessageEnvelope" /> class.
    /// </summary>
    [Obsolete("Only used for serialization", true)]
    public ActorMessageEnvelope()
    {
        MessageJson = string.Empty;
        MetadataJson = string.Empty;
        Message = string.Empty;
        Metadata = new();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActorMessageEnvelope" /> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    public ActorMessageEnvelope([NotNull] object message, [NotNull] Metadata metadata)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        MessageJson = string.Empty;
        MetadataJson = string.Empty;
        Message = message;
        Metadata = metadata;
        AggregateMetadata messageAggregate = new(message);
        if (metadata.Message.Aggregate.Name != messageAggregate.Name || metadata.Message.Aggregate.Id != messageAggregate.Id)
        {
            throw new ArgumentException($"Metadata and message aggregate details don't match. Metadata: {metadata.Message.Aggregate.Name}/{metadata.Message.Aggregate.Id}, Message: {messageAggregate.Name}/{messageAggregate.Id}", nameof(metadata));
        }
    }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>The message.</value>
    [JsonIgnore]
    [IgnoreDataMember]
    public object Message { get; set; }

    /// <summary>
    /// Gets or sets the messages json.
    /// </summary>
    /// <value>The messages json.</value>
    [DataMember(Name = nameof(Message))]
    [JsonPropertyName(nameof(Message))]
    public string MessageJson { get; set; }

    /// <summary>
    /// Gets or sets the metadata.
    /// </summary>
    /// <value>The metadata.</value>
    [JsonIgnore]
    [IgnoreDataMember]
    public Metadata Metadata { get; set; }

    /// <summary>
    /// Gets or sets the metadatas json.
    /// </summary>
    /// <value>The metadatas json.</value>
    [DataMember(Name = nameof(Metadata))]
    [JsonPropertyName(nameof(Metadata))]
    public string MetadataJson { get; set; }

    /// <summary>
    /// The method that is called after deserialization.
    /// </summary>
    public void OnDeserialized()
    {
        Metadata = JsonSerializer.Deserialize<Metadata>(MetadataJson) ?? throw new InvalidOperationException($"Metadata property contains invalid JSON: {MetadataJson}");
        Message = JsonSerializer.Deserialize<object>(MessageJson) ?? throw new InvalidOperationException($"Message property contains invalid JSON: {MessageJson}");
    }

    /// <summary>
    /// The method that is called before serialization.
    /// </summary>
    public void OnSerializing()
    {
        MessageJson = JsonSerializer.Serialize(Message);
        MetadataJson = JsonSerializer.Serialize(Metadata);
    }

    /// <summary>
    /// Called when [deserialized].
    /// </summary>
    /// <param name="context">The context.</param>
    [OnDeserialized]
    private void OnDeserialized(StreamingContext context) => OnDeserialized();

    /// <summary>
    /// Called when [serializing].
    /// </summary>
    /// <param name="context">The context.</param>
    [OnSerializing]
    private void OnSerializing(StreamingContext context) => OnSerializing();
}