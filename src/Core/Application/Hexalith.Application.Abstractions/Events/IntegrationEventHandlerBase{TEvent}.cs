// <copyright file="IntegrationEventHandlerBase{TEvent}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System;
using System.Threading.Tasks;

using Hexalith.Commons.Metadatas;

/// <summary>
/// Represents a base class for integration event handlers.
/// </summary>
/// <typeparam name="TEvent">The type of the event to be handled.</typeparam>
public abstract class IntegrationEventHandlerBase<TEvent> : IIntegrationEventHandler<TEvent>
    where TEvent : class
{
    /// <inheritdoc/>
    Task<IEnumerable<object>> IIntegrationEventHandler.ApplyAsync(object baseEvent, Metadata metadata, CancellationToken cancellationToken)
        => ApplyAsync(ToEvent(baseEvent), metadata, cancellationToken);

    /// <inheritdoc/>
    public abstract Task<IEnumerable<object>> ApplyAsync(TEvent baseEvent, Metadata metadata, CancellationToken cancellationToken);

    /// <summary>
    /// Converts the given object to the expected event type.
    /// </summary>
    /// <param name="event">The event object to convert.</param>
    /// <returns>The event object cast to the expected type.</returns>
    /// <exception cref="ArgumentException">Thrown when the event is not of the expected type.</exception>
    private static TEvent ToEvent(object @event)
    {
        return @event is TEvent c
            ? c
            : throw new ArgumentException($"{@event.GetType().Name} is an invalid event type. Expected: {typeof(TEvent).Name}.", nameof(@event));
    }
}