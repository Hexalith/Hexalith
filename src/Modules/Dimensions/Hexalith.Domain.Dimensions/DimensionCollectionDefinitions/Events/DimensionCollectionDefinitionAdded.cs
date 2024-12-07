// <copyright file="DimensionCollectionDefinitionAdded.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionCollectionDefinitions.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an event that occurs when a dimension collection definition is added.
/// </summary>
/// <remarks>
/// This class inherits from <see cref="DimensionCollectionDefinitionEvent"/> and uses the <see cref="PolymorphicSerializationAttribute"/> for serialization purposes.
/// </remarks>
/// <param name="Id">The unique identifier for the dimension collection definition.</param>
/// <param name="Name">The name of the dimension collection definition.</param>
/// <param name="Description">An optional description of the dimension collection definition.</param>
[PolymorphicSerialization]
public partial record DimensionCollectionDefinitionAdded(
    string Id,
    string Name,
    string? Description) : DimensionCollectionDefinitionEvent(Id);