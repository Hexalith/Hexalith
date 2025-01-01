// <copyright file="GetAggregateIdsRequestHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Services;

/// <summary>
/// Handles requests to get aggregate IDs.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
public class GetAggregateIdsRequestHandler<TRequest> : RequestHandlerBase<TRequest>
    where TRequest : class, IChunkableRequest
{
    private readonly IIdCollectionFactory _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAggregateIdsRequestHandler{TRequest}"/> class.
    /// </summary>
    /// <param name="factory">The factory to create ID collection services.</param>
    /// <exception cref="ArgumentNullException">Thrown when the factory is null.</exception>
    public GetAggregateIdsRequestHandler(IIdCollectionFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    /// <summary>
    /// Executes the request asynchronously.
    /// </summary>
    /// <param name="request">The request to execute.</param>
    /// <param name="metadata">The metadata associated with the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The executed request with results.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the request or metadata is null.</exception>
    public override async Task<TRequest> ExecuteAsync(TRequest request, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(metadata);
        IIdCollectionService service = _factory.CreateService(
            IIdCollectionFactory.GetAggregateCollectionName(metadata.Message.Aggregate.Name),
            metadata.Context.PartitionId);
        return (TRequest)request.CreateResults(await service
                .GetAsync(request.Skip, request.Take, CancellationToken.None)
                .ConfigureAwait(false));
    }
}