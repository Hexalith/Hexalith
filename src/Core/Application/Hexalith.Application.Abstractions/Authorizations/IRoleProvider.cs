// <copyright file="IRoleProvider.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Authorizations;

using System.Collections.Generic;

/// <summary>
/// Role list provider interface.
/// </summary>
public interface IRoleProvider
{
    /// <summary>
    /// Gets the collection of roles.
    /// </summary>
    IEnumerable<string> Roles { get; }
}