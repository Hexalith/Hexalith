// <copyright file="IMessageMetadata.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// The message metadata.
/// </summary>
public interface IMessageMetadata
{
    /// <summary>
    /// Gets aggregate metadata.
    /// </summary>
    IAggregateMetadata Aggregate { get; }

    /// <summary>
    /// Gets the message name.
    /// </summary>
    DateTimeOffset Date { get; }

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the message version.
    /// </summary>
    IMessageVersion Version { get; }
}