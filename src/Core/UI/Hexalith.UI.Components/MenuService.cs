// <copyright file="MenuService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Filters menu items based on security policies and user authorization.
/// </summary>
/// <remarks>
/// This class implements <see cref="IEnumerable{MenuItemInformation}"/> to provide a filtered collection
/// of menu items where only items accessible to the current user are included.
/// </remarks>
public sealed class MenuService : IMenuService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IEnumerable<MenuItemInformation> _menuItems;

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuService"/> class.
    /// </summary>
    /// <param name="menuItems">The collection of menu items to filter.</param>
    /// <param name="authorizationService">The authorization service used to check user permissions.</param>
    /// <exception cref="ArgumentNullException">Thrown when menuItems, authorizationService or httpContext is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when HttpContext or IAuthorizationService is not available.</exception>
    public MenuService(
        [NotNull] IEnumerable<MenuItemInformation> menuItems,
        [NotNull] IAuthorizationService authorizationService)
    {
        ArgumentNullException.ThrowIfNull(menuItems);
        ArgumentNullException.ThrowIfNull(authorizationService);
        _menuItems = menuItems;
        _authorizationService = authorizationService;
    }

    /// <summary>
    /// Recursively filters menu items based on security policies.
    /// </summary>
    /// <param name="user">The user for whom the menu items are filtered.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the filtering operation.</param>
    /// <returns>A filtered collection containing only menu items the current user is authorized to access.</returns>
    /// <remarks>
    /// This method:
    /// - Includes items without security policies
    /// - Checks authorization for items with security policies
    /// - Recursively filters sub-items at all levels (sub-items, sub-sub-items, etc.)
    /// - Skips items the user is not authorized to access
    /// - Caches authorization results to avoid redundant checks
    /// - Ensures all menu items at every level are properly filtered by authorization.
    /// </remarks>
    public IAsyncEnumerable<MenuItemInformation> GetMenuItemsAsync(ClaimsPrincipal? user, CancellationToken cancellationToken)
    {
        // Create a new cache for each enumeration to ensure thread safety
        Dictionary<string, bool> authorizationCache = new(StringComparer.OrdinalIgnoreCase);
        return FilterMenuItemsAsync(user, _menuItems, authorizationCache, cancellationToken);
    }

    private async Task<bool> CheckMenuItemPolicyAsync(Dictionary<string, bool> authorizationCache, ClaimsPrincipal? user, string securityPolicy)
    {
        bool isAuthorized;

        if (user is null)
        {
            if (string.IsNullOrWhiteSpace(securityPolicy))
            {
                // No security policy, no need to check authorization
                return true;
            }

            // If user is null, they are not authorized
            return false;
        }

        // Check cache first
        if (authorizationCache.TryGetValue(securityPolicy, out bool cachedResult))
        {
            isAuthorized = cachedResult;
        }
        else
        {
            // Not in cache, perform authorization check
            AuthorizationResult authResult = await _authorizationService.AuthorizeAsync(user, securityPolicy);
            isAuthorized = authResult.Succeeded;

            // Cache the result
            authorizationCache[securityPolicy] = isAuthorized;
        }

        if (!isAuthorized)
        {
            // User is not authorized, skip this item and its sub-items
            return false;
        }

        return true;
    }

    private async IAsyncEnumerable<MenuItemInformation> FilterMenuItemsAsync(
        ClaimsPrincipal? user,
        IEnumerable<MenuItemInformation> items,
        Dictionary<string, bool> authorizationCache,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (MenuItemInformation item in items)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Check authorization first if there's a security policy
            if (!string.IsNullOrWhiteSpace(item.SecurityPolicy) && !await CheckMenuItemPolicyAsync(authorizationCache, user, item.SecurityPolicy))
            {
                continue;
            }

            // User is authorized or no security policy, include the item with filtered sub-items
            if (item.SubItems.Any())
            {
                List<MenuItemInformation> filteredSubItems = [];
                await foreach (MenuItemInformation subItem in FilterMenuItemsAsync(user, item.SubItems, authorizationCache, cancellationToken))
                {
                    filteredSubItems.Add(subItem);
                }

                // Always return the item with filtered sub-items to ensure proper authorization at all levels
                yield return item with { SubItems = filteredSubItems };
            }
            else
            {
                yield return item;
            }
        }
    }
}