// <copyright file="Aggregate.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Aggregates;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Events;
using Hexalith.Domain.Exceptions;
using Hexalith.Domain.Messages;

/// <summary>
/// Class Aggregate.
/// Implements the <see cref="IAggregate" />.
/// </summary>
/// <seealso cref="IAggregate" />
[DataContract]
[DebuggerDisplay("{AggregateName}/{AggregateId}")]
[Obsolete("This class is obsolete.", false)]
public abstract record Aggregate : IAggregate
{
    /// <summary>
    /// Gets default string used for separating natural keys to compose the aggregate identifier.
    /// </summary>
    public static string Separator => "-";

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public virtual string AggregateId => DefaultAggregateId();

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public virtual string AggregateName => DefaultAggregateName();

    /// <summary>
    /// Checks the validity of the specified domain event for the aggregate.
    /// </summary>
    /// <param name="domainEvent">The domain event to check.</param>
    /// <exception cref="InvalidAggregateEventException">Thrown when the domain event is invalid for the aggregate.</exception>
    public virtual void CheckValid([NotNull] BaseEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        // if the snapshot is for this aggregate, we initialize it
        if (domainEvent.AggregateId == AggregateId && domainEvent.AggregateName == AggregateName)
        {
            return;
        }

        throw new InvalidAggregateEventException(
            this,
            domainEvent,
            false,
            $"Event aggregate 'Name={domainEvent.AggregateName}'/'Id={domainEvent.AggregateId}' is invalid. Expected: 'Name={AggregateName}'/'Id={AggregateId}'");
    }

    /// <inheritdoc/>
    public virtual (IAggregate Aggregate, IEnumerable<BaseMessage> Messages) Apply(BaseEvent domainEvent)
    {
        if (domainEvent is SnapshotEvent s)
        {
            // if the snapshot is for this aggregate, we initialize it
            return s.AggregateId == AggregateId && s.AggregateName == AggregateName
                ? ((IAggregate Aggregate, IEnumerable<BaseMessage> Messages))(s.GetAggregate(GetType()), [domainEvent])
                : throw new InvalidAggregateEventException(this, domainEvent, false, "Invalid snapshot aggregate id");
        }

        throw new InvalidAggregateEventException(this, domainEvent, false);
    }

    /// <inheritdoc/>
    public abstract bool IsInitialized();

    /// <summary>
    /// Normalizes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>System.String.</returns>
    public static string Normalize([NotNull] string id)
    {
        return string.IsNullOrWhiteSpace(id)
            ? throw new ArgumentException("The specified identifier cannot be empty or white space.", nameof(id))
            : id;
    }

    /// <summary>
    /// Get the aggregate identifier.
    /// </summary>
    /// <returns>The identifier.</returns>
    protected virtual string DefaultAggregateId() => DefaultAggregateName();

    /// <summary>
    /// Get the aggregate name.
    /// </summary>
    /// <returns>The name.</returns>
    protected virtual string DefaultAggregateName() => GetType().Name;
}