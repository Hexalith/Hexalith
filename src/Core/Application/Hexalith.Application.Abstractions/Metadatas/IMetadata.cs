// <copyright file="IMetadata.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Metadatas;

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
    /// Gets the major version.
    /// </summary>
    /// <value>The major version.</value>
    int MajorVersion { get; }

    /// <summary>
    /// Gets the message metadata.
    /// </summary>
    IMessageMetadata Message { get; }

    /// <summary>
    /// Gets the minor version.
    /// </summary>
    /// <value>The minor version.</value>
    int MinorVersion { get; }

    /// <summary>
    /// Gets the message scopes names.
    /// </summary>
    IEnumerable<string>? Scopes { get; }

    /// <summary>
    /// Gets the name of the type.
    /// </summary>
    /// <value>The name of the type.</value>
    string TypeName { get; }
}