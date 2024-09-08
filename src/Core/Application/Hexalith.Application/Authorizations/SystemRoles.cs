// <copyright file="SystemRoles.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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