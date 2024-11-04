// <copyright file="ActorProxyHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Helpers;

using Dapr.Actors.Client;

using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Sessions.Actors;

/// <summary>
/// Provides helper methods for creating and managing Dapr actor proxies related to user sessions and identities.
/// This static class centralizes the creation of actor proxies and maintains consistent actor naming conventions.
/// </summary>
/// <remarks>
/// The ActorProxyHelper simplifies the interaction with various actor types in the system by providing
/// strongly-typed proxy creation methods and maintaining constant actor names. It handles actors for:
/// - Session management
/// - User identity management
/// - Active session tracking
/// - Collection of user identities.
/// </remarks>
public static class ActorProxyHelper
{
    /// <summary>
    /// Gets the identifier used for the collection of all user identity IDs.
    /// </summary>
    public static string AllIdsCollectionName => "AllIds";

    /// <summary>
    /// Gets the actor proxy for managing the collection of all user identity IDs.
    /// </summary>
    /// <remarks>
    /// This actor maintains a list of all user identity IDs in the system for efficient querying and management.
    /// </remarks>
    public static IKeyListActor AllUserIdentityIdsActor
            => ActorProxy.Create<IKeyListActor>(AllIdsCollectionName.ToActorId(), UserIdentityCollectionName);

    /// <summary>
    /// Gets the name of the actor.
    /// This name is used to identify session actor instances in the Dapr runtime.
    /// </summary>
    public static string SessionActorName => "Session";

    /// <summary>
    /// Gets the name of the actor.
    /// </summary>
    public static string UserActiveSessionActorName => "UserActiveSession";

    /// <summary>
    /// Gets the name of the actor.
    /// </summary>
    public static string UserIdentityActorName => "UserIdentity";

    /// <summary>
    /// Gets the name of the collection used to store user identities.
    /// </summary>
    public static string UserIdentityCollectionName => "UserIdentities";

    /// <summary>
    /// Creates a session actor proxy for interacting with a specific session.
    /// </summary>
    /// <param name="sessionId">The unique identifier of the session to create an actor for.</param>
    /// <returns>A proxy instance of <see cref="ISessionActor"/> for the specified session.</returns>
    public static ISessionActor SessionActor(this string sessionId)
        => ActorProxy.Create<ISessionActor>(sessionId.ToActorId(), SessionActorName);

    /// <summary>
    /// Creates a user active session actor proxy.
    /// </summary>
    /// <param name="userId">The user identifier used to create a unique actor instance.</param>
    /// <returns>A proxy instance of <see cref="IUserActiveSessionActor"/> for the specified user.</returns>
    public static IUserActiveSessionActor UserActiveSessionActor(this string userId)
        => ActorProxy.Create<IUserActiveSessionActor>(userId.ToActorId(), UserActiveSessionActorName);

    /// <summary>
    /// Creates a proxy to interact with a user identity actor instance.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="provider">The identity provider identifier.</param>
    /// <returns>A proxy instance to interact with the user identity actor.</returns>
    public static IUserIdentityActor UserIdentityActor(string id, string provider)
        => ActorProxy.Create<IUserIdentityActor>($"{id}-{provider}".ToActorId(), UserIdentityActorName);

    /// <summary>
    /// Creates a proxy to interact with a user identity actor instance using a tuple containing the user ID and provider.
    /// </summary>
    /// <param name="user">A tuple containing the user's ID and provider information. The tuple consists of:
    /// - Id: The unique identifier of the user
    /// - Provider: The identity provider identifier</param>
    /// <returns>A proxy instance to interact with the user identity actor.</returns>
    public static IUserIdentityActor UserIdentityActor(this (string Id, string Provider) user)
        => ActorProxy.Create<IUserIdentityActor>($"{user.Id}-{user.Provider}".ToActorId(), UserIdentityActorName);
}
