// <copyright file="RoleRoles.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Authorizations;

using System.Collections.Generic;

/// <summary>
/// Role management, roles list provider.
/// </summary>
public class RoleRoles : IRoleProvider
{
    /// <summary>
    /// The assign role to user role name.
    /// </summary>
    public const string AssignRoleToUser = nameof(AssignRoleToUser);

    /// <summary>
    /// The remove user role role name.
    /// </summary>
    public const string RemoveUserRole = nameof(RemoveUserRole);

    /// <summary>
    /// The role management role name.
    /// </summary>
    public const string ViewUserRoles = nameof(ViewUserRoles);

    /// <summary>
    /// Gets the collection of role management roles.
    /// </summary>
    public IEnumerable<string> Roles => [ViewUserRoles, AssignRoleToUser, RemoveUserRole];
}