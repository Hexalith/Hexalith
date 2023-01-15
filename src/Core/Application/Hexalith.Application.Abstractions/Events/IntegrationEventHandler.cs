// <copyright file="IntegrationEventHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Events;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Domain.Abstractions.Events;

using System;
using System.Threading.Tasks;

/// <summary>
/// Class EventHandler.
/// Implements the <see cref="Hexalith.Application.Abstractions.Events.IEventHandler{TEvent}" />.
/// </summary>
/// <typeparam name="TEvent">The type of the t event.</typeparam>
/// <seealso cref="Hexalith.Application.Abstractions.Events.IEventHandler{TEvent}" />
public abstract class IntegrationEventHandler<TEvent> : IIntegrationEventHandler<TEvent>
    where TEvent : IEvent
{
    /// <inheritdoc/>
    public abstract Task<IEnumerable<BaseCommand>> ApplyAsync(TEvent @event, CancellationToken cancellationToken);

    /// <inheritdoc/>
    Task<IEnumerable<BaseCommand>> IIntegrationEventHandler.ApplyAsync(IEvent @event, CancellationToken cancellationToken)
    {
        return ApplyAsync(ToEvent(@event), cancellationToken);
    }

    /// <summary>
    /// Converts to event.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <returns>TEvent.</returns>
    /// <exception cref="System.ArgumentException">event.</exception>
    private static TEvent ToEvent(IEvent @event)
    {
        return @event is TEvent c
            ? c
            : throw new ArgumentException($"{@event.GetType().Name} is an invalid event type. Expected: {typeof(TEvent).Name}.", nameof(@event));
    }
}