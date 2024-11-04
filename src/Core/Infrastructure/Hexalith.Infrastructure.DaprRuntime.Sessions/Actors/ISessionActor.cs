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
/// Interface for session actor operations.
/// </summary>
public interface ISessionActor : IActor
{
    /// <summary>
    /// Gets the name of the actor.
    /// </summary>
    public static string ActorName => "Session";

    /// <summary>
    /// Creates a session actor proxy.
    /// </summary>
    /// <param name="sessionId">The session identifier.</param>
    /// <returns>A session actor proxy.</returns>
    public static ISessionActor Actor(string sessionId)
        => ActorProxy.Create<ISessionActor>(sessionId.ToActorId(), ActorName);

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
    /// <param name="identityProviderName">The name of the user identity provider.</param>
    /// <returns>A task that represents the asynchronous open operation.</returns>
    Task OpenAsync(string partitionId, string userId, string identityProviderName);
}