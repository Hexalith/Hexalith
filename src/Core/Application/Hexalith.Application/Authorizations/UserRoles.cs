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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2339:Public constant members should not be used", Justification = "Need to be const as it's used in attribute")]
    public const string UserManagement = nameof(UserManagement);

    /// <summary>
    /// Gets the collection of roles.
    /// </summary>
    public IEnumerable<string> Roles => [UserManagement];
}