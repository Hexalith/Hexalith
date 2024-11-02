// <copyright file="ISessionIdService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Services;

/// <summary>
/// Defines a service for retrieving session IDs.
/// </summary>
public interface ISessionIdService
{
    /// <summary>
    /// Asynchronously gets the session ID.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a result of the session ID as a string.</returns>
    Task<string?> GetSessionIdAsync();
}