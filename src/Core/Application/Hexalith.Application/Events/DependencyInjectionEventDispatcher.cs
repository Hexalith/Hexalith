// <copyright file="DependencyInjectionEventDispatcher.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Domain.Events;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class DependencyInjectionEventDispatcher.
/// Implements the <see cref="IIntegrationEventDispatcher" />.
/// </summary>
/// <seealso cref="IIntegrationEventDispatcher" />
public class DependencyInjectionEventDispatcher : IIntegrationEventDispatcher
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// The service provider.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyInjectionEventDispatcher" /> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="logger">The logger.</param>
    public DependencyInjectionEventDispatcher(IServiceProvider serviceProvider, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<IEnumerable<BaseCommand>>> ApplyAsync(IEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Dispatching event {EventType} with aggregate id {AggregateName}-{AggregateId}", @event.TypeName, @event.AggregateName, @event.AggregateId);
        return await Task.WhenAll(
            GetHandlers(@event)
                .Select(p => p.ApplyAsync(@event, cancellationToken)));
    }

    /// <summary>
    /// Gets the event handler of type IEventHandler.<MyEvent>.
    /// </summary>
    /// <param name="event">The event to handle.</param>
    /// <returns>IEventHandler list.</returns>
    private IEnumerable<IIntegrationEventHandler> GetHandlers(IEvent @event)
    {
        Type eventType = @event.GetType();
        Type eventHandlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
        Type eventHandlerTypeList = typeof(IEnumerable<>).MakeGenericType(eventHandlerType);
        return (_serviceProvider.GetService(eventHandlerTypeList) as IEnumerable ?? Array.Empty<IIntegrationEventHandler>())
            .Cast<IIntegrationEventHandler>()
            .ToList();
    }
}