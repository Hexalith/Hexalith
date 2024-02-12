// <copyright file="ApplicationUser.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Security.Abstractions.Models;

using Microsoft.AspNetCore.Identity;

// Add profile data for application users by adding properties to the ApplicationUser class

/// <summary>
/// Class ApplicationUser.
/// Implements the <see cref="IdentityUser" />.
/// </summary>
/// <seealso cref="IdentityUser" />
public class ApplicationUser : IdentityUser
{
}