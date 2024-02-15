// <copyright file="UserViewModel.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Authentications.ViewModels;

using System.ComponentModel.DataAnnotations;

using Hexalith.Domain.Users.Models;

/// <summary>
/// Represents a user.
/// Implements the <see cref="IUser" />.
/// </summary>
/// <seealso cref="IUser" />
public class UserViewModel : IUser
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserViewModel"/> class.
    /// </summary>
    public UserViewModel() => Email = Id = Name = string.Empty;

    /// <inheritdoc/>
    [Required]
    public string Email { get; set; }

    /// <inheritdoc/>
    [Required]
    public string Id { get; set; }

    /// <inheritdoc/>
    [Required]
    public string Name { get; set; }
}