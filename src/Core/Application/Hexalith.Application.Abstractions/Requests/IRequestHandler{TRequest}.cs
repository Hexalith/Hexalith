// <copyright file="IRequestHandler{TRequest}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;

/// <summary>
/// Defines a handler for processing requests of type <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
public interface IRequestHandler<TRequest> : IRequestHandler
    where TRequest : class
{
    /// <summary>
    /// Executes the specified request asynchronously.
    /// </summary>
    /// <param name="baseRequest">The request to be processed.</param>
    /// <param name="metadata">The metadata associated with the request.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the processed request.</returns>
    Task<TRequest> ExecuteAsync(TRequest baseRequest, Metadata metadata, CancellationToken cancellationToken);
}