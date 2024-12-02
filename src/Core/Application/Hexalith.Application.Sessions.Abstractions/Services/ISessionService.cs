// <copyright file="ISessionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Services;

using System.Threading.Tasks;

using Hexalith.Application.Sessions.Models;

/// <summary>
/// Defines the contract for managing user sessions in the application.
/// </summary>
public interface ISessionService
{
    /// <summary>
    /// Gets the session information for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the session information.</returns>
    Task<SessionInformation> GetAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Sets the current partition for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetCurrentPartitionAsync(string userId, string partitionId, CancellationToken cancellationToken);
}