// <copyright file="DimensionDefinition.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionDefinitions.Aggregates;

using System;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Dimensions.DimensionDefinitions.Entities;
using Hexalith.Domain.Events;

/// <summary>
/// Class DimensionDefinition.
/// Implements the <see cref="Aggregate" />
/// Implements the <see cref="IAggregate" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// Implements the <see cref="System.IEquatable{Hexalith.Domain.Dimensions.DimensionDefinitions.Aggregates.DimensionDefinition}" />.
/// </summary>
/// <seealso cref="Aggregate" />
/// <seealso cref="IAggregate" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Aggregates.Aggregate}" />
/// <seealso cref="System.IEquatable{Hexalith.Domain.Dimensions.DimensionDefinitions.Aggregates.DimensionDefinition}" />
public record DimensionDefinition(
    string PartitionId,
    string Id,
    string Name,
    string Description,
    IEnumerable<DimensionValue> Values) : Aggregate
{
    /// <inheritdoc/>
    public override (IAggregate Aggregate, IEnumerable<BaseEvent> Events) Apply(BaseEvent domainEvent) => throw new NotImplementedException();

    /// <inheritdoc/>
    public override bool IsInitialized() => throw new NotImplementedException();
}