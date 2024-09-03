// <copyright file="NumberSequence.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.NumberSequences.Abstractions.Domain;

using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Messages;

/// <summary>
/// Represents a number sequence.
/// </summary>
[DataContract]
public record NumberSequence(
    string PartitionId,
    [property: DataMember(Order = 2)] string CompanyId,
    [property: DataMember(Order = 3)] string Id) : PartitionedAggregate(PartitionId)
{
    /// <inheritdoc/>
    [Obsolete]
    public override (IAggregate Aggregate, IEnumerable<BaseMessage> Messages) Apply(BaseEvent domainEvent) => throw new NotImplementedException();

    /// <inheritdoc/>
    public override bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);
}