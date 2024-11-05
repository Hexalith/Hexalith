// <copyright file="IKeyHashActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using Dapr.Actors;

/// <summary>
/// Represents an actor interface for managing a list of string values in a distributed actor system.
/// This actor provides functionality for adding, removing, checking existence, and retrieving all values
/// in a thread-safe and distributed manner.
/// </summary>
/// <seealso cref="IActor" />
public interface IKeyHashActor : IActor
{
    /// <summary>
    /// Adds a new value to the list asynchronously. If the value already exists,
    /// it will not create a duplicate entry.
    /// </summary>
    /// <param name="value">The string value to add to the list. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains
    /// the total count of items in the list after the addition.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the value parameter is null.</exception>
    Task<int> AddAsync(string value);

    /// <summary>
    /// Retrieves all values from the list asynchronously. The values are returned
    /// in the order they were added.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains
    /// an enumerable collection of strings representing all values currently stored in the list.
    /// Returns an empty collection if no values exist.</returns>
    Task<IEnumerable<string>> AllAsync();

    /// <summary>
    /// Checks if a specific value exists in the list asynchronously.
    /// The comparison is case-sensitive.
    /// </summary>
    /// <param name="value">The string value to check for existence in the list. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains
    /// true if the value exists in the list; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the value parameter is null.</exception>
    Task<bool> ExistsAsync(string value);

    /// <summary>
    /// Removes a specific value from the list asynchronously. If the value doesn't exist,
    /// the operation completes successfully without any effect.
    /// </summary>
    /// <param name="value">The string value to remove from the list. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the value parameter is null.</exception>
    Task RemoveAsync(string value);
}