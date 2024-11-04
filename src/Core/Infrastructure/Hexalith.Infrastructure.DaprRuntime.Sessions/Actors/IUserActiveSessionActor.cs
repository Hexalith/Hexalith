// <copyright file="IUserActiveSessionActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Actors;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.Sessions.Models;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

/// <summary>
/// Represents an actor interface for managing user active sessions in a distributed system.
/// This actor maintains the state of active sessions for a specific user, including their creation,
/// retrieval, and removal operations.
/// </summary>
public interface IUserActiveSessionActor : IActor
{
    /// <summary>
    /// Adds a new session asynchronously.
    /// </summary>
    /// <param name="sessionId">The ID of the session to add.</param>
    /// <param name="expirationDate">The date and time when the session will expire.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task AddAsync(string sessionId, DateTimeOffset expirationDate);

    /// <summary>
    /// Retrieves all sessions asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains a collection of <see cref="ActiveSession"/>.</returns>
    Task<IEnumerable<ActiveSession>> AllAsync();

    /// <summary>
    /// Removes a session asynchronously.
    /// </summary>
    /// <param name="sessionId">The ID of the session to remove.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    Task RemoveAsync(string sessionId);
}