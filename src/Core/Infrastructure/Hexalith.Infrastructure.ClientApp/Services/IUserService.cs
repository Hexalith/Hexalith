// <copyright file="IUserService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Services;

using System.Threading.Tasks;

/// <summary>
/// Represents a service for managing user sessions and retrieving user information.
/// </summary>
/// <remarks>
/// This interface defines the contract for user-related operations in the client application.
/// Implementations of this interface should handle user authentication, session management,
/// and retrieval of user-specific data.
/// </remarks>
public interface IUserService
{
    /// <summary>
    /// Retrieves the unique identifier for the current user asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation.
    /// The task result contains a string representing the user's unique identifier.
    /// </returns>
    /// <remarks>
    /// This method should be used to obtain the current user's ID, which can be used for
    /// authorization, personalization, or tracking purposes within the application.
    /// If no user is currently authenticated, the implementation should handle this scenario
    /// appropriately (e.g., by returning a guest ID or throwing an exception).
    /// </remarks>
    Task<string> GetUserIdAsync(CancellationToken cancellationToken);
}
