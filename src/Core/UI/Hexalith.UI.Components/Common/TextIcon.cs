// <copyright file="TextIcon.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Common;

using Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a text icon component.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TextIcon"/> class.
/// </remarks>
/// <param name="character">The character to be displayed as an icon.</param>
public class TextIcon(string character) : Icon(character, IconVariant.Regular, IconSize.Custom, $"<span style=\"padding: 0px; margin: 0px;width: 20px;font-size: 20px;color: var(--accent-fill-rest);\">{character}</span>")
{
}