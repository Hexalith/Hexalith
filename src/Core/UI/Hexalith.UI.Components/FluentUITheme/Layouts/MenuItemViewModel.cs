// <copyright file="MenuItemViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.FluentUITheme.Layouts;

using System.Collections.Generic;
using System.Linq;

using Hexalith.Extensions.Helpers;
using Hexalith.UI.Components.Icons;

/// <summary>
/// Represents a view model for a menu item.
/// This class extends the <see cref="MenuItemInformation"/> class and includes an identifier.
/// </summary>
public record MenuItemViewModel(
    string Id,
    string Name,
    string? Path,
    IconInformation? Icon,
    bool DividerAfter,
    int OrderWeight,
    string? SecurityPolicy,
    IEnumerable<MenuItemInformation> SubItems) : MenuItemInformation(Name, Path, Icon, DividerAfter, OrderWeight, SecurityPolicy, SubItems)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MenuItemViewModel"/> class.
    /// </summary>
    /// <param name="item">The menu item information.</param>
    /// <param name="subItems">The sub-items associated with the menu item.</param>
    public MenuItemViewModel(MenuItemInformation item, IEnumerable<MenuItemViewModel> subItems)
        : this(
            UniqueIdHelper.GenerateUniqueStringId(),
            item.Name,
            item.Path,
            item.Icon,
            item.DividerAfter,
            item.OrderWeight,
            item.SecurityPolicy,
            subItems)
    {
    }

    /// <summary>
    /// Converts and orders a collection of <see cref="MenuItemInformation"/> items into <see cref="MenuItemViewModel"/> items.
    /// </summary>
    /// <param name="items">The collection of menu item information.</param>
    /// <returns>An enumerable collection of <see cref="MenuItemViewModel"/> items.</returns>
    public static IEnumerable<MenuItemViewModel> From(IEnumerable<MenuItemInformation> items)
        => items.OrderByDescending(p => p.OrderWeight).Select(Convert);

    private static MenuItemViewModel Convert(MenuItemInformation item)
        => new(item, item.SubItems.OrderByDescending(p => p.OrderWeight).Select(Convert));
}