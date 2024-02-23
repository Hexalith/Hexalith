// <copyright file="DimensionHierarchy.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.Aggregates;

using System;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;

/// <summary>
/// Represents a user.
/// </summary>
public record DimensionHierarchy(string Id, string Name, string Description, IEnumerable<string> DimensionsIds) : Aggregate
{
    /// <inheritdoc/>
    public override IAggregate Apply(BaseEvent domainEvent) => throw new NotImplementedException();

    /// <inheritdoc/>
    public override bool IsInitialized() => throw new NotImplementedException();
}