// <copyright file="DimensionHierarchy.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionHIerarchies;

using System;
using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Messages;

/// <summary>
/// Represents a user.
/// </summary>
[DataContract]
public record DimensionHierarchy(
    string PartitionId,
    [property: DataMember(Order = 2)] string Id,
    [property: DataMember(Order = 3)] string Name,
    [property: DataMember(Order = 4)] string Description,
    [property: DataMember(Order = 5)] IEnumerable<string> DimensionsIds) : PartitionedAggregate(PartitionId)
{
    /// <inheritdoc/>
    [Obsolete]
    public override (IAggregate Aggregate, IEnumerable<BaseMessage> Messages) Apply(BaseEvent domainEvent) => throw new NotImplementedException();

    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);
}