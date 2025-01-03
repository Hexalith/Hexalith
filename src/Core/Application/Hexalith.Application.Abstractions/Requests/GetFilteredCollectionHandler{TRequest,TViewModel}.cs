// <copyright file="GetFilteredCollectionHandler{TRequest,TViewModel}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.Application.Services;

/// <summary>
/// Handler for getting filtered collection with pagination support.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
public class GetFilteredCollectionHandler<TRequest, TViewModel> : RequestHandlerBase<TRequest>
    where TRequest : class, IFilteredChunkableRequest
    where TViewModel : class, IIdDescription
{
    private readonly IIdCollectionFactory _collectionFactory;
    private readonly IProjectionFactory<TViewModel> _projectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFilteredCollectionHandler{TRequest, TViewModel}"/> class.
    /// </summary>
    /// <param name="collectionFactory">The collection factory.</param>
    /// <param name="projectionFactory">The projection factory.</param>
    public GetFilteredCollectionHandler(IIdCollectionFactory collectionFactory, IProjectionFactory<TViewModel> projectionFactory)
    {
        ArgumentNullException.ThrowIfNull(collectionFactory);
        ArgumentNullException.ThrowIfNull(projectionFactory);
        _collectionFactory = collectionFactory;
        _projectionFactory = projectionFactory;
    }

    /// <summary>
    /// Executes the request asynchronously.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The request with results.</returns>
    public override async Task<TRequest> ExecuteAsync(TRequest request, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(metadata);

        if (request.Ids.Any())
        {
            return (TRequest)request.CreateResults(
                await GetChunkAsync(request.Ids, cancellationToken)
                .ConfigureAwait(false));
        }

        IIdCollectionService service = _collectionFactory.CreateService(
            IIdCollectionFactory.GetAggregateCollectionName(metadata.Message.Aggregate.Name),
            metadata.Context.PartitionId);
        if (request.Filter is null)
        {
            return (TRequest)request.CreateResults(
                await GetChunkAsync(
                    request.Skip,
                    request.Take,
                    service,
                    cancellationToken)
                .ConfigureAwait(false));
        }

        // Search with filter
        List<TViewModel> chunkResults;
        List<TViewModel> searchResults = [];
        int chunkTake = 100;
        int chunkSkip = 0;
        while (true)
        {
            chunkResults = [..await GetChunkAsync(
                    chunkSkip,
                    chunkTake,
                    service,
                    cancellationToken)
                .ConfigureAwait(false)];
            chunkSkip += chunkTake;
            searchResults.AddRange(chunkResults.Where(
                p => p.Description.Contains(request.Filter, StringComparison.OrdinalIgnoreCase) ||
                p.Id.Contains(request.Filter, StringComparison.OrdinalIgnoreCase)));
            if (searchResults.Count >= request.Skip + request.Take || chunkResults.Count < chunkTake)
            {
                break;
            }
        }

        if (request.Skip > 0)
        {
            searchResults = [.. searchResults.Skip(request.Skip)];
        }

        if (request.Take > 0)
        {
            searchResults = [.. searchResults.Take(request.Take)];
        }

        return (TRequest)request.CreateResults(searchResults);
    }

    private async Task<IEnumerable<TViewModel>> GetChunkAsync(int skip, int take, IIdCollectionService service, CancellationToken cancellationToken)
    {
        List<string> ids = [.. await service
                .GetAsync(skip, take, cancellationToken)
                .ConfigureAwait(false)];
        IEnumerable<TViewModel> queryResult = await GetChunkAsync(ids, cancellationToken).ConfigureAwait(false);
        if (skip > 0)
        {
            queryResult = queryResult.Skip(skip);
        }

        if (take > 0)
        {
            queryResult = queryResult.Take(take);
        }

        return queryResult;
    }

    private async Task<IEnumerable<TViewModel>> GetChunkAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
    {
        // Retrieve states (ViewModel) in parallel
        List<Task<TViewModel?>> summaryTasks = [.. ids
                .Select(id => _projectionFactory.GetStateAsync(id, cancellationToken))];

        TViewModel?[] results = await Task.WhenAll(summaryTasks).ConfigureAwait(false);

        // Filter and process results
        return results
            .Where(p => p != null)
            .Select(p => p!)
            .OrderBy(p => p.Id);
    }
}