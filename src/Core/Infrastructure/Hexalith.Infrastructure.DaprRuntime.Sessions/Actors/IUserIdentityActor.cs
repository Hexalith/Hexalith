// <copyright file="IUserIdentityActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Actors;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.Sessions.Models;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

/// <summary>
/// Represents a user identity actor interface for managing user state and operations in a distributed actor system.
/// </summary>
public interface IUserIdentityActor : IActor
{
    /// <summary>
    /// Adds a new user identity with the specified information.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="provider">The identity provider identifier.</param>
    /// <param name="name">The name of the user.</param>
    /// <param name="email">The email address of the user.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(string id, string provider, string name, string email);

    /// <summary>
    /// Disables the user account.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DisableAsync();

    /// <summary>
    /// Enables the user account.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task EnableAsync();

    /// <summary>
    /// Checks if the user identity exists in the system.
    /// </summary>
    /// <returns>A task representing the asynchronous operation that returns true if the user identity exists; otherwise, false.</returns>
    Task<bool> ExistsAsync();

    /// <summary>
    /// Attempts to find the user identity in the system.
    /// </summary>
    /// <returns>A task representing the asynchronous operation that returns the user identity if found; otherwise, null.</returns>
    Task<UserIdentity?> FindAsync();

    /// <summary>
    /// Retrieves the user identity information.
    /// </summary>
    /// <returns>A task representing the asynchronous operation that returns the user identity information.</returns>
    Task<UserIdentity> GetAsync();
}