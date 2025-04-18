// <copyright file="MessageState.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.States;

using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Common;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents the state of a message with its associated metadata.
/// </summary>
[DataContract]
public class MessageState : IIdempotent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageState"/> class.
    /// </summary>
    /// <param name="message">The message content.</param>
    /// <param name="metadata">The metadata associated with the message.</param>
    public MessageState(string message, Metadata metadata)
    {
        Message = message;
        Metadata = metadata;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageState"/> class.
    /// </summary>
    /// <param name="message">The polymorphic message content.</param>
    /// <param name="metadata">The metadata associated with the message.</param>
    public MessageState(Polymorphic message, Metadata metadata)
    {
        Message = Serialize(message);
        Metadata = metadata;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageState"/> class.
    /// </summary>
    public MessageState()
    {
        Message = string.Empty;
        Metadata = null!;
    }

    /// <inheritdoc/>
    [JsonIgnore]
    [IgnoreDataMember]
    public string IdempotencyId => Metadata.Message.Id;

    /// <summary>
    /// Gets or sets the message content.
    /// </summary>
    [JsonPropertyOrder(1)]
    [DataMember(Order = 1)]
    public string Message { get; set; }

    /// <summary>
    /// Gets the polymorphic message content as an object.
    /// </summary>
    [JsonIgnore]
    [IgnoreDataMember]
    public Polymorphic MessageObject => Deserialize();

    /// <summary>
    /// Gets or sets the metadata associated with the message.
    /// </summary>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public Metadata Metadata { get; set; }

    /// <summary>
    /// Serializes the given <see cref="Polymorphic"/> object to a JSON string.
    /// </summary>
    /// <param name="message">The message object to serialize.</param>
    /// <returns>The serialized JSON string.</returns>
    private static string Serialize(Polymorphic message) =>
        JsonSerializer.Serialize<Polymorphic>(message, PolymorphicHelper.DefaultJsonSerializerOptions);

    /// <summary>
    /// Deserializes the message content to a <see cref="Polymorphic"/> object.
    /// </summary>
    /// <returns>The deserialized <see cref="Polymorphic"/> object.</returns>
    /// <exception cref="InvalidOperationException">Thrown when deserialization fails.</exception>
    private Polymorphic Deserialize() =>
        JsonSerializer.Deserialize<Polymorphic>(Message, PolymorphicHelper.DefaultJsonSerializerOptions)
         ?? throw new InvalidOperationException("Message deserialization failed :" + Message);
}