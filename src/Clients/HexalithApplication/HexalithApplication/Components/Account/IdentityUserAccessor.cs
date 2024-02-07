// ***********************************************************************
// Assembly         : HexalithApplication
// Author           : Jérôme Piquot
// Created          : 01-14-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-14-2024
// ***********************************************************************
// <copyright file="IdentityUserAccessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace HexalithApplication.Components.Account;

using Hexalith.Infrastructure.ClientApp.Models;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Class IdentityUserAccessor. This class cannot be inherited.
/// </summary>
internal sealed class IdentityUserAccessor(UserManager<ApplicationUser> userManager, IdentityRedirectManager redirectManager)
{
    /// <summary>
    /// Get required user as an asynchronous operation.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>A Task&lt;ApplicationUser&gt; representing the asynchronous operation.</returns>
    public async Task<ApplicationUser> GetRequiredUserAsync(HttpContext context)
    {
        ApplicationUser? user = await userManager.GetUserAsync(context.User).ConfigureAwait(false);

        if (user is null)
        {
            redirectManager.RedirectToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
        }

        return user;
    }
}