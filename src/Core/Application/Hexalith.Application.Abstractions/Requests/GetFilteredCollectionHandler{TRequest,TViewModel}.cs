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
using Hexalith.Domain.ValueObjects;

using Microsoft.Extensions.Logging;

/// <summary>
/// Handler for getting filtered collection with pagination support.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
public partial class GetFilteredCollectionHandler<TRequest, TViewModel> : RequestHandlerBase<TRequest>
    where TRequest : class, ISearchChunkableRequest
    where TViewModel : class, IIdDescription
{
    private readonly IIdCollectionFactory _collectionFactory;
    private readonly ILogger _logger;
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
    /// Logs an error message indicating missing results for aggregate identifiers in a request.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="messageName">The name of the message.</param>
    /// <param name="messageId">The ID of the message.</param>
    /// <param name="correlationId">The correlation ID of the context.</param>
    /// <param name="aggregateIds">The aggregate identifiers that are missing results.</param>
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Error,
        Message = "Missing results for aggregate identifiers in request {MessageName} : {AggregateIds}. MessageId={MessageId}; CorrelationId={CorrelationId}")]
    public static partial void LogMissingResults(ILogger logger, string messageName, string messageId, string correlationId, IEnumerable<string> aggregateIds);

    /// <summary>
    /// Logs a warning message indicating that the request contains both ids and a filter or search.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="messageName">The name of the message.</param>
    /// <param name="messageId">The ID of the message.</param>
    /// <param name="correlationId">The correlation ID of the context.</param>
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Warning,
        Message = "Request contains both ids and a filter or search in request {MessageName}. MessageId={MessageId}; CorrelationId={CorrelationId}")]
    public static partial void LogWarningForIdsWithFilterOrSearch(ILogger logger, string messageName, string messageId, string correlationId);

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
        IFilteredRequest? filtered = request as IFilteredRequest;
        if (request.Ids.Any())
        {
            if (!string.IsNullOrWhiteSpace(request.Search) || filtered?.Filter is not null)
            {
                LogWarningForIdsWithFilterOrSearch(_logger, metadata.Message.Name, metadata.Message.Id, metadata.Context.CorrelationId);
            }

            return await GetFromIdsAsync(request, metadata, cancellationToken);
        }

        IIdCollectionService service = _collectionFactory.CreateService(
                IIdCollectionFactory.GetAggregateCollectionName(metadata.Message.Aggregate.Name),
                metadata.Context.PartitionId);
        if (string.IsNullOrEmpty(request.Search) && filtered?.Filter is null)
        {
            return (TRequest)request.CreateResults(
                await GetProjectionChunkAsync(
                    request.Skip,
                    request.Take,
                    service,
                    cancellationToken)
                .ConfigureAwait(false));
        }

        // Search with search
        List<TViewModel> chunkResults;
        List<TViewModel> searchResults = [];
        const int chunkTake = 100;
        int chunkSkip = 0;

        // Split and normalize words
        string[] splittedWords = request.Search?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];
        string[] normalizedWords = [.. splittedWords.Select(w => w.Normalize(System.Text.NormalizationForm.FormD))];

        while (true)
        {
            chunkResults = [..await GetProjectionChunkAsync(
                    chunkSkip,
                    chunkTake,
                    service,
                    cancellationToken)
                .ConfigureAwait(false)];
            chunkSkip += chunkTake;
            foreach (TViewModel chunkResult in chunkResults)
            {
                if (await CompliesWithFilterAndSearchAsync(normalizedWords, chunkResult, filtered, cancellationToken))
                {
                    searchResults.Add(chunkResult);
                }
            }

            // Break if enough results have been found or fewer items remain
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

    private static async Task<bool> CompliesWithFilterAndSearchAsync(string[] normalizedWords, TViewModel p, IFilteredRequest? filtered, CancellationToken cancellationToken)
    {
        // Filter projections using pre-normalized words:
        string normalizedDescription = p.Description.Normalize(System.Text.NormalizationForm.FormD);
        string normalizedId = p.Id.Normalize(System.Text.NormalizationForm.FormD);
        bool compliesToSearch = normalizedWords.All(
                nw => normalizedDescription.Contains(nw, StringComparison.OrdinalIgnoreCase)
                        || normalizedId.Contains(nw, StringComparison.OrdinalIgnoreCase));
        bool compliesToFilter = filtered?.Filter is null || await filtered.Filter.CompliesToFilterAsync(p, cancellationToken);
        return compliesToSearch && compliesToFilter;
    }

    private async Task<TRequest> GetFromIdsAsync(TRequest request, Metadata metadata, CancellationToken cancellationToken)
    {
        IEnumerable<string> globalIds = request.Ids.Select(metadata.CreateAggregateGlobalId);
        IDictionary<string, TViewModel> r = await GetIdsProjectionsAsync(globalIds, cancellationToken);
        List<TViewModel> results = [.. r.Values];

        // If the request result does not contain all the requested ids, we need to log an error
        if (results.Count != request.Ids.Count())
        {
            LogMissingResults(
                _logger,
                metadata.Message.Name,
                metadata.Message.Id,
                metadata.Context.CorrelationId,
                request.Ids.Except(r.Keys));
        }

        return (TRequest)request.CreateResults(results);
    }

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