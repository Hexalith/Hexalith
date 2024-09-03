// <copyright file="ContextMetadata.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// The context metadata.
/// </summary>
[DataContract]
public class ContextMetadata : IContextMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContextMetadata" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public ContextMetadata() => CorrelationId = UserId = string.Empty;

    /// <summary>Initializes a new instance of the <see cref="ContextMetadata" /> class.</summary>
    /// <param name="correlationId">The message correlation identifier.</param>
    /// <param name="userId">The initiating user identifier.</param>
    /// <param name="receivedDate">The received date.</param>
    /// <param name="sequenceNumber">The message sequence number.</param>
    /// <param name="sessionId">The message session identifier.</param>
    [JsonConstructor]
    public ContextMetadata(
        string correlationId,
        string userId,
        DateTimeOffset? receivedDate,
        long? sequenceNumber,
        string? sessionId)
    {
        CorrelationId = correlationId;
        UserId = userId;
        ReceivedDate = receivedDate;
        SequenceNumber = sequenceNumber;
        SessionId = sessionId;
    }

    /// <summary>Initializes a new instance of the <see cref="ContextMetadata" /> class.</summary>
    /// <param name="context">The context.</param>
    public ContextMetadata(IContextMetadata context)
        : this(
            (context ?? throw new ArgumentNullException(nameof(context))).CorrelationId,
            context.UserId,
            context.ReceivedDate,
            context.SequenceNumber,
            context.SessionId)
    {
    }

    /// <summary>
    /// Gets the message correlationId.
    /// </summary>
    public string CorrelationId { get; }

    /// <summary>
    /// Gets the date the message was received.
    /// </summary>
    public DateTimeOffset? ReceivedDate { get; }

    /// <summary>
    /// Gets the message sequence number.
    /// </summary>
    public long? SequenceNumber { get; }

    /// <summary>
    /// Gets the message session identifier.
    /// </summary>
    public string? SessionId { get; }

    /// <summary>
    /// Gets the initiating user identifier.
    /// </summary>
    public string UserId { get; }
}