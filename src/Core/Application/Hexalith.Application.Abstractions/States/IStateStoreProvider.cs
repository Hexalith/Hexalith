// <copyright file="IStateStoreProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.States;

using System.Threading.Tasks;

using Hexalith.Extensions.Common;

/// <summary>
/// State store provider interface.
/// </summary>
public interface IStateStoreProvider
{
    /// <summary>
    /// Adds the state asynchronous.
    /// </summary>
    /// <typeparam name="T">Type of the value to persist.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task AddStateAsync<T>(string key, T value, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the or add state asynchronous.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;T&gt;.</returns>
    Task<T> GetOrAddStateAsync<T>(string key, T value, CancellationToken cancellationToken);

    /// <summary>
    /// Gets an object from the state store.
    /// </summary>
    /// <typeparam name="T">Type of the object to get.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;T&gt;.</returns>
    Task<T> GetStateAsync<T>(string key, CancellationToken cancellationToken);

    /// <summary>
    /// Saves all the state store changes.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Sets an object to the state store.
    /// </summary>
    /// <typeparam name="T">Type of the object to save.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SetStateAsync<T>(string key, T value, CancellationToken cancellationToken);

    /// <summary>
    /// Tries to get the state from the store.
    /// </summary>
    /// <typeparam name="T">The type of the object to retrieve.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;ConditionalValue&lt;T&gt;&gt;.</returns>
    Task<ConditionalValue<T>> TryGetStateAsync<T>(string key, CancellationToken cancellationToken);
}