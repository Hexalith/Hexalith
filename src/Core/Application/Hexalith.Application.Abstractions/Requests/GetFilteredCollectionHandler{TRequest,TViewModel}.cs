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
/// Handler for getting file type summaries.
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

        List<TViewModel> queryResult = [];
        int skip = 0;
        bool filter = !string.IsNullOrWhiteSpace(request.Filter);

        // Decide how many items to fetch in each loop
        // If there's a filter, fetch up to 100 items at a time;
        // otherwise, fetch the min between request.Take and 100
        int take = filter ? 100 : Math.Min(request.Take, 100);

        // While there are more ids to process
        // Loop until no more IDs or we've reached the request.Take limit
        while (true)
        {
            // Retrieve next chunk of IDs
            List<string> ids = (await service
                .GetAsync(skip, take, cancellationToken)
                .ConfigureAwait(false))
                .ToList();

            if (ids.Count == 0)
            {
                // No more IDs to process
                break;
            }

            // Update skip for next iteration
            skip += take;

            // Retrieve states (ViewModel) in parallel
            List<Task<TViewModel?>> summaryTasks = [];
            foreach (string id in ids)
            {
                summaryTasks.Add(_projectionFactory.GetStateAsync(id, cancellationToken));
            }

            TViewModel?[] results = await Task.WhenAll(summaryTasks).ConfigureAwait(false);

            // Filter out null items
            IEnumerable<TViewModel> chunk = results
                .Where(p => p != null)
                .Select(p => p!);

            // Apply filter logic
            if (filter)
            {
                chunk = chunk.Where(p =>
                    p.Description.Contains(request?.Filter!, StringComparison.OrdinalIgnoreCase) ||
                    p.Id.Contains(request?.Filter!, StringComparison.OrdinalIgnoreCase));
            }

            // Merge the chunk into the main query results
            queryResult = queryResult
                .Union(chunk)
                .Take(request.Take)
                .ToList();

            // If we already have enough items to match request.Take, we stop
            if (queryResult.Count >= request.Take)
            {
                break;
            }
        }

        return (TRequest)request.CreateResults(queryResult.OrderBy(p => p.Id).ToList());
    }
}