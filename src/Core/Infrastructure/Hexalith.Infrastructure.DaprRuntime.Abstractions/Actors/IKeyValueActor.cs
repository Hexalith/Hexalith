// <copyright file="IKeyValueActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using Dapr.Actors;

/// <summary>
/// Interface IKeyValueActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
public interface IKeyValueActor : IActor
{
    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
    Task<string?> GetAsync();

    /// <summary>
    /// Removes the asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    Task RemoveAsync();

    /// <summary>
    /// Sets the asynchronous.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>Task.</returns>
    Task SetAsync(string? value);
}