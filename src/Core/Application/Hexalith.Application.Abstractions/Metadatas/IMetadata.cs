// <copyright file="IMetadata.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// Interface for all message metadatas.
/// </summary>
public interface IMetadata
{
    /// <summary>
    /// Gets the message context metadata.
    /// </summary>
    IContextMetadata Context { get; }

    /// <summary>
    /// Gets the message metadata.
    /// </summary>
    IMessageMetadata Message { get; }

    /// <summary>
    /// Gets the message scopes names.
    /// </summary>
    IEnumerable<string>? Scopes { get; }

    /// <summary>
    /// Gets the metadata version.
    /// </summary>
    IMetadataVersion Version { get; }
}