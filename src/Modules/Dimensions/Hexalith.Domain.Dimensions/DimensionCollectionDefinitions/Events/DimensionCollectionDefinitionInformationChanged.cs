// <copyright file="DimensionCollectionDefinitionInformationChanged.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionCollectionDefinitions.Events;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an event that occurs when the information of a dimension collection definition has changed.
/// </summary>
/// <param name="Id">The unique identifier for the dimension collection definition.</param>
/// <param name="Name">The new name of the dimension collection definition.</param>
/// <param name="Description">The new description of the dimension collection definition.</param>
[PolymorphicSerialization]
public partial record DimensionCollectionDefinitionInformationChanged(
    string Id,
    [property: DataMember] string Name,
    [property: DataMember] string? Description) : DimensionCollectionDefinitionEvent(Id);