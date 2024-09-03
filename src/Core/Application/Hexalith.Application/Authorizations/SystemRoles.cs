// <copyright file="SystemRoles.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Authorizations;

using System.Collections.Generic;

/// <summary>
/// Role list provider interface.
/// </summary>
public class SystemRoles : IRoleProvider
{
    /// <summary>
    /// Gets administrator role.
    /// </summary>
    public static string Administrator => nameof(Administrator);

    /// <summary>
    /// Gets system role.
    /// </summary>
    public static string System => nameof(System);

    /// <summary>
    /// Gets the collection of roles.
    /// </summary>
    public IEnumerable<string> Roles => [Administrator, System];
}