// <copyright file="IconHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Helpers;

using System;

using Hexalith.UI.Components.Icons;

using Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Helper class for converting <see cref="IconInformation"/> to <see cref="Icon"/>.
/// </summary>
public static class IconHelper
{
    /// <summary>
    /// Gets the <see cref="IconSize"/> based on the provided size.
    /// </summary>
    /// <param name="size">The size of the icon.</param>
    /// <returns>The corresponding <see cref="IconSize"/>.</returns>
    public static IconSize GetIconSize(int size)
    {
        return size < (int)IconSize.Size12
            ? IconSize.Size10
            : size < (int)IconSize.Size16
            ? IconSize.Size12
            : size < (int)IconSize.Size20
            ? IconSize.Size16
            : size < (int)IconSize.Size24
            ? IconSize.Size20
            : size < (int)IconSize.Size28
            ? IconSize.Size24
            : size < (int)IconSize.Size32 ? IconSize.Size28 : size < (int)IconSize.Size48 ? IconSize.Size32 : IconSize.Size48;
    }

    /// <summary>
    /// Converts an <see cref="IconInformation"/> to a Nav <see cref="Icon"/>.
    /// </summary>
    /// <param name="icon">The <see cref="IconInformation"/> to convert.</param>
    /// <returns>The converted <see cref="Icon"/>.</returns>
    public static Icon? ToFluentNavIcon(this IconInformation? icon)
    {
        return icon == null
            ? null
            : icon.Source switch
            {
                IconSource.Fluent => new IconInfo
                {
                    Name = icon.Name,
                    Size = GetIconSize(icon.Size),
                    Variant = GetIconStyle(icon.Style),
                }.GetInstance(),
                IconSource.FontAwesome => FontAwesomeIcons.GetNavIcon(icon),
                _ => throw new ArgumentOutOfRangeException(nameof(icon), icon.Source, $"Invalid icon source : {icon.Source}"),
            };
    }

    /// <summary>
    /// Converts an <see cref="IconInformation"/> to a Tab <see cref="Icon"/>.
    /// </summary>
    /// <param name="icon">The <see cref="IconInformation"/> to convert.</param>
    /// <returns>The converted <see cref="Icon"/>.</returns>
    public static Icon? ToFluentTabIcon(this IconInformation? icon)
    {
        return icon == null
            ? null
            : icon.Source switch
            {
                IconSource.Fluent => new IconInfo
                {
                    Name = icon.Name,
                    Size = GetIconSize(icon.Size),
                    Variant = GetIconStyle(icon.Style),
                }.GetInstance(),
                IconSource.FontAwesome => FontAwesomeIcons.GetTabIcon(icon),
                _ => throw new ArgumentOutOfRangeException(nameof(icon), icon.Source, $"Invalid icon source : {icon.Source}"),
            };
    }

    /// <summary>
    /// Gets the <see cref="IconVariant"/> based on the provided style.
    /// </summary>
    /// <param name="style">The style of the icon.</param>
    /// <returns>The corresponding <see cref="IconVariant"/>.</returns>
    private static IconVariant GetIconStyle(IconStyle style) => style switch
    {
        IconStyle.Filled => IconVariant.Filled,
        IconStyle.Regular => IconVariant.Regular,
        IconStyle.Light => throw new NotImplementedException(),
        IconStyle.Thin => throw new NotImplementedException(),
        _ => throw new ArgumentOutOfRangeException(nameof(style), style, $"Invalid style : {style}"),
    };
}