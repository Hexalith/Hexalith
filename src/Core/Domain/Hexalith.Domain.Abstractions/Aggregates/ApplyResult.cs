// <copyright file="ApplyResult.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Aggregates;

using System.Collections.Generic;

using Hexalith.Domain.Events;

/// <summary>
/// Represents the result of applying domain events to an aggregate.
/// </summary>
/// <param name="Aggregate">The domain aggregate.</param>
/// <param name="Messages">The collection of messages produced during the application of events.</param>
/// <param name="Failed">A flag indicating whether the application of events failed.</param>
/// <param name="Reason">The reason why the application of events failed.</param>
public record ApplyResult(
    IDomainAggregate Aggregate,
    IEnumerable<object> Messages,
    bool Failed,
    string? Reason = null)
{
    /// <summary>
    /// Creates an ApplyResult indicating that the event is not implemented.
    /// </summary>
    /// <param name="aggregate">The domain aggregate.</param>
    /// <returns>An ApplyResult indicating failure due to unimplemented event.</returns>
    public static ApplyResult NotImplemented(IDomainAggregate aggregate)
        => new(aggregate, [], true, "Event not implemented");

    /// <summary>
    /// Creates an ApplyResult indicating that an invalid event was applied.
    /// </summary>
    /// <param name="aggregate">The domain aggregate.</param>
    /// <param name="domainEvent">The domain event that was invalid.</param>
    /// <returns>An ApplyResult indicating failure due to invalid event.</returns>
    public static ApplyResult InvalidEvent(IDomainAggregate aggregate, object domainEvent)
        => new(
                aggregate,
                [InvalidEventApplied.CreateNotSupportedAppliedEvent(
                    aggregate.AggregateName,
                    aggregate.AggregateId,
                    domainEvent)],
                true);
}