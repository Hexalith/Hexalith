// <copyright file="IAggregateService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Services;

using System.Threading.Tasks;

using Hexalith.Domain.Events;

/// <summary>
/// Interface for aggregate services.
/// </summary>
public interface IAggregateService
{
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
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <param name="globalId">The global identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the snapshot event.</returns>
    Task<SnapshotEvent?> GetSnapshotAsync(string aggregateName, string globalId, CancellationToken cancellationToken);
}