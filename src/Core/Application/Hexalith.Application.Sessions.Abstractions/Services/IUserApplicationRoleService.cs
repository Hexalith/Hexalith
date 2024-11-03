// <copyright file="IUserApplicationRoleService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Services;

using Hexalith.Application.Sessions.Models;

/// <summary>
/// Interface for user application role service.
/// </summary>
public interface IUserApplicationRoleService
{
    /// <summary>
    /// Gets all application roles for a user in a specific partition.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of application roles.</returns>
    Task<IEnumerable<ApplicationRole>> AllAsync(string userId, CancellationToken cancellationToken);
}