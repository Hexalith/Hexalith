// <copyright file="IOneToManyAggregateRelationService{TLeft,TRight}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Services;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface for managing one-to-many aggregate relations.
/// </summary>
/// <typeparam name="TLeft">The type of the left aggregate service.</typeparam>
/// <typeparam name="TRight">The type of the right aggregate service.</typeparam>
public interface IOneToManyAggregateRelationService<TLeft, TRight>
    where TLeft : IDomainAggregate
    where TRight : IDomainAggregate
{
    /// <summary>
    /// Adds a relation between two aggregates.
    /// </summary>
    /// <param name="partitionId">The partition ID.</param>
    /// <param name="aggregateId">The ID of the left aggregate.</param>
    /// <param name="linkedAggregateId">The ID of the right aggregate to be linked.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(string partitionId, string aggregateId, string linkedAggregateId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the related aggregates for a given aggregate.
    /// </summary>
    /// <param name="partitionId">The partition ID.</param>
    /// <param name="aggregateId">The ID of the left aggregate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of related aggregate global IDs.</returns>
    Task<IEnumerable<string>> GetAsync(string partitionId, string aggregateId, CancellationToken cancellationToken);
}