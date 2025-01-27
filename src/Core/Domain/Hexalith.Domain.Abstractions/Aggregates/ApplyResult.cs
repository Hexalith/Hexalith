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
    /// Creates an ApplyResult indicating that the aggregate is not initialized.
    /// </summary>
    /// <param name="aggregate">The domain aggregate.</param>
    /// <returns>An ApplyResult indicating failure due to uninitialized aggregate.</returns>
    public static ApplyResult NotInitialized(IDomainAggregate aggregate)
        => new(aggregate, [], true, $"Cannot apply changes to an uninitialized manhole element ({aggregate.AggregateName}).");

    /// <summary>
    /// Creates an ApplyResult indicating that the aggregate is not enabled.
    /// </summary>
    /// <param name="aggregate">The domain aggregate.</param>
    /// <returns>An ApplyResult indicating failure due to disabled aggregate.</returns>
    public static ApplyResult NotEnabled(IDomainAggregate aggregate)
        => new(aggregate, [], true, $"Cannot change a disabled manhole element ({aggregate.AggregateName}).");

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

    /// <summary>
    /// Creates an ApplyResult indicating a successful application of events.
    /// </summary>
    /// <param name="aggregate">The domain aggregate.</param>
    /// <param name="messages">The collection of messages produced during the application of events.</param>
    /// <returns>An ApplyResult indicating success.</returns>
    public static ApplyResult Success(IDomainAggregate aggregate, IEnumerable<object> messages)
        => new(aggregate, messages, false);

    /// <summary>
    /// Creates an ApplyResult indicating an error during the application of events.
    /// </summary>
    /// <param name="aggregate">The domain aggregate.</param>
    /// <param name="reason">The reason for the error.</param>
    /// <returns>An ApplyResult indicating failure due to an error.</returns>
    public static ApplyResult Error(IDomainAggregate aggregate, string reason)
        => new(aggregate, [], true, reason);
}