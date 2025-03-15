// <copyright file="ApplicationRoles.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application;

/// <summary>
/// Provides application role names as constants.
/// </summary>
public static class ApplicationRoles
{
    /// <summary>
    /// Gets the name of the global administrator role.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2339:Public constant members should not be used", Justification = "Need to be const as it's used in attribute")]
    public const string GlobalAdministrator = nameof(GlobalAdministrator);
}