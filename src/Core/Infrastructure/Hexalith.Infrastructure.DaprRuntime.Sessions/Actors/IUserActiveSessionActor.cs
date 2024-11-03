// <copyright file="IUserActiveSessionActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Actors;

using Dapr.Actors;

using Hexalith.Application.Sessions.Models;

/// <summary>
/// Interface for user active sessions actor operations.
/// </summary>
public interface IUserActiveSessionActor : IActor
{
    /// <summary>
    /// Gets the name of the actor.
    /// </summary>
    public static string ActorName => "UserActiveSession";

    /// <summary>
    /// Adds a new session asynchronously.
    /// </summary>
    /// <param name="sessionId">The ID of the session to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(string sessionId, DateTimeOffset expirationDate);

    /// <summary>
    /// Retrieves all sessions asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of session IDs.</returns>
    Task<IEnumerable<ActiveSession>> AllAsync();

    Task RemoveAsync(string sessionId);
}