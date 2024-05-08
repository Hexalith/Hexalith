// <copyright file="DimensionCollectionDefinition.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionDefinitions.Aggregates;

using System;
using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Dimensions.DimensionDefinitions.Entities;
using Hexalith.Domain.Events;

/// <summary>
/// Class DimensionDefinition.
/// Implements the <see cref="Aggregate" />
/// Implements the <see cref="IAggregate" />.
/// </summary>
/// <seealso cref="Aggregate" />
/// <seealso cref="IAggregate" />
[DataContract]
public record DimensionCollectionDefinition(
    string PartitionId,
    string OriginId,
    string Id,
    [property: DataMember(Order = 3)] string Name,
    [property: DataMember(Order = 4)] string Description,
    [property: DataMember(Order = 5)] IEnumerable<DimensionDefinition> Values) : CommonEntityAggregate(PartitionId, OriginId, Id)
{
    /// <inheritdoc/>
    public override (IAggregate Aggregate, IEnumerable<BaseEvent> Events) Apply(BaseEvent domainEvent) => throw new NotImplementedException();

    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);
}