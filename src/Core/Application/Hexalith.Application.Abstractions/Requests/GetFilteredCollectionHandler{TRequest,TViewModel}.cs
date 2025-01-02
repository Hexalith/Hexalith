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

        IIdCollectionService service = _collectionFactory.CreateService(
            IIdCollectionFactory.GetAggregateCollectionName(metadata.Message.Aggregate.Name),
            metadata.Context.PartitionId);
        List<TViewModel> finalResults;
        if (request.Filter is null)
        {
            finalResults = await GetChunkAsync(request, service, cancellationToken).ConfigureAwait(false);
        }

        return (TRequest)request.CreateResults(finalResults);
    }

    private async Task<List<TViewModel>> GetChunkAsync(TRequest request, IIdCollectionService service, CancellationToken cancellationToken)
    {
        List<TViewModel> queryResult = [];
        bool hasFilter = !string.IsNullOrWhiteSpace(request.Filter);

        // Calculate total items to fetch to ensure we have enough after filtering
        int totalToFetch = request.Skip + (request.Take > 0 ? request.Take : int.MaxValue);

        int currentSkip = 0;
        int remainingToFetch = totalToFetch;

        while (remainingToFetch > 0)
        {
            List<string> ids = [.. await service
                .GetAsync(currentSkip, hasFilter ? 100 : totalToFetch, cancellationToken)
                .ConfigureAwait(false)];

            if (ids.Count <= 0)
            {
                break;
            }

            // Retrieve states (ViewModel) in parallel
            List<Task<TViewModel?>> summaryTasks = [.. ids.Select(id => _projectionFactory.GetStateAsync(id, cancellationToken))];

            TViewModel?[] results = await Task.WhenAll(summaryTasks).ConfigureAwait(false);

            // Filter and process results
            IEnumerable<TViewModel> chunk = results
                .Where(p => p != null)
                .Select(p => p!);

            // Apply filter if specified
            if (hasFilter)
            {
                chunk = chunk.Where(p =>
                    p.Description.Contains(request?.Filter!, StringComparison.OrdinalIgnoreCase) ||
                    p.Id.Contains(request?.Filter!, StringComparison.OrdinalIgnoreCase));
            }

            // Add to results
            queryResult.AddRange(chunk);

            // Update loop variables
            currentSkip += chunkSize;
            remainingToFetch -= chunkSize;

            // If we have enough results after filtering, stop fetching
            if (hasFilter && queryResult.Count >= (request.Skip + request.Take))
            {
                break;
            }
        }

        queryResult = (List<TViewModel>)queryResult
            .OrderBy(p => p.Id);
        if (request.Skip > 0)
        {
            queryResult = (List<TViewModel>)queryResult
                .Skip(request.Skip);
        }

        if (request.Take > 0)
        {
            queryResult = (List<TViewModel>)queryResult
                .Take(request.Take);
        }

        // Apply final skip and take
        List<TViewModel> finalResults = queryResult.ToList();
        return finalResults;
    }
}