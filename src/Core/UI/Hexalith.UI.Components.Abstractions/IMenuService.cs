// <copyright file="IMenuService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;

/// <summary>
/// Provides menu services for retrieving menu items based on user permissions.
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// Gets the menu items available to the specified user asynchronously.
    /// </summary>
    /// <param name="user">The claims principal representing the user. Can be null for anonymous users.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An async enumerable of menu item information.</returns>
    IAsyncEnumerable<MenuItemInformation> GetMenuItemsAsync(ClaimsPrincipal? user, CancellationToken cancellationToken);
}