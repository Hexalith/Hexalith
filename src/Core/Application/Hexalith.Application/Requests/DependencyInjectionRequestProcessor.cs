// <copyright file="DependencyInjectionRequestProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Processes requests using dependency injection to resolve the appropriate handler.
/// </summary>
public class DependencyInjectionRequestProcessor : IRequestProcessor
{
    private readonly IServiceProvider _services;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyInjectionRequestProcessor"/> class.
    /// </summary>
    /// <param name="services">The service provider used to resolve request handlers.</param>
    public DependencyInjectionRequestProcessor(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        _services = services;
    }

    /// <summary>
    /// Processes the specified request asynchronously.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="metadata">The metadata associated with the request.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the processed request.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request or metadata is null.</exception>
    public async Task<object> ProcessAsync(object request, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(metadata);
        Type type = typeof(IRequestHandler<>).MakeGenericType(request.GetType());
        IRequestHandler handler = _services.GetRequiredService(type) as IRequestHandler
            ?? throw new InvalidOperationException($"No request handler found for request of type '{request.GetType().Name}'.");
        return await handler.ExecuteAsync(request, metadata, cancellationToken);
    }
}