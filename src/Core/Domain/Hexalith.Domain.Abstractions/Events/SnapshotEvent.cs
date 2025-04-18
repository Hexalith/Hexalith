// <copyright file="SnapshotEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Domain.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a snapshot event in the domain, capturing the state of an aggregate at a specific point in time.
/// </summary>
/// <param name="AggregateName">The name of the aggregate.</param>
/// <param name="AggregateId">The unique identifier of the aggregate.</param>
/// <param name="Snapshot">A JSON serialized representation of the aggregate's state.</param>
[PolymorphicSerialization]
public partial record SnapshotEvent(
    [property: DataMember(Order = 1)] string AggregateName,
    [property: DataMember(Order = 2)] string AggregateId,
    [property: DataMember(Order = 3)] string Snapshot)
{
    /// <summary>
    /// Creates a new SnapshotEvent from the given aggregate.
    /// </summary>
    /// <param name="aggregate">The aggregate to create a snapshot from.</param>
    /// <returns>A new SnapshotEvent containing the serialized state of the aggregate.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the aggregate is null.</exception>
    public static SnapshotEvent Create(IDomainAggregate aggregate)
        => new(
            (aggregate ?? throw new ArgumentNullException(nameof(aggregate))).AggregateName,
            aggregate.AggregateId,
            JsonSerializer.Serialize(aggregate, aggregate.GetType()));

    /// <summary>
    /// Gets the aggregate of the specified type from the snapshot.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the aggregate to deserialize.</typeparam>
    /// <returns>The deserialized aggregate of the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the type is null.</exception>
    /// <exception cref="InvalidDataContractException">Thrown when the snapshot cannot be deserialized to the specified type.</exception>
    public TAggregate GetAggregate<TAggregate>()
        where TAggregate : IDomainAggregate
        => (TAggregate)GetAggregate(typeof(TAggregate));

    /// <summary>
    /// Gets the aggregate of the specified type from the snapshot.
    /// </summary>
    /// <param name="type">The type of the aggregate to deserialize.</param>
    /// <returns>The deserialized aggregate of the specified type.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the type is null.</exception>
    /// <exception cref="InvalidDataContractException">Thrown when the snapshot cannot be deserialized to the specified type.</exception>
    public IDomainAggregate GetAggregate([NotNull] Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return (IDomainAggregate)(JsonSerializer.Deserialize(Snapshot, type)
            ?? throw new InvalidDataContractException($"Could not deserialize to {type.Name} : \n{Snapshot}"));
    }
}