// <copyright file="RoleRoles.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Authorizations;

using System.Collections.Generic;

/// <summary>
/// Role management, roles list provider.
/// </summary>
public class RoleRoles : IRoleProvider
{
    /// <summary>
    /// Gets the assign role to user role name.
    /// </summary>
    public static string AssignRoleToUser => nameof(AssignRoleToUser);

    /// <summary>
    /// Gets the remove user role role name.
    /// </summary>
    public static string RemoveUserRole => nameof(RemoveUserRole);

    /// <summary>
    /// Gets the collection of role management roles.
    /// </summary>
    public static IEnumerable<string> Roles => [ViewUserRoles, AssignRoleToUser, RemoveUserRole];

    /// <summary>
    /// Gets the role management role name.
    /// </summary>
    public static string ViewUserRoles => nameof(ViewUserRoles);

    /// <inheritdoc/>
    IEnumerable<string> IRoleProvider.Roles => Roles;
}