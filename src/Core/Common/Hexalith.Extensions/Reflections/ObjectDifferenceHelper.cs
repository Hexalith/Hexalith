// <copyright file="ObjectDifferenceHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Reflections;

using KellermanSoftware.CompareNetObjects;

/// <summary>
/// Class ObjectDifferenceHelper.
/// </summary>
public static class ObjectDifferenceHelper
{
    /// <summary>
    /// Gets the object properties difference.
    /// </summary>
    /// <param name="currentValue">The current value.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>System.Collections.Generic.Dictionary&lt;string, (object? CurrentValue, object? NewValue)&gt;.</returns>
    public static string? GetDifferenceInformation(this object? currentValue, object? newValue)
    {
        CompareLogic compareLogic = new();

        ComparisonResult result = compareLogic.Compare(currentValue, newValue);

        return !result.AreEqual ? result.DifferencesString : null;
    }
}