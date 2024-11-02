// <copyright file="IKeyListActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using Dapr.Actors;

/// <summary>
/// Interface IKeyListActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
public interface IKeyListActor : IActor
{
    /// <summary>
    /// Adds a new value to the list asynchronously.
    /// </summary>
    /// <param name="value">The value to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(string value);

    /// <summary>
    /// Checks if a value exists in the list asynchronously.
    /// </summary>
    /// <param name="value">The value to check for existence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the value exists.</returns>
    Task<bool> ExistsAsync(string value);

    /// <summary>
    /// Gets all values from the list asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of strings representing the values in the list.</returns>
    Task<IEnumerable<string>> GetAsync();

    /// <summary>
    /// Removes a value from the list asynchronously.
    /// </summary>
    /// <param name="value">The value to remove.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveAsync(string value);
}