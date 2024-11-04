// <copyright file="ISessionActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Actors;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.Sessions.Models;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

/// <summary>
/// Represents an actor interface for managing individual user sessions in a distributed system.
/// This actor handles session lifecycle operations including opening, closing, and retrieving
/// session information for specific users within partitions.
/// </summary>
public interface ISessionActor : IActor
{
    /// <summary>
    /// Closes the current session asynchronously.
    /// This operation marks the session as terminated and performs necessary cleanup.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous close operation.</returns>
    Task CloseAsync();

    /// <summary>
    /// Retrieves the current session's information asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation.
    /// The task result contains the <see cref="Session"/> with its current state and properties.</returns>
    Task<Session> GetAsync();

    /// <summary>
    /// Opens a new session asynchronously with the specified parameters.
    /// </summary>
    /// <param name="partitionId">The unique identifier of the partition this session belongs to.</param>
    /// <param name="userId">The unique identifier of the user this session is created for.</param>
    /// <param name="identityProviderName">The name of the authentication provider that verified the user's identity.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous open operation.</returns>
    Task OpenAsync(string partitionId, string userId, string identityProviderName);
}