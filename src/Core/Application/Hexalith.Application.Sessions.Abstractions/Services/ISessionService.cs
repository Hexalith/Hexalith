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
    /// Closes a session asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the session to close.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task CloseAsync(string id);

    /// <summary>
    /// Retrieves session information asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the session to retrieve.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the <see cref="SessionInformation"/> if found; otherwise, null.</returns>
    Task<SessionInformation?> GetAsync(string id);

    /// <summary>
    /// Opens a new session asynchronously.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom to open the session.</param>
    /// <param name="partitionId">The partition identifier for the new session.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the newly created <see cref="SessionInformation"/>.</returns>
    Task<SessionInformation> OpenAsync(string userId, string partitionId);
}