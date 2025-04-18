// <copyright file="BusMessage.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Buses;

using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents the state of a message, including its content and metadata.
/// </summary>
/// <remarks>
/// This record is used to encapsulate both the message content and its associated metadata
/// for transmission through a message bus.
/// </remarks>
/// <param name="Message">The JSON-serialized string representing the message content.</param>
/// <param name="Metadata">The metadata associated with the message.</param>
[DataContract]
public record BusMessage(
    [property: JsonPropertyOrder(1)]
    [property: DataMember(Order = 1)]
    string Message,
    [property: DataMember(Order = 2)]
    [property: JsonPropertyOrder(2)]
    Metadata Metadata)
{
    /// <summary>
    /// Creates a new instance of the <see cref="BusMessage"/> class.
    /// </summary>
    /// <param name="message">The object to be serialized as the message content.</param>
    /// <param name="metadata">The metadata associated with the message.</param>
    /// <returns>A new <see cref="BusMessage"/> instance with the serialized message and provided metadata.</returns>
    /// <remarks>
    /// This method serializes the provided message object to JSON using the default polymorphic serializer options.
    /// </remarks>
    public static BusMessage Create(object message, Metadata metadata)
    {
        ArgumentNullException.ThrowIfNull(message);
        return message is Polymorphic polymorphicObject
            ? new(
            JsonSerializer
            .Serialize(polymorphicObject, PolymorphicHelper.DefaultJsonSerializerOptions),
            metadata)
            : throw new ArgumentException(
                $"Only objects derived from {nameof(Polymorphic)} can be serialized by the {nameof(BusMessage)}.",
                nameof(message));
    }
}