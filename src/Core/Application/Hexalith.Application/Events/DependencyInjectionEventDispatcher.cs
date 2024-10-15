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

using Hexalith.Application.MessageMetadatas;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class DependencyInjectionEventDispatcher.
/// Implements the <see cref="IIntegrationEventDispatcher" />.
/// </summary>
/// <seealso cref="IIntegrationEventDispatcher" />
public partial class DependencyInjectionEventDispatcher : IIntegrationEventDispatcher
{
    private readonly ILogger<DependencyInjectionEventDispatcher> _logger;

    /// <summary>
    /// The service provider.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyInjectionEventDispatcher" /> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="logger">The logger.</param>
    public DependencyInjectionEventDispatcher(IServiceProvider serviceProvider, ILogger<DependencyInjectionEventDispatcher> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<IEnumerable<object>>> ApplyAsync([NotNull] object baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        DispatchingEvent(metadata.Message.Name, metadata.Message.Aggregate.Name, metadata.Message.Aggregate.Id);
        return await Task.WhenAll(
            GetHandlers(baseEvent)
                .Select(p => p.ApplyAsync(baseEvent, metadata, cancellationToken))).ConfigureAwait(false);
    }

    [LoggerMessage(
        EventId = 1,
    Level = LogLevel.Debug,
    Message = "Dispatching event with name '{EventName}', identifier '{AggregateId}' on AggregateName '{AggregateName}'.")]
    public partial void DispatchingEvent(string? eventName, string? aggregateName, string? aggregateId);

    /// <summary>
    /// Gets the event handler of type IEventHandler.<MyEvent>.
    /// </summary>
    /// <param name="event">The event to handle.</param>
    /// <returns>IEventHandler list.</returns>
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