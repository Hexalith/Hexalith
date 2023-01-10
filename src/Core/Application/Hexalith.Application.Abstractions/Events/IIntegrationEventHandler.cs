// <copyright file="IIntegrationEventHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Events;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Domain.Abstractions.Events;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Event handler interface.
/// </summary>
public interface IIntegrationEventHandler
{
    /// <summary>
    /// Handles the event.
    /// </summary>
    /// <param name="event">The event to execute.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The generated events with their metadata.</returns>
    Task<IEnumerable<BaseCommand>> ApplyAsync(IEvent @event, CancellationToken cancellationToken);
}

/// <summary>
/// Event handler interface.
/// </summary>
/// <typeparam name="TEvent">The type of the event.</typeparam>
public interface IIntegrationEventHandler<TEvent> : IIntegrationEventHandler
    where TEvent : IEvent
{
    /// <summary>
    /// Handles the event.
    /// </summary>
    /// <param name="event">The event to execute.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The generated events with their metadata.</returns>
    Task<IEnumerable<BaseCommand>> ApplyAsync(TEvent @event, CancellationToken cancellationToken);
}
