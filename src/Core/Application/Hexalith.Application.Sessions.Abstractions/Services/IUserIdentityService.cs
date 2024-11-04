// <copyright file="IUserIdentityService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Services;

using System.Threading.Tasks;

using Hexalith.Application.Sessions.Models;

/// <summary>
/// Interface for managing user identity operations.
/// </summary>
public interface IUserIdentityService
{
    /// <summary>
    /// Adds a new user identity.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="provider">The identity provider.</param>
    /// <param name="name">The user name.</param>
    /// <param name="email">The user's email address.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created user identity.</returns>
    Task<UserIdentity> AddAsync(string id, string provider, string name, string email, CancellationToken cancellationToken);

    /// <summary>
    /// Checks if a user identity exists.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="provider">The identity provider.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the user identity exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(string id, string provider, CancellationToken cancellationToken);

    /// <summary>
    /// Finds a user identity by ID.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="provider">The identity provider.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user identity if found; otherwise, null.</returns>
    Task<UserIdentity?> FindAsync(string id, string provider, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a user identity by ID.
    /// </summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="provider">The identity provider.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user identity.</returns>
    Task<UserIdentity> GetAsync(string id, string provider, CancellationToken cancellationToken);
}