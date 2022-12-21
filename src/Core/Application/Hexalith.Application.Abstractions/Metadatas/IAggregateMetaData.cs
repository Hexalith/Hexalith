// <copyright file="IAggregateMetaData.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// Aggregate metadata.
/// </summary>
public interface IAggregateMetadata
{
    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    string Name { get; }
}