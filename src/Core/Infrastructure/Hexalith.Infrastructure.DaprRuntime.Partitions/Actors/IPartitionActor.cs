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
    /// Gets the name of the partition collection actor.
    /// </summary>
    public static string ActorCollectionName => "Partitions";

    /// <summary>
    /// Gets the name of the partition actor.
    /// </summary>
    public static string ActorName => "Partition";

    /// <summary>
    /// Adds a new partition with the specified identifier and name.
    /// </summary>
    /// <param name="name">The name of the new partition.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(string name);

    /// <summary>
    /// Disables the partition with the specified identifier.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DisableAsync();

    /// <summary>
    /// Enables the partition with the specified identifier.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task EnableAsync();

    /// <summary>
    /// Gets the partition with the specified identifier.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the partition.</returns>
    Task<Partition> GetAsync();

    /// <summary>
    /// Renames the partition with the specified identifier.
    /// </summary>
    /// <param name="newName">The new name of the partition.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RenameAsync(string newName);
}