// <copyright file="IPartitionActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Partitions.Actors;

using Dapr.Actors;

using Hexalith.Application.Partitions.Models;

/// <summary>
/// Interface for partition actor operations.
/// </summary>
public interface IPartitionActor : IActor
{
    /// <summary>
    /// Adds a new partition with the specified identifier and name.
    /// </summary>
    /// <param name="id">The identifier of the new partition.</param>
    /// <param name="name">The name of the new partition.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(string id, string name);

    /// <summary>
    /// Disables the partition with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the partition to disable.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DisableAsync(string id);

    /// <summary>
    /// Enables the partition with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the partition to enable.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task EnableAsync(string id);

    /// <summary>
    /// Gets the partition with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the partition to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the partition.</returns>
    Task<Partition> GetAsync(string id);

    /// <summary>
    /// Gets the identifiers of all partitions.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of partition identifiers.</returns>
    Task<IEnumerable<string>> GetIdsAsync();

    /// <summary>
    /// Renames the partition with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the partition to rename.</param>
    /// <param name="newName">The new name of the partition.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RenameAsync(string id, string newName);
}