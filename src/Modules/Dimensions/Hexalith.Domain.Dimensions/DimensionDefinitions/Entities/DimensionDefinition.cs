// <copyright file="DimensionDefinition.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionDefinitions.Entities;

using System;
using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Represents a role.
/// Implements the <see cref="Aggregate" />
/// Implements the <see cref="IAggregate" />
/// Implements the <see cref="IEquatable{Aggregate}" />
/// Implements the <see cref="IEquatable{Role}" />.
/// </summary>
/// <seealso cref="Aggregate" />
/// <seealso cref="IAggregate" />
/// <seealso cref="IEquatable{Aggregate}" />
/// <seealso cref="IEquatable{Role}" />
[DataContract]
public record DimensionDefinition(
    [property: DataMember(Order = 1)] string Name,
    [property: DataMember(Order = 2)] string Description,
    [property: DataMember(Order = 3)] IEnumerable<(string Code, string Name)> Values)
{
}