// <copyright file="MenuItemInformationTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.UI.Components;

using FluentAssertions;

using Hexalith.UI.Components;

public class MenuItemInformationTest
{
    [Fact]
    public void GetMenuItemIndex_Should_Return_False_When_MenuItem_Not_Found()
    {
        // Arrange
        MenuItemInformation menuItem = new(
            "Menu",
            "/",
            null,
            false,
            0,
            null,
            new List<MenuItemInformation>([
                new MenuItemInformation("Sub Menu", "/sub", null, false, 0, null, [])]));

        // Act
        (bool found, int last) = menuItem.GetMenuItemIndex(0, new MenuItemInformation("Not Found", "/not-found", null, false, 0, null, []));

        // Assert
        _ = found.Should().BeFalse();
        _ = last.Should().Be(2);
    }

    [Fact]
    public void GetMenuItemIndex_Should_Return_True_When_MenuItem_Found()
    {
        // Arrange
        MenuItemInformation foundItem = new("Found", "/found", null, false, 0, null, []);
        MenuItemInformation menuItem = new(
            "Menu",
            "/",
            null,
            false,
            0,
            null,
            new List<MenuItemInformation>([
                new MenuItemInformation("Sub Menu", "/sub", null, false, 0, null, []),
                new MenuItemInformation("Sub Menu", "/sub", null, false, 0, null, []),
                new MenuItemInformation("Sub Menu", "/sub", null, false, 0, null, []),
                new("Found", "/found", null, false, 0, null, []),
                new MenuItemInformation("Sub Menu", "/sub", null, false, 0, null, []),
                new MenuItemInformation("Sub Menu", "/sub", null, false, 0, null, [
                    new MenuItemInformation("Sub Menu", "/sub", null, false, 0, null, []),
                    new MenuItemInformation("Sub Menu", "/sub", null, false, 0, null, []),
                    foundItem,
                    new MenuItemInformation("Sub Menu", "/sub", null, false, 0, null, []),
                    new MenuItemInformation("Sub Menu", "/sub", null, false, 0, null, []),
                    ]),
                ]));

        // Act
        (bool found, int last) = menuItem.GetMenuItemIndex(0, foundItem);

        // Assert
        _ = found.Should().BeTrue();
        _ = last.Should().Be(10);
    }
}