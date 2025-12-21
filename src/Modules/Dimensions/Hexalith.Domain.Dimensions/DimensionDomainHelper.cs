// <copyright file="DimensionDomainHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions;

using Hexalith.Domain.Dimensions.DimensionCollectionDefinitions.Aggregates;

/// <summary>
/// Provides helper methods and properties for working with dimension domains.
/// </summary>
public static class DimensionDomainHelper
{
    /// <summary>
    /// Gets the name of the DimensionCollectionDefinition aggregate.
    /// </summary>
    /// <value>
    /// The name of the DimensionCollectionDefinition aggregate.
    /// </value>
    public static string DimensionCollectionDefinitionDomainName => nameof(DimensionCollectionDefinition);

    /// <summary>
    /// Builds the aggregate ID for a DimensionCollectionDefinition.
    /// </summary>
    /// <param name="id">The base ID to use for building the aggregate ID.</param>
    /// <returns>The constructed aggregate ID for the DimensionCollectionDefinition.</returns>
    public static string BuildDimensionCollectionDefinitionAggregateId(string id) => id;
}