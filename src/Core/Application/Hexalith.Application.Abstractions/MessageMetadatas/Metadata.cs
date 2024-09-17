// <copyright file="Metadata.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.MessageMetadatas;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Represents the metadata of a message.
/// </summary>
/// <param name="Message">The message metadata.</param>
/// <param name="Context">The context metadata.</param>
[DataContract]
public record Metadata(
    [property: DataMember(Order = 1)]
    [property: JsonPropertyOrder(1)]
    MessageMetadata Message,
    [property: DataMember(Order = 2)]
    [property: JsonPropertyOrder(2)]
    ContextMetadata Context)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public Metadata()
        : this(new MessageMetadata(), new ContextMetadata())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata"/> class.
    /// </summary>
    /// <param name="message">The message metadata.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="dateTime">The created date.</param>
    /// <returns>A new instance of the <see cref="Metadata"/> class.</returns>
    public static Metadata CreateNew(object message, Metadata metadata, DateTimeOffset dateTime)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        return new Metadata(new MessageMetadata(message, dateTime), metadata.Context);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Metadata"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="dateTime">The created date.</param>
    /// <returns>A new instance of the <see cref="Metadata"/> class.</returns>
    public static Metadata CreateNew(object message, string userId, DateTimeOffset dateTime)
    {
        ArgumentNullException.ThrowIfNull(message);
        MessageMetadata msgMeta = new(message, dateTime);
        return new Metadata(
            msgMeta,
            new ContextMetadata(msgMeta.Id, userId, dateTime, null, null, []));
    }
}