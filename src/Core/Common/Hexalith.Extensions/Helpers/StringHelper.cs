// <copyright file="StringHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

using System.Globalization;

/// <summary>
/// String helper class.
/// </summary>
public static class StringHelper
{
    /// <summary>
    /// Convert double float number to invariant culture string.
    /// </summary>
    /// <param name="value">The number.</param>
    /// <returns>The value as string.</returns>
    public static string ToInvariantString(this double value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Convert long number to invariant culture string.
    /// </summary>
    /// <param name="value">The number.</param>
    /// <returns>The value as string.</returns>
    public static string ToInvariantString(this long value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Convert number to invariant culture string.
    /// </summary>
    /// <param name="value">The number.</param>
    /// <returns>The value as string.</returns>
    public static string ToInvariantString(this int value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
}
