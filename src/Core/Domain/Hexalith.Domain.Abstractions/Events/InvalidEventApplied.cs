// <copyright file="InvalidEventApplied.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using System.Text.Json;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents an invalid event that was applied to an aggregate in the domain.
/// This record is used to capture and track events that could not be properly
/// processed by an aggregate, allowing for error handling, logging, and debugging
/// of event processing failures in the domain-driven design context.
/// </summary>
/// <remarks>
/// This record is particularly useful in event sourcing scenarios where we need to:
/// <list type="bullet">
/// <item><description>Track failed event applications for audit purposes</description></item>
/// <item><description>Debug issues with event processing in aggregates</description></item>
/// <item><description>Handle recovery scenarios where events couldn't be applied</description></item>
/// </list>
/// The record stores both the metadata about the failed event application and the
/// actual event content in serialized form for later analysis.
///
/// <para>
/// The EventContent property contains the JSON serialized form of the event,
/// which can be deserialized back to the original event type when needed.
/// </para>
///
/// <para>
/// Example usage:
/// <code>
/// try
/// {
///     aggregate.ApplyEvent(someEvent);
/// }
/// catch (UnsupportedEventException ex)
/// {
///     var invalidEvent = InvalidEventApplied.CreateNotSupportedAppliedEvent(
///         nameof(MyAggregate),
///         aggregate.Id,
///         someEvent,
///         ex.Message);
///     // Log or handle the invalid event...
/// }
/// </code>
/// </para>
///
/// <para>
/// Thread Safety:
/// This record is immutable and thread-safe. All properties are read-only and
/// the record can be safely shared between multiple threads.
/// </para>
/// </remarks>
/// <param name="AggregateName">The name of the aggregate to which the invalid event was applied.</param>
/// <param name="AggregateId">The unique identifier of the aggregate.</param>
/// <param name="EventType">The type of the event that was invalid.</param>
/// <param name="EventContent">The serialized JSON content of the invalid event. Uses System.Text.Json for serialization.</param>
/// <param name="Reason">The reason why the event was considered invalid.</param>
[PolymorphicSerialization] // Enables polymorphic serialization for handling different event types
public partial record InvalidEventApplied(string AggregateName, string AggregateId, string EventType, string EventContent, string Reason)
{
    /// <summary>
    /// Creates an InvalidEventApplied instance for an event that is not supported by the aggregate.
    /// This factory method handles the serialization of the event object and captures the necessary
    /// information about why the event application failed.
    /// </summary>
    /// <param name="aggregateName">The name of the aggregate that doesn't support the event.</param>
    /// <param name="aggregateId">The unique identifier of the aggregate.</param>
    /// <param name="event">The event object that is not supported.</param>
    /// <returns>A new instance of InvalidEventApplied containing details about the unsupported event.</returns>
    /// <remarks>
    /// This method is typically used when an aggregate receives an event type it doesn't
    /// know how to handle, or when an event cannot be applied due to business rules or
    /// current aggregate state.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when aggregateName, aggregateId, or event is null.</exception>
    /// <exception cref="JsonException">Thrown when the event object cannot be serialized to JSON.</exception>
    public static InvalidEventApplied CreateNotSupportedAppliedEvent(string aggregateName, string aggregateId, object @event)
    {
        ArgumentNullException.ThrowIfNull(aggregateName);
        ArgumentNullException.ThrowIfNull(aggregateId);
        ArgumentNullException.ThrowIfNull(@event);

        return new InvalidEventApplied(
            aggregateName,
            aggregateId,
            @event.GetType().FullName ?? "Unknown type.",
            JsonSerializer.Serialize(@event, @event.GetType()),
            "Event not supported.");
    }
}