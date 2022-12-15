// <copyright file="ContextMetadata.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// The context metadata.
/// </summary>
public class ContextMetadata : IContextMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContextMetadata" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public ContextMetadata()
    {
        CorrelationId = UserId = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContextMetadata" /> class.
    /// </summary>
    /// <param name="correlationId">The message correlation identifier.</param>
    /// <param name="userId">The initiating user identifier.</param>
    /// <param name="sequenceNumber">The message sequence number.</param>
    /// <param name="sessionId">The message session identifier.</param>
    public ContextMetadata(string correlationId, string userId, long sequenceNumber, string? sessionId)
    {
        CorrelationId = correlationId;
        UserId = userId;
        SequenceNumber = sequenceNumber;
        SessionId = sessionId;
    }

    /// <summary>
    /// Gets the message correlationId.
    /// </summary>
    public string CorrelationId { get; }

    /// <summary>
    /// Gets the message sequence number.
    /// </summary>
    public long SequenceNumber { get; }

    /// <summary>
    /// Gets the message session identifier.
    /// </summary>
    public string? SessionId { get; }

    /// <summary>
    /// Gets the initiating user identifier.
    /// </summary>
    public string UserId { get; }
}