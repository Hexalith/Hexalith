// <copyright file="ISequentialStringListActor.cs" company="ITANEO">
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
public interface ISequentialStringListActor : IActor
{
    /// <summary>
    /// Adds a new value to the list asynchronously. If the value already exists,
    /// it will not create a duplicate entry.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <param name="value">The string value to add to the list. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value parameter is null.</exception>
    Task AddAsync(string value);

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
    /// Reads a page of string values from the list asynchronously.
    /// </summary>
    /// <param name="pageNumber">The page number to read. Must be a non-negative integer.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains
    /// an enumerable of string values for the specified page, or null if the page does not exist.</returns>
    Task<IEnumerable<string>?> ReadAsync(int pageNumber);

    /// <summary>
    /// Removes a specific value from the list asynchronously. If the value doesn't exist,
    /// the operation completes successfully without any effect.
    /// </summary>
    /// <param name="value">The string value to remove from the list. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the value parameter is null.</exception>
    Task RemoveAsync(string value);
}