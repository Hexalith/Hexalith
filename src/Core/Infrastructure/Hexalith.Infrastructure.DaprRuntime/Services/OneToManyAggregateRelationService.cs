// <copyright file="OneToManyAggregateRelationService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Services;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Client;

using Hexalith.Application.Services;
using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

/// <summary>
/// Service for managing one-to-many aggregate relations.
/// </summary>
/// <typeparam name="TLeft">The type of the left aggregate.</typeparam>
/// <typeparam name="TRight">The type of the right aggregate.</typeparam>
/// <remarks>
/// Initializes a new instance of the <see cref="OneToManyAggregateRelationService{TLeft, TRight}"/> class.
/// </remarks>
/// <param name="actorProxyFactory">The actor proxy factory.</param>
public class OneToManyAggregateRelationService<TLeft, TRight>(IActorProxyFactory actorProxyFactory) : IOneToManyAggregateRelationService<TLeft, TRight>
    where TRight : IDomainAggregate, new()
    where TLeft : IDomainAggregate, new()
{
    private readonly IActorProxyFactory _actorProxyFactory = actorProxyFactory;

    /// <inheritdoc/>
    public async Task AddAsync(string partitionId, string aggregateId, string relationAggregateId, CancellationToken cancellationToken)
        => await GetActor(partitionId, aggregateId).AddAsync(relationAggregateId);

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetAsync(string partitionId, string aggregateId, CancellationToken cancellationToken)
        => await GetActor(partitionId, aggregateId).AllAsync();

    private IKeyHashActor GetActor(string partitionId, string aggregateId)
        => _actorProxyFactory.CreateActorProxy<IKeyHashActor>(
            $"{partitionId}-{aggregateId}".ToActorId(),
            IOneToManyAggregateRelationService<TLeft, TRight>.RelationName);
}