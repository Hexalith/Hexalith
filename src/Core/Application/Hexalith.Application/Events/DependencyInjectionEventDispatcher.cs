// <copyright file="DependencyInjectionEventDispatcher.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;

using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a dependency injection-based event dispatcher for integration events.
/// </summary>
/// <remarks>
/// This class uses dependency injection to resolve and invoke appropriate event handlers
/// for given integration events.
/// </remarks>
public partial class DependencyInjectionEventDispatcher : IIntegrationEventDispatcher
{
    private readonly ILogger<DependencyInjectionEventDispatcher> _logger;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyInjectionEventDispatcher"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve event handlers.</param>
    /// <param name="logger">The logger used for logging dispatch operations.</param>
    /// <exception cref="ArgumentNullException">Thrown if serviceProvider or logger is null.</exception>
    public DependencyInjectionEventDispatcher(IServiceProvider serviceProvider, ILogger<DependencyInjectionEventDispatcher> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc/>
    /// <summary>
    /// Applies the given event by dispatching it to all registered handlers.
    /// </summary>
    /// <param name="baseEvent">The event to be dispatched.</param>
    /// <param name="metadata">Metadata associated with the event.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of results from all handlers.</returns>
    /// <exception cref="ArgumentNullException">Thrown if baseEvent is null.</exception>
    public async Task<IEnumerable<IEnumerable<object>>> ApplyAsync([NotNull] object baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        LogDispatchingEvent(metadata.Message.Name, metadata.PartitionKey);
        return await Task.WhenAll(
            GetHandlers(baseEvent)
                .Select(p => p.ApplyAsync(baseEvent, metadata, cancellationToken))).ConfigureAwait(false);
    }

    /// <summary>
    /// Logs the dispatching of an event.
    /// </summary>
    /// <param name="eventName">The name of the event being dispatched.</param>
    /// <param name="partitionKey">The partition key associated with the event.</param>
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Dispatching event with name '{EventName}', partition key '{PartitionKey}'.")]
    public partial void LogDispatchingEvent(string? eventName, string? partitionKey);

    /// <summary>
    /// Gets the event handlers for a specific event type.
    /// </summary>
    /// <param name="event">The event for which to retrieve handlers.</param>
    /// <returns>A list of event handlers that can handle the given event type.</returns>
    private List<IIntegrationEventHandler> GetHandlers(object @event)
    {
        Type eventType = @event.GetType();
        Type eventHandlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
        Type eventHandlerTypeList = typeof(IEnumerable<>).MakeGenericType(eventHandlerType);
        return (_serviceProvider.GetService(eventHandlerTypeList) as IEnumerable ?? Array.Empty<IIntegrationEventHandler>())
            .Cast<IIntegrationEventHandler>()
            .ToList();
    }
}
