// <copyright file="NoRolesUserManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Authorizations;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Represents a user manager implementation that does not have any roles. All users are administrators.
/// </summary>
/// <remarks>
/// This user manager always returns the administrator role membership for any user.
/// </remarks>
public class NoRolesUserManager : IUserManager
{
    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId, CancellationToken cancellationToken)
        => await Task
            .FromResult<IEnumerable<string>>([SystemRoles.Administrator])
            .ConfigureAwait(false);
}