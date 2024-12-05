// <copyright file="IPartitionActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Partitions.Actors;

using Dapr.Actors;

using Hexalith.Application.Partitions.Models;

/// <summary>
/// Represents an actor interface for managing partitions in a distributed system.
/// This actor handles partition lifecycle operations including creation, enabling/disabling,
/// retrieval, and renaming of partitions.
/// </summary>
public interface IPartitionActor : IActor
{
    /// <summary>
    /// Gets the name of the partition collection actor.
    /// This name is used to identify the collection of all partition actors in the system.
    /// </summary>
    internal static string ActorCollectionName => "Partitions";

    /// <summary>
    /// Gets the name of the partition actor.
    /// This name is used to identify individual partition actor instances.
    /// </summary>
    internal static string ActorName => "Partition";

    /// <summary>
    /// Disables the current partition asynchronously.
    /// A disabled partition remains in the system but is marked as inactive.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task DisableAsync();

    /// <summary>
    /// Enables the current partition asynchronously.
    /// An enabled partition is marked as active and can be used in the system.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task EnableAsync();

    /// <summary>
    /// Determines whether the current partition is enabled asynchronously.
    /// </summary>
    /// <returns>true if the partition is enabled; otherwise, false.</returns>
    Task<bool> EnabledAsync();

    /// <summary>
    /// Retrieves the current partition's information asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation.
    /// The task result contains the <see cref="Partition"/> with its current state and properties.</returns>
    Task<Partition> GetAsync();

    /// <summary>
    /// Renames the current partition asynchronously.
    /// </summary>
    /// <param name="newName">The new name to assign to the partition. Must be unique within the system.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task RenameAsync(string newName);

    /// <summary>
    /// Adds a new partition with the specified name asynchronously.
    /// </summary>
    /// <param name="partition">The partition to add.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task SetAsync(Partition partition);
}