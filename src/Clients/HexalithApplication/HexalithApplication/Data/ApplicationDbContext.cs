// ***********************************************************************
// Assembly         : HexalithApplication
// Author           : Jérôme Piquot
// Created          : 01-14-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-14-2024
// ***********************************************************************
// <copyright file="ApplicationDbContext.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace HexalithApplication.Data;

using Hexalith.Infrastructure.ClientApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Class ApplicationDbContext.
/// Implements the <see cref="Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext{ApplicationUser}" />.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext{ApplicationUser}" />
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
}