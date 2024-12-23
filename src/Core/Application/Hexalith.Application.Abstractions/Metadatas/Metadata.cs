// <copyright file="Metadata.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Metadatas;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the metadata of a message, including both message-specific and context-related information.
/// </summary>
/// <param name="Message">The message-specific metadata.</param>
/// <param name="Context">The context-related metadata.</param>
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
    /// Gets the partition key, which is a combination of the partition ID, aggregate name, and aggregate ID.
    /// </summary>
    /// <remarks>
    /// The partition key is used to determine how data is distributed across partitions in a distributed system.
    /// It is constructed by escaping the combination of PartitionId, Aggregate Name, and Aggregate Id.
    /// </remarks>
    public string AggregateGlobalId => CreateAggregateGlobalId(Context.PartitionId, Message.Aggregate.Name, Message.Aggregate.Id);

    /// <summary>
    /// Creates a global identifier for an aggregate by combining the partition ID, aggregate name, and aggregate ID.
    /// </summary>
    /// <param name="partitionId">The identifier of the partition.</param>
    /// <param name="aggregateName">The name of the aggregate.</param>
    /// <param name="aggregateId">The identifier of the aggregate.</param>
    /// <returns>A string representing the global identifier for the aggregate.</returns>
    public static string CreateAggregateGlobalId(string partitionId, string aggregateName, string aggregateId) => $"{partitionId}-{aggregateName}-{aggregateId}";

    /// <summary>
    /// Creates a new instance of the <see cref="Metadata"/> class with updated message information.
    /// </summary>
    /// <param name="message">The new message object to be included in the metadata.</param>
    /// <param name="metadata">The existing metadata to derive context from.</param>
    /// <param name="dateTime">The timestamp for the new message.</param>
    /// <returns>A new instance of the <see cref="Metadata"/> class with updated message information and existing context.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> or <paramref name="metadata"/> is null.</exception>
    public static Metadata CreateNew(object message, Metadata metadata, DateTimeOffset dateTime)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        return new Metadata(MessageMetadata.Create(message, dateTime), metadata.Context);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Metadata"/> class with new message and context information.
    /// </summary>
    /// <param name="message">The new message object to be included in the metadata.</param>
    /// <param name="userId">The identifier of the user associated with this message.</param>
    /// <param name="partitionId">The identifier of the partition this message belongs to.</param>
    /// <param name="dateTime">The timestamp for the new message.</param>
    /// <returns>A new instance of the <see cref="Metadata"/> class with new message and context information.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is null.</exception>
    public static Metadata CreateNew(object message, string userId, string partitionId, DateTimeOffset dateTime)
    {
        ArgumentNullException.ThrowIfNull(message);
        MessageMetadata msgMeta = MessageMetadata.Create(message, dateTime);
        return new Metadata(
            msgMeta,
            new ContextMetadata(msgMeta.Id, userId, partitionId, dateTime, null, null, []));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Metadata"/> class with new message, context, and session information.
    /// </summary>
    /// <param name="message">The new message object to be included in the metadata.</param>
    /// <param name="userId">The identifier of the user associated with this message.</param>
    /// <param name="partitionId">The identifier of the partition this message belongs to.</param>
    /// <param name="sessionId">The session identifier associated with this message.</param>
    /// <param name="dateTime">The timestamp for the new message.</param>
    /// <returns>A new instance of the <see cref="Metadata"/> class with new message, context, and session information.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="message"/> is null.</exception>
    public static Metadata CreateNew(object message, string userId, string partitionId, string sessionId, DateTimeOffset dateTime)
    {
        ArgumentNullException.ThrowIfNull(message);
        MessageMetadata msgMeta = MessageMetadata.Create(message, dateTime);
        return new Metadata(
            msgMeta,
            new ContextMetadata(msgMeta.Id, userId, partitionId, dateTime, null, sessionId, []));
    }

    /// <summary>
    /// Converts the metadata to a log string.
    /// </summary>
    /// <returns>A string representation of the metadata for logging purposes.</returns>
    public string ToLogString()
    {
        return $"MessageName={Message.Name}; AggregateGlobalId={AggregateGlobalId}; " +
            $"MessageId={Message.Id}; CorrelationId={Context.CorrelationId}; UserId={Context.UserId}";
    }
}