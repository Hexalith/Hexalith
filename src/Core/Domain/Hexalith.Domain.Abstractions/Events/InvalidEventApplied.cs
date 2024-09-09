// <copyright file="InvalidEventApplied.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Domain.Events;

using System.Text.Json;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an invalid event applied.
/// </summary>
/// <param name="AggregateName">The aggregate name.</param>
/// <param name="AggregateId">The aggregate identifier.</param>
/// <param name="EventName">The event name.</param>
/// <param name="EventContent">The event content serialized in JSON.</param>
public abstract record InvalidEventApplied(string AggregateName, string AggregateId, string EventName, string EventContent)
{
    /// <summary>
    /// Creates an instance of InvalidEventApplied.
    /// </summary>
    /// <typeparam name="TInvalidEvent">The type of the invalid event applied.</typeparam>
    /// <param name="aggregateName">The aggregate name.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="event">The event object.</param>
    /// <param name="create">The factory method to create the invalid event applied.</param>
    /// <returns>An instance of InvalidEventApplied.</returns>
    public static TInvalidEvent Create<TInvalidEvent>(string aggregateName, string aggregateId, object @event, Func<string, string, string, string, TInvalidEvent> create)
        where TInvalidEvent : InvalidEventApplied
    {
        ArgumentNullException.ThrowIfNull(@event);
        ArgumentNullException.ThrowIfNull(create);

        string eventName = (Attribute.GetCustomAttribute(typeof(TInvalidEvent), typeof(PolymorphicSerializationAttribute)) is not PolymorphicSerializationAttribute attribute)
            ? @event.GetType().Name :
            attribute.TypeName;

        return create(aggregateName, aggregateId, eventName, JsonSerializer.Serialize(@event));
    }
}