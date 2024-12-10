// <copyright file="RequestHandlerBase{TRequest}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;

/// <summary>
/// Base class for handling requests of type <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
public abstract class RequestHandlerBase<TRequest> : IRequestHandler<TRequest>
    where TRequest : class
{
    /// <summary>
    /// Executes the request asynchronously.
    /// </summary>
    /// <param name="baseRequest">The request to execute.</param>
    /// <param name="metadata">The metadata associated with the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the request.</returns>
    public abstract Task<TRequest> ExecuteAsync(TRequest baseRequest, Metadata metadata, CancellationToken cancellationToken);

    /// <inheritdoc/>
    async Task<object> IRequestHandler.ExecuteAsync(object baseRequest, Metadata metadata, CancellationToken cancellationToken)
        => await ExecuteAsync(ToRequest(baseRequest), metadata, cancellationToken);

    /// <summary>
    /// Converts the given object to the expected request type.
    /// </summary>
    /// <param name="request">The request object to convert.</param>
    /// <returns>The request object cast to the expected type.</returns>
    /// <exception cref="ArgumentException">Thrown when the request is not of the expected type.</exception>
    private static TRequest ToRequest(object request)
    {
        return request is TRequest c
            ? c
            : throw new ArgumentException($"{request.GetType().Name} is an invalid request type. Expected: {typeof(TRequest).Name}.", nameof(request));
    }
}