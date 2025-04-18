// <copyright file="OptionCollectionHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Helpers;

using Hexalith.Domains.ValueObjects;

using Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides helper methods for converting <see cref="IIdDescription"/> to <see cref="Option{TType}"/>.
/// </summary>
public static class OptionCollectionHelper
{
    /// <summary>
    /// Converts an <see cref="IIdDescription"/> to an <see cref="Option{TType}"/>.
    /// </summary>
    /// <param name="idDescription">The ID description to convert.</param>
    /// <param name="selected">A value indicating whether the option is selected.</param>
    /// <returns>An <see cref="Option{TType}"/> representing the ID description.</returns>
    public static Option<string> ToOption(this IIdDescription idDescription, bool selected)
    {
        return new Option<string>
        {
            Value = idDescription.Id,
            Text = idDescription.Description,
            Disabled = idDescription.Disabled,
            Selected = selected,
        };
    }

    /// <summary>
    /// Converts a collection of <see cref="IIdDescription"/> to a collection of <see cref="Option{TType}"/>.
    /// </summary>
    /// <param name="idDescriptions">The collection of ID descriptions to convert.</param>
    /// <param name="selected">A value indicating whether the options are selected.</param>
    /// <returns>A collection of <see cref="Option{TType}"/> representing the ID descriptions.</returns>
    public static IEnumerable<Option<string>> ToOptions(this IEnumerable<IIdDescription> idDescriptions, bool selected)
        => idDescriptions.Select(p => p.ToOption(selected));
}