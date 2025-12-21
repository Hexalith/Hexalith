// <copyright file="DependencyInjectionProjectionUpdateProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Projections;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Events;
using Hexalith.Commons.Metadatas;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class DependencyInjectionEventDispatcher.
/// Implements the <see cref="IIntegrationEventDispatcher" />.
/// </summary>
/// <seealso cref="IIntegrationEventDispatcher" />
public class DependencyInjectionProjectionUpdateProcessor : IProjectionUpdateProcessor
{
    /// <summary>
    /// The service provider.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyInjectionProjectionUpdateProcessor"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="logger">The logger.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public DependencyInjectionProjectionUpdateProcessor(IServiceProvider serviceProvider, ILogger<DependencyInjectionProjectionUpdateProcessor> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);
        _serviceProvider = serviceProvider;
        Logger = logger;
    }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    protected ILogger Logger { get; }

    /// <inheritdoc/>
    public async Task ApplyAsync([NotNull] object ev, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(ev);
        await Task.WhenAll(
            GetHandlers(ev)
                    .Select(p => p.ApplyAsync(ev, metadata, cancellationToken)))
            .ConfigureAwait(false);
    }

    private List<IProjectionUpdateHandler> GetHandlers(object @event)
    {
        Type eventType = @event.GetType();
        Type eventHandlerType = typeof(IProjectionUpdateHandler<>).MakeGenericType(eventType);
        Type eventHandlerTypeList = typeof(IEnumerable<>).MakeGenericType(eventHandlerType);
        return [.. (_serviceProvider
            .GetService(eventHandlerTypeList) as IEnumerable ?? Array.Empty<IProjectionUpdateHandler>())
            .Cast<IProjectionUpdateHandler>()];
    }
}