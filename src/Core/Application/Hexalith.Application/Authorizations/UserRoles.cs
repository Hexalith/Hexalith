// <copyright file="UserRoles.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Authorizations;

using System.Collections.Generic;

/// <summary>
/// User roles list provider.
/// </summary>
public class UserRoles : IRoleProvider
{
    /// <summary>
    /// Gets the constant representing the User Management role.
    /// </summary>
    public static string UserManagement => nameof(UserManagement);

    /// <summary>
    /// Gets the collection of roles.
    /// </summary>
    public IEnumerable<string> Roles => [UserManagement];
}