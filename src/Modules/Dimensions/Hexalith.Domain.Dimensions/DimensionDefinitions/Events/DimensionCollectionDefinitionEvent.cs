// <copyright file="DimensionCollectionDefinitionEvent.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionDefinitions.Events;

using System.Text.Json.Serialization;

using Hexalith.Domain.Events;
using Hexalith.Extensions;

/// <summary>
/// Represents a base class for dimension collection definition events.
/// </summary>
/// <remarks>
/// This class is used as a base class for all dimension collection definition events.
/// </remarks>
/// <seealso cref="CommonEntityEvent"/>
public abstract class DimensionCollectionDefinitionEvent : CommonEntityEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DimensionCollectionDefinitionEvent"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The event identifier.</param>
    [JsonConstructor]
    protected DimensionCollectionDefinitionEvent(string partitionId, string originId, string id)
        : base(partitionId, originId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DimensionCollectionDefinitionEvent"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor is marked as obsolete and should only be used for serialization purposes.
    /// </remarks>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected DimensionCollectionDefinitionEvent()
    {
    }
}