// <copyright file="AggregateMetadata.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// The aggregate metadata.
/// </summary>
[DataContract]
public class AggregateMetadata : IAggregateMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateMetadata" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public AggregateMetadata() => Id = Name = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateMetadata" /> class.
    /// </summary>
    /// <param name="id">The aggregate identifier.</param>
    /// <param name="name">The aggregate name.</param>
    [JsonConstructor]
    public AggregateMetadata(string id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    public string Name { get; }
}