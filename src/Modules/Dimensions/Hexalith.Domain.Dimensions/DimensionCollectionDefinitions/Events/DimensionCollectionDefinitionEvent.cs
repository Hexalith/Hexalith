// <copyright file="DimensionCollectionDefinitionEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionCollectionDefinitions.Events;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents an event related to a dimension collection definition.
/// </summary>
/// <param name="Id">The unique identifier for the dimension collection definition.</param>
[PolymorphicSerialization]
public partial record DimensionCollectionDefinitionEvent(string Id)
{
    /// <summary>
    /// Gets the aggregate identifier for the dimension collection definition.
    /// </summary>
    /// <returns>The aggregate identifier constructed from the Id.</returns>
    public string DomainId
        => DimensionDomainHelper.BuildDimensionCollectionDefinitionDomainId(Id);

    /// <summary>
    /// Gets the name of the aggregate for dimension collection definitions.
    /// </summary>
    /// <returns>The name of the aggregate for dimension collection definitions.</returns>
    public string DomainName
        => DimensionDomainHelper.DimensionCollectionDefinitionDomainName;
}