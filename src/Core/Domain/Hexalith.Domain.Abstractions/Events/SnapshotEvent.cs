// <copyright file="SnapshotEvent.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Domain.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Represents a snapshot event in the domain.
/// </summary>
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
    /// Gets or sets the snapshot.
    /// </summary>
    [DataMember(Order = 12)]
    public string Snapshot { get; set; }

    /// <summary>
    /// Gets or sets the source aggregate identifier.
    /// </summary>
    [DataMember(Order = 11)]
    public string SourceAggregateId { get; set; }

    /// <summary>
    /// Gets or sets the name of the source aggregate.
    /// </summary>
    [DataMember(Order = 10)]
    public string SourceAggregateName { get; set; }

    /// <summary>
    /// Gets the aggregate of the specified type.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
    /// <returns>The aggregate of the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the type is null.</exception>
    /// <exception cref="InvalidDataContractException">Thrown when the snapshot cannot be deserialized to the specified type.</exception>
    public TAggregate GetAggregate<TAggregate>()
        where TAggregate : IAggregate
        => (TAggregate)GetAggregate(typeof(TAggregate));

    /// <summary>
    /// Gets the aggregate of the specified type.
    /// </summary>
    /// <param name="type">The type of the aggregate.</param>
    /// <returns>The aggregate of the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the type is null.</exception>
    /// <exception cref="InvalidDataContractException">Thrown when the snapshot cannot be deserialized to the specified type.</exception>
    public IAggregate GetAggregate([NotNull] Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return (IAggregate)(JsonSerializer.Deserialize(Snapshot, type)
            ?? throw new InvalidDataContractException($"Could not deserialize to {type.Name} : \n{Snapshot}"));
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => SourceAggregateId;

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => SourceAggregateName;
}