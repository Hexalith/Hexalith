// <copyright file="MenuService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;

public interface IMenuService
{
    IAsyncEnumerable<MenuItemInformation> GetMenuItemsAsync(ClaimsPrincipal? user, CancellationToken cancellationToken);
}