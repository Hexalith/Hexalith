﻿// <copyright file="ActorMessageEnvelope.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

using Hexalith.Application.Metadatas;
using Hexalith.PolymorphicSerializations;

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
[DataContract]
public record ActorMessageEnvelope(
    [property: DataMember(Order = 1)]
    string Message,
    [property: DataMember(Order = 2)]
    string Metadata)
{
    /// <summary>
    /// Creates a new instance of the <see cref="ActorMessageEnvelope"/> class by encoding the message and metadata fields to Base64.
    /// </summary>
    /// <param name="message">The message object to be serialized and encoded.</param>
    /// <param name="metadata">The metadata object to be serialized and encoded.</param>
    /// <returns>A new instance of the <see cref="ActorMessageEnvelope"/> class.</returns>
    /// <exception cref="ArgumentException">Thrown when the message is not derived from Polymorphic or PolymorphicClassBase.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the message or metadata is null.</exception>
    public static ActorMessageEnvelope Create([NotNull] object message, [NotNull] Metadata metadata)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);

        if (message is not Polymorphic)
        {
            throw new ArgumentException(
                $"Only objects derived from {nameof(Polymorphic)} can be serialized by the {nameof(ActorMessageEnvelope)}.",
                nameof(message));
        }

        // Serialize the message and metadata to JSON
        string messageJson = JsonSerializer.Serialize((Polymorphic)message, PolymorphicHelper.DefaultJsonSerializerOptions);
        string metadataJson = JsonSerializer.Serialize(metadata, PolymorphicHelper.DefaultJsonSerializerOptions);

        // Encode the JSON strings to Base64
        string encodedMessage = Convert.ToBase64String(Encoding.UTF8.GetBytes(messageJson));
        string encodedMetadata = Convert.ToBase64String(Encoding.UTF8.GetBytes(metadataJson));

        return new ActorMessageEnvelope(encodedMessage, encodedMetadata);
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
        Metadata metadata = JsonSerializer.Deserialize<Metadata>(decodedMetadataJson, PolymorphicHelper.DefaultJsonSerializerOptions)
            ?? throw new InvalidOperationException("The message metadata could not be deserialized. JSON : " + decodedMetadataJson);

        // Decode the Base64 encoded message and deserialize it
        string decodedMessageJson = Encoding.UTF8.GetString(Convert.FromBase64String(Message));
        Polymorphic message = JsonSerializer.Deserialize<Polymorphic>(decodedMessageJson, PolymorphicHelper.DefaultJsonSerializerOptions)
            ?? throw new InvalidOperationException("The message could not be deserialized. JSON : " + decodedMessageJson + " Metadata : " + decodedMetadataJson);

        return (message, metadata);
    }
}