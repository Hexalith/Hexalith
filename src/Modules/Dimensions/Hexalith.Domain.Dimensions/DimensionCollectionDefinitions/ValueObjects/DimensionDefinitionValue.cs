// <copyright file="DimensionDefinitionValue.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionCollectionDefinitions.ValueObjects;

using System.Runtime.Serialization;

/// <summary>
/// Represents a dimension value.
/// </summary>
/// <param name="Id">The unique identifier for the dimension value.</param>
/// <param name="Name">The name of the dimension value.</param>
[DataContract]
public record class DimensionDefinitionValue(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name)
{
}