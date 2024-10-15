// <copyright file="DimensionCollectionDefinitionInformationChanged.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionCollectionDefinitions.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an event that occurs when the information of a dimension collection definition has changed.
/// </summary>
[PolymorphicSerialization]
public partial record DimensionCollectionDefinitionInformationChanged(
    /// <summary>
    /// Gets the unique identifier of the dimension collection definition.
    /// </summary>
    string Id,

    /// <summary>
    /// Gets the updated name of the dimension collection definition.
    /// </summary>
    string Name,

    /// <summary>
    /// Gets the updated description of the dimension collection definition. Can be null if no description is provided.
    /// </summary>
    string? Description) : DimensionCollectionDefinitionEvent(Id);
