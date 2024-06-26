﻿// <copyright file="DimensionDefinitionValue.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionDefinitions.ValueObjects;

using System.Runtime.Serialization;

/// <summary>
/// Represents a dimension value.
/// </summary>
[DataContract]
public record class DimensionDefinitionValue(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name)
{
}