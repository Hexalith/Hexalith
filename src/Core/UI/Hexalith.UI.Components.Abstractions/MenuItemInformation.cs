// <copyright file="MenuItemInformation.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components;

using System.Collections.Generic;
using System.Runtime.Serialization;

using Hexalith.UI.Components.Icons;

/// <summary>
/// Represents the information of a menu item.
/// </summary>
/// <param name="Name">The menu item name.</param>
/// <param name="Path">The url path.</param>
/// <param name="Icon">The item icon.</param>
/// <param name="DividerAfter">Add a divider after the menu item.</param>
/// <param name="OrderWeigth">Defines the order of the menu item. The lower the value, the higher the item will be displayed.</param>
/// <param name="SubItems">List of sub menu items.</param>
[DataContract]
public record MenuItemInformation(
    [property: DataMember] string Name,
    [property: DataMember] string? Path,
    [property: DataMember] IconInformation? Icon,
    [property: DataMember] bool DividerAfter,
    [property: DataMember] int OrderWeigth,
    [property: DataMember] IEnumerable<MenuItemInformation> SubItems)
{
    /// <summary>
    /// Finds the index of a menu item in the sub items list.
    /// </summary>
    /// <param name="start">The starting index.</param>
    /// <param name="menuItem">The menu item to find.</param>
    /// <returns>A tuple indicating whether the menu item was found and the last index.</returns>
    public (bool Found, int Last) GetMenuItemIndex(int start, MenuItemInformation menuItem)
    {
        start++;
        if (ReferenceEquals(menuItem, this))
        {
            return (true, start);
        }

        foreach (MenuItemInformation subItem in SubItems)
        {
            (bool found, start) = subItem.GetMenuItemIndex(start, menuItem);
            if (found)
            {
                return (true, start);
            }
        }

        return (false, start);
    }
}