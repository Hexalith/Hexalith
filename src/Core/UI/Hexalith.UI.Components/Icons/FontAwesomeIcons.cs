// <copyright file="FontAwesomeIcons.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Icons;

using Hexalith.UI.Components.Helpers;

using Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides methods to get Font Awesome icons.
/// </summary>
public static class FontAwesomeIcons
{
    /// <summary>
    /// Gets the Font Awesome navigation icon based on the provided information.
    /// </summary>
    /// <param name="info">The icon information.</param>
    /// <returns>The Font Awesome icon.</returns>
    public static Icon GetNavIcon(IconInformation info)
    {
        ArgumentNullException.ThrowIfNull(info);
        string content = $@"
            <svg class=""fluent-nav-icon"" style=""width: {info.Size}px; fill: var(--accent-fill-rest);"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 512 512"">
              <use href=""./_content/{info.IconLibraryName}/fontawesome/sprites/{GetStylePath(info.Style)}.svg#{info.Name.ToLowerInvariant()}""></use>
            </svg>
            ";
        Icon icon = new(
            info.Name,
            info.Style switch { IconStyle.Regular => IconVariant.Regular, IconStyle.Filled => IconVariant.Filled, IconStyle.Light => IconVariant.Light, IconStyle.Thin => IconVariant.Light, _ => IconVariant.Regular },
            IconHelper.GetIconSize(info.Size),
            content);
        return icon;
    }

    /// <summary>
    /// Gets the Font Awesome Tab icon based on the provided information.
    /// </summary>
    /// <param name="info">The icon information.</param>
    /// <returns>The Font Awesome icon.</returns>
    public static Icon GetTabIcon(IconInformation info)
    {
        ArgumentNullException.ThrowIfNull(info);

        // <svg class=""fluent-tab-icon"" style=""width: {info.Size - 2}px; fill: var(--accent-fill-rest);"" xmlns=""http://www.w3.org/2000/svg"" focusable=""false"" viewBox=""0 0 512 512"">
        string content = $@"
            <svg style=""padding:2px; padding-bottom:0px;"" xmlns=""http://www.w3.org/2000/svg"" focusable=""false"" viewBox=""0 0 512 512"">
              <use href=""./_content/{info.IconLibraryName}/fontawesome/sprites/{GetStylePath(info.Style)}.svg#{info.Name.ToLowerInvariant()}""></use>
            </svg>
            ";
        Icon icon = new(
            info.Name,
            info.Style switch { IconStyle.Regular => IconVariant.Regular, IconStyle.Filled => IconVariant.Filled, IconStyle.Light => IconVariant.Light, IconStyle.Thin => IconVariant.Light, _ => IconVariant.Regular },
            IconHelper.GetIconSize(info.Size),
            content);
        return icon;
    }

    /// <summary>
    /// Gets the style path for the specified icon style.
    /// </summary>
    /// <param name="style">The icon style.</param>
    /// <returns>The style path.</returns>
    private static string GetStylePath(IconStyle style)
    {
        return style switch
        {
            IconStyle.Regular => "regular",
            IconStyle.Filled => "solid",
            IconStyle.Light => "light",
            IconStyle.Thin => "thin",
            _ => throw new ArgumentOutOfRangeException(nameof(style), style, "Unsupported style for Fontawesome icons."),
        };
    }
}