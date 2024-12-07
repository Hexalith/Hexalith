// <copyright file="DimensionDefinition.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionCollectionDefinitions.Entities;

using System.Runtime.Serialization;

/// <summary>
/// Represents a definition of a dimension.
/// </summary>
/// <param name="Name">The name of the dimension.</param>
/// <param name="Description">An optional description of the dimension.</param>
/// <param name="Values">The values of the dimension.</param>
[DataContract]
public record DimensionDefinition(
    [property: DataMember(Order = 1)] string Name,
    [property: DataMember(Order = 2)] string Description,
    [property: DataMember(Order = 3)] IEnumerable<(string Code, string Name)> Values)
{
}