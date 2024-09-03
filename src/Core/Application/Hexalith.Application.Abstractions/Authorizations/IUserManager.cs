// <copyright file="IUserManager.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Authorizations;

using System.Collections.Generic;

/// <summary>
/// Represents a user manager interface.
/// </summary>
/// <remarks>
/// This interface provides functionality to manage user roles.
/// </remarks>
public interface IUserManager
{
    /// <summary>
    /// Gets the roles assigned to the user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The roles assigned to the user.</returns>
    Task<IEnumerable<string>> GetUserRolesAsync(string userId, CancellationToken cancellationToken);
}