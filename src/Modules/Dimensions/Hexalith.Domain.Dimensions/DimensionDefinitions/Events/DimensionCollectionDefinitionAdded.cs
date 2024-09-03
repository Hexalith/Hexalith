// <copyright file="DimensionCollectionDefinitionAdded.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionDefinitions.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Dimension collection definition added event.
/// Implements the <see cref="Hexalith.Domain.Dimensions.DimensionDefinitions.Events.DimensionCollectionDefinitionEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Dimensions.DimensionDefinitions.Events.DimensionCollectionDefinitionEvent" />
[DataContract]
public class DimensionCollectionDefinitionAdded : DimensionCollectionDefinitionEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DimensionCollectionDefinitionAdded"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    [JsonConstructor]
    public DimensionCollectionDefinitionAdded(
        string partitionId,
        string originId,
        string id,
        string name,
        string? description)
    : base(partitionId, originId, id)
    {
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DimensionCollectionDefinitionAdded"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public DimensionCollectionDefinitionAdded() => Name = string.Empty;

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    [DataMember(Order = 2)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [DataMember(Order = 1)]
    public string Name { get; set; }
}