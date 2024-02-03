// <copyright file="IRole.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Users.Models;

/// <summary>
/// Represents a role.
/// </summary>
public interface IRole
{
    /// <summary>
    /// Gets the description of the role.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the ID of the role.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the name of the role.
    /// </summary>
    string Name { get; }
}