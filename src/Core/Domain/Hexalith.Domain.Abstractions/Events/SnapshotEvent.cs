// <copyright file="SnapshotEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Domain.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Class SnapshotEvent.
/// Implements the <see cref="Hexalith.Domain.Events.BaseEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.BaseEvent" />
[DataContract]
public class SnapshotEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SnapshotEvent" /> class.
    /// </summary>
    /// <param name="sourceAggregateName">Name of the source aggregate.</param>
    /// <param name="sourceAggregateId">The source aggregate identifier.</param>
    /// <param name="snapshot">The snapshot.</param>
    public SnapshotEvent(string sourceAggregateName, string sourceAggregateId, string snapshot)
    {
        SourceAggregateName = sourceAggregateName;
        SourceAggregateId = sourceAggregateId;
        Snapshot = snapshot;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnapshotEvent" /> class.
    /// </summary>
    /// <param name="aggregate">The aggregate.</param>
    public SnapshotEvent(IAggregate aggregate)
        : this(
              (aggregate ?? throw new ArgumentNullException(nameof(aggregate))).AggregateName,
              aggregate.AggregateId,
              JsonSerializer.Serialize(aggregate, aggregate.GetType()))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SnapshotEvent" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public SnapshotEvent() => SourceAggregateId = SourceAggregateName = Snapshot = string.Empty;

    /// <summary>
    /// Gets or sets the aggregate.
    /// </summary>
    /// <value>The aggregate.</value>
    [DataMember(Order = 12)]
    public string Snapshot { get; set; }

    /// <summary>
    /// Gets or sets the source aggregate identifier.
    /// </summary>
    /// <value>The source aggregate identifier.</value>
    [DataMember(Order = 11)]
    public string SourceAggregateId { get; set; }

    /// <summary>
    /// Gets or sets the name of the source aggregate.
    /// </summary>
    /// <value>The name of the source aggregate.</value>
    [DataMember(Order = 10)]
    public string SourceAggregateName { get; set; }

    /// <summary>
    /// Gets the aggregate.
    /// </summary>
    /// <typeparam name="TAggregate">Aggregate type.</typeparam>
    /// <returns>T.</returns>
    /// <exception cref="InvalidDataContractException">Could not deserialize to {typeof(T).Name} : \n{Snapshot}.</exception>
    public TAggregate GetAggregate<TAggregate>()
        where TAggregate : IAggregate
        => JsonSerializer.Deserialize<TAggregate>(Snapshot) ?? throw new InvalidDataContractException($"Could not deserialize to {typeof(TAggregate).Name} : \n{Snapshot}");

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => SourceAggregateId;

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => SourceAggregateName;
}