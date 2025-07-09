// <copyright file="GetExportsRequestHandler.cs" company="ITANEO">
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
using Hexalith.Application.Services;
using Hexalith.Domain.Events;
using Hexalith.Domains;

/// <summary>
/// Handles export requests for a specific type of request, export model, and aggregate.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
public class GetExportsRequestHandler<TRequest, TAggregate> : RequestHandlerBase<TRequest>
    where TRequest : class, IChunkableRequest
    where TAggregate : class, IDomainAggregate
{
    private readonly IAggregateService _aggregateService;
    private readonly IIdCollectionFactory _collectionFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetExportsRequestHandler{TRequest, TAggregate}"/> class.
    /// </summary>
    /// <param name="collectionFactory">The collection factory.</param>
    /// <param name="aggregateService">The aggregate service.</param>
    /// <exception cref="ArgumentNullException">Thrown when collectionFactory or aggregateService is null.</exception>
    public GetExportsRequestHandler(IIdCollectionFactory collectionFactory, IAggregateService aggregateService)
    {
        ArgumentNullException.ThrowIfNull(collectionFactory);
        ArgumentNullException.ThrowIfNull(aggregateService);
        _collectionFactory = collectionFactory;
        _aggregateService = aggregateService;
    }

    /// <summary>
    /// Executes the export request asynchronously.
    /// </summary>
    /// <param name="request">The request to execute.</param>
    /// <param name="metadata">The metadata associated with the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated request with the results.</returns>
    /// <exception cref="ArgumentNullException">Thrown when request or metadata is null.</exception>
    public override async Task<TRequest> ExecuteAsync(TRequest request, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(metadata);

        IIdCollectionService service = _collectionFactory.CreateService(
            IIdCollectionFactory.GetAggregateCollectionName(metadata.Message.Aggregate.Name),
            metadata.Context.PartitionId);

        IEnumerable<string> ids = await service
                .GetAsync(request.Skip, request.Take, cancellationToken)
                .ConfigureAwait(false);

        List<Task<SnapshotEvent?>> summaryTasks = [];

        foreach (string id in ids)
        {
            summaryTasks.Add(_aggregateService.GetSnapshotAsync(metadata.Message.Aggregate.Name, id, cancellationToken));
        }

        SnapshotEvent?[] results = await Task.WhenAll(summaryTasks).ConfigureAwait(false);

        IEnumerable<TAggregate> queryResult = results
            .Where(p => p is not null)
            .Select(p => p!.GetAggregate<TAggregate>());

        return (TRequest)request.CreateResults(queryResult);
    }
}