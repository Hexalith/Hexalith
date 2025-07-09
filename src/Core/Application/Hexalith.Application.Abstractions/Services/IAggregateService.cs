// <copyright file="IAggregateService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Services;

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Hexalith.Domain.Events;
using Hexalith.Domains;

/// <summary>
/// Interface for aggregate services.
/// </summary>
public interface IAggregateService
{
    /// <summary>
    /// Finds the aggregate asynchronously.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
    /// <param name="aggregate">The aggregate instance.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the aggregate, or null if not found.</returns>
    async Task<TAggregate?> FindAsync<TAggregate>([NotNull] TAggregate aggregate, [NotNull] string partitionId, CancellationToken cancellationToken)
        where TAggregate : class, IDomainAggregate
    {
        ArgumentNullException.ThrowIfNull(aggregate);
        ArgumentException.ThrowIfNullOrWhiteSpace(partitionId);
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregate.AggregateId);
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregate.AggregateName);
        SnapshotEvent? snapshot = await GetSnapshotAsync(aggregate, partitionId, cancellationToken);
        return snapshot?.GetAggregate<TAggregate>();
    }

    /// <summary>
    /// Gets the aggregate asynchronously.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
    /// <param name="aggregate">The aggregate instance.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the aggregate.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the aggregate is not found.</exception>
    async Task<TAggregate> GetAsync<TAggregate>([NotNull] TAggregate aggregate, [NotNull] string partitionId, CancellationToken cancellationToken)
        where TAggregate : class, IDomainAggregate
    {
        return (await FindAsync(aggregate, partitionId, cancellationToken))
            ?? throw new InvalidOperationException($"The aggregate with name '{aggregate.AggregateName}' and ID '{aggregate.AggregateId}' was not found in partition '{partitionId}'.");
    }

    /// <summary>
    /// Gets the snapshot asynchronously.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the snapshot event.</returns>
    Task<SnapshotEvent?> GetSnapshotAsync(string aggregateName, string partitionId, string id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the snapshot asynchronously.
    /// </summary>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the snapshot event.</returns>
    async Task<SnapshotEvent?> GetSnapshotAsync(IDomainAggregate aggregate, string partitionId, CancellationToken cancellationToken)
        => await GetSnapshotAsync(aggregate.AggregateName, partitionId, aggregate.AggregateId, cancellationToken);

    /// <summary>
    /// Gets the snapshot asynchronously.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <param name="globalId">The global identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the snapshot event.</returns>
    Task<SnapshotEvent?> GetSnapshotAsync(string aggregateName, string globalId, CancellationToken cancellationToken);
}