// <copyright file="SnapshotEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Class SnapshotEvent.
/// Implements the <see cref="Hexalith.Domain.Events.BaseEvent" />.
/// </summary>
/// <typeparam name="TAggregate">The type of the t aggregate.</typeparam>
/// <seealso cref="Hexalith.Domain.Events.BaseEvent" />
[DataContract]
public abstract class SnapshotEvent<TAggregate> : BaseEvent
    where TAggregate : Aggregate, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnapshotEvent{TAggregate}" /> class.
    /// </summary>
    /// <param name="aggregate">The aggregate.</param>
    protected SnapshotEvent(TAggregate aggregate) => Aggregate = aggregate;

    /// <summary>
    /// Initializes a new instance of the <see cref="SnapshotEvent{TAggregate}" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected SnapshotEvent() => Aggregate = new();

    /// <summary>
    /// Gets or sets the aggregate.
    /// </summary>
    /// <value>The aggregate.</value>
    [DataMember(Order = 10)]
    public TAggregate Aggregate { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => Aggregate.AggregateId;

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => Aggregate.AggregateName;
}