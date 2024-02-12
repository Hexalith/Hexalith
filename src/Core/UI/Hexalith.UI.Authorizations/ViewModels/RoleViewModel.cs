// <copyright file="RoleViewModel.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Authorizations.ViewModels;

using System.ComponentModel.DataAnnotations;

using Hexalith.Domain.Users.Models;

/// <summary>
/// Represents a role.
/// Implements the <see cref="IRole" />.
/// </summary>
/// <seealso cref="IRole" />
public class RoleViewModel : IRole
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleViewModel"/> class.
    /// </summary>
    public RoleViewModel() => Id = Name = Description = string.Empty;

    /// <inheritdoc/>
    public string Description { get; set; }

    /// <inheritdoc/>
    [Required]
    public string Id { get; set; }

    /// <inheritdoc/>
    [Required]
    public string Name { get; set; }
}