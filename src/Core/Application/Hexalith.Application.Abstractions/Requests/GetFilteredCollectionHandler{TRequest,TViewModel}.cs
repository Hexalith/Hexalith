﻿// <copyright file="GetFilteredCollectionHandler{TRequest,TViewModel}.cs" company="ITANEO">
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

using Microsoft.Extensions.Logging;

/// <summary>
/// Handler for getting filtered collection with pagination support.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
public partial class GetFilteredCollectionHandler<TRequest, TViewModel> : RequestHandlerBase<TRequest>
    where TRequest : class, IFilteredChunkableRequest
    where TViewModel : class, IIdDescription
{
    private readonly IIdCollectionFactory _collectionFactory;
    private readonly ILogger<GetFilteredCollectionHandler<TRequest, TViewModel>> _logger;
    private readonly IProjectionFactory<TViewModel> _projectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFilteredCollectionHandler{TRequest, TViewModel}"/> class.
    /// </summary>
    /// <param name="collectionFactory">The collection factory.</param>
    /// <param name="projectionFactory">The projection factory.</param>
    /// <param name="logger">The logger.</param>
    public GetFilteredCollectionHandler(
        IIdCollectionFactory collectionFactory,
        IProjectionFactory<TViewModel> projectionFactory,
        ILogger<GetFilteredCollectionHandler<TRequest, TViewModel>> logger)
    {
        ArgumentNullException.ThrowIfNull(collectionFactory);
        ArgumentNullException.ThrowIfNull(projectionFactory);
        ArgumentNullException.ThrowIfNull(logger);
        _collectionFactory = collectionFactory;
        _projectionFactory = projectionFactory;
        _logger = logger;
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
            IEnumerable<string> globalIds = request.Ids.Select(metadata.CreateAggregateGlobalId);
            IDictionary<string, TViewModel> r = await GetIdsProjectionsAsync(globalIds, cancellationToken);
            List<TViewModel> results = [.. r.Values];

            // If the request result does not contain all the requested ids, we need to log an error
            if (results.Count != request.Ids.Count())
            {
                LogMissingResults(
                    metadata.Message.Name,
                    metadata.Message.Id,
                    metadata.Context.CorrelationId,
                    request.Ids.Except(r.Keys));
            }

            return (TRequest)request.CreateResults(results);
        }

        IIdCollectionService service = _collectionFactory.CreateService(
                IIdCollectionFactory.GetAggregateCollectionName(metadata.Message.Aggregate.Name),
                metadata.Context.PartitionId);
        if (request.Filter is null)
        {
            return (TRequest)request.CreateResults(
                await GetProjectionChunkAsync(
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
            chunkResults = [..await GetProjectionChunkAsync(
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

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Missing results for aggregate identifiers in request {MessageName} : {AggregateIds}. MessageId={MessageId}; CorrelationId={CorrelationId}")]
    public partial void LogMissingResults(string messageName, string messageId, string correlationId, IEnumerable<string> aggregateIds);

    private async Task<IDictionary<string, TViewModel>> GetIdsProjectionsAsync(IEnumerable<string> ids, CancellationToken cancellationToken)
    {
        // Retrieve states (ViewModel) in parallel
        List<Task<TViewModel?>> summaryTasks = [.. ids
                .Select(id => _projectionFactory.GetStateAsync(id, cancellationToken))];

        TViewModel?[] results = await Task.WhenAll(summaryTasks).ConfigureAwait(false);

        // Filter and process results
        return results
            .Where(p => p != null)
            .Select(p => p!)
            .ToDictionary(k => k.Id, v => v);
    }

    private async Task<IEnumerable<TViewModel>> GetProjectionChunkAsync(int skip, int take, IIdCollectionService service, CancellationToken cancellationToken)
    {
        List<string> ids = [.. await service
                .GetAsync(skip, take, cancellationToken)
                .ConfigureAwait(false)];
        IEnumerable<TViewModel> queryResult = (await GetIdsProjectionsAsync(ids, cancellationToken).ConfigureAwait(false)).Select(p => p.Value);
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
}