// <copyright file="NumberSequence.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.NumberSequences.Abstractions.Domain;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;

public record NumberSequence : Aggregate
{
    /// <inheritdoc/>
    public override (IAggregate Aggregate, IEnumerable<BaseEvent> Events) Apply(BaseEvent domainEvent) => throw new NotImplementedException();

    /// <inheritdoc/>
    public override bool IsInitialized() => throw new NotImplementedException();
}