// <copyright file="IContextMetadata.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Metadatas;

/// <summary>
/// The message context metadata.
/// </summary>
public interface IContextMetadata
{
    /// <summary>
    /// Gets message correlation id. It's used to group messages that are related to the same business action.
    /// </summary>
    string CorrelationId { get; }

    /// <summary>
    /// Gets the date the message was received.
    /// </summary>
    DateTimeOffset? ReceivedDate { get; }

    /// <summary>
    /// Gets the message sequence number.
    /// </summary>
    public long? SequenceNumber { get; }

    /// <summary>
    /// Gets session identifier.
    /// </summary>
    string? SessionId { get; }

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    string UserId { get; }
}