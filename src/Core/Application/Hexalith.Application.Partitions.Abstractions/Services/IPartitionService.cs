// <copyright file="IPartitionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Partitions.Services;

using Hexalith.Application.Partitions.Models;

/// <summary>
/// Defines a service for retrieving partition information.
/// </summary>
public interface IPartitionService
{
    /// <summary>
    /// Gets the default partition asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the default partition.</returns>
    Task<string> DefaultAsync(CancellationToken cancellationToken) => Task.FromResult("Default");

    /// <summary>
    /// Gets all partitions asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of partitions.</returns>
    IAsyncEnumerable<Partition> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets all partition IDs asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of partition IDs.</returns>
    Task<IEnumerable<string>> GetAllIdsAsync(CancellationToken cancellationToken);
}