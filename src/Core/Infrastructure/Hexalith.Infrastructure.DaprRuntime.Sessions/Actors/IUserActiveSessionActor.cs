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
/// Interface for user active sessions actor operations.
/// </summary>
public interface IUserActiveSessionActor : IActor
{
    /// <summary>
    /// Gets the name of the actor.
    /// </summary>
    public static string ActorName => "UserActiveSession";

    /// <summary>
    /// Creates a user active session actor proxy.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A user active session actor proxy.</returns>
    public static IUserActiveSessionActor Actor(string userId)
    => ActorProxy.Create<IUserActiveSessionActor>(userId.ToActorId(), ActorName);

    /// <summary>
    /// Adds a new session asynchronously.
    /// </summary>
    /// <param name="sessionId">The ID of the session to add.</param>
    /// <param name="expirationDate"></param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(string sessionId, DateTimeOffset expirationDate);

    /// <summary>
    /// Retrieves all sessions asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of session IDs.</returns>
    Task<IEnumerable<ActiveSession>> AllAsync();

    Task RemoveAsync(string sessionId);
}