// <copyright file="ISessionActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Actors;

using Dapr.Actors;

using Hexalith.Application.Sessions.Models;

/// <summary>
/// Interface for session actor operations.
/// </summary>
public interface ISessionActor : IActor
{
    /// <summary>
    /// Gets the name of the actor.
    /// </summary>
    public static string ActorName => "Session";

    /// <summary>
    /// Closes the session asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous close operation.</returns>
    Task CloseAsync();

    /// <summary>
    /// Gets the session asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous get operation. The task result contains the session.</returns>
    Task<Session> GetAsync();

    /// <summary>
    /// Opens the session asynchronously.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="name">The session name.</param>
    /// <returns>A task that represents the asynchronous open operation.</returns>
    Task OpenAsync(string partitionId, string userId);
}