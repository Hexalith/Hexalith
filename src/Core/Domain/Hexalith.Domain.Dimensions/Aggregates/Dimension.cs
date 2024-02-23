// <copyright file="Dimension.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.Aggregates;

using System;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;

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
public record Dimension(
    string PartitionId,
    string Id,
    string Name,
    string Description,
    string IEnumerable<DimentionValue>) : Aggregate
{
    /// <inheritdoc/>
    public override IAggregate Apply(BaseEvent domainEvent) => throw new NotImplementedException();

    /// <inheritdoc/>
    public override bool IsInitialized() => throw new NotImplementedException();
}