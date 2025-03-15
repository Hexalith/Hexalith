// <copyright file="SystemRoles.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2339:Public constant members should not be used", Justification = "Need to be const as it's used in attribute")]
    public const string Administrator = nameof(Administrator);

    /// <summary>
    /// Gets system role.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2339:Public constant members should not be used", Justification = "Need to be const as it's used in attribute")]
    public const string System = nameof(System);

    /// <summary>
    /// Gets the collection of roles.
    /// </summary>
    public IEnumerable<string> Roles => [Administrator, System];
}