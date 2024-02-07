// <copyright file="UserInfo.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Security;

using Hexalith.Domain.Users.Models;

/// <summary>
/// Represents user security information.
/// Implements the <see cref="IUser" />.
/// </summary>
/// <seealso cref="IUser" />
public class UserInfo : IUser
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserInfo"/> class.
    /// </summary>
    public UserInfo() => Email = Id = Name = string.Empty;

    /// <inheritdoc/>
    public string Email { get; set; }

    /// <inheritdoc/>
    public string Id { get; set; }

    /// <inheritdoc/>
    public string Name { get; }
}