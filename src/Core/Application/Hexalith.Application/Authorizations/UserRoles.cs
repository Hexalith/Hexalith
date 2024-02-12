// <copyright file="UserRoles.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Authorizations;

using System.Collections.Generic;

/// <summary>
/// User roles list provider.
/// </summary>
public class UserRoles : IRoleProvider
{
    /// <summary>
    /// The constant representing the User Management role.
    /// </summary>
    public const string UserManagement = nameof(UserManagement);

    /// <summary>
    /// Gets the collection of roles.
    /// </summary>
    public IEnumerable<string> Roles => [UserManagement];
}