// <copyright file="SnapshotEvent - Copy.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
public record AggregateSnapshotEvent(string SourceAggregateName, string SourceAggregateId, string Snapshot)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateSnapshotEvent"/> class.
    /// </summary>
    /// <param name="aggregate">The aggregate.</param>
    public AggregateSnapshotEvent(IDomainAggregate aggregate)
        : this(
              (aggregate ?? throw new ArgumentNullException(nameof(aggregate))).AggregateName,
              aggregate.AggregateId,
              JsonSerializer.Serialize(aggregate, aggregate.GetType()))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateSnapshotEvent"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public AggregateSnapshotEvent()
        : this(string.Empty, string.Empty, string.Empty)
    {
    }

    /// <summary>
    /// Gets the aggregate of the specified type.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
    /// <returns>The aggregate of the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the type is null.</exception>
    /// <exception cref="InvalidDataContractException">Thrown when the snapshot cannot be deserialized to the specified type.</exception>
    public TAggregate GetAggregate<TAggregate>()
        where TAggregate : IDomainAggregate
        => (TAggregate)GetAggregate(typeof(TAggregate));

    /// <summary>
    /// Gets the aggregate of the specified type.
    /// </summary>
    /// <param name="type">The type of the aggregate.</param>
    /// <returns>The aggregate of the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the type is null.</exception>
    /// <exception cref="InvalidDataContractException">Thrown when the snapshot cannot be deserialized to the specified type.</exception>
    public IDomainAggregate GetAggregate([NotNull] Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return (IDomainAggregate)(JsonSerializer.Deserialize(Snapshot, type)
            ?? throw new InvalidDataContractException($"Could not deserialize to {type.Name} : \n{Snapshot}"));
    }
}