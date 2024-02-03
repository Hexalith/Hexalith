// <copyright file="IUser.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Users.Models;

/// <summary>
/// Represents a user.
/// </summary>
public interface IUser
{
    /// <summary>
    /// Gets the email of the user.
    /// </summary>
    string Email { get; }

    /// <summary>
    /// Gets the ID of the user.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the name of the user.
    /// </summary>
    string Name { get; }
}