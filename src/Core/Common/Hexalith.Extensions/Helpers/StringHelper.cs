// ***********************************************************************
// Assembly         : Hexalith.Extensions
// Author           : JérômePiquot
// Created          : 01-13-2023
//
// Last Modified By : JérômePiquot
// Last Modified On : 01-16-2023
// ***********************************************************************
// <copyright file="StringHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Extensions.Helpers;

using System.Globalization;
using System.Text.RegularExpressions;

/// <summary>
/// String helper class.
/// </summary>
public static partial class StringHelper
{
    /// <summary>
    /// Formats the with named placeholders.
    /// </summary>
    /// <param name="provider">The provider.</param>
    /// <param name="value">The value.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>System.String.</returns>
    public static string FormatWithNamedPlaceholders(IFormatProvider provider, string value, params object?[] args)
    {
        return string.Format(
            provider,
            ReplacePlaceholderNamesByIndex(value),
            args);
    }

    /// <summary>
    /// Formats the with named placeholders.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>System.String.</returns>
    public static string FormatWithNamedPlaceholders(string value, params object?[] args)
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            ReplacePlaceholderNamesByIndex(value),
            args);
    }

    /// <summary>
    /// Replaces the placeholder names by their index.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.String.</returns>
    public static string ReplacePlaceholderNamesByIndex(string value)
    {
        string pattern = @"\{\w+\}";
        int i = 0;
        return Regex.Replace(value, pattern, match => "{" + i++ + "}");
    }

    /// <summary>
    /// Convert an invariant culture string to a decimal number.
    /// </summary>
    /// <param name="value">The number string.</param>
    /// <returns>The number.</returns>
    public static decimal ToDecimal(this string value)
    {
        return decimal.Parse(value, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Convert an invariant culture string to a long integer number.
    /// </summary>
    /// <param name="value">The number string.</param>
    /// <returns>The number.</returns>
    public static double ToDouble(this string value)
    {
        return double.Parse(value, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Convert an invariant culture string to an integer number.
    /// </summary>
    /// <param name="value">The number string.</param>
    /// <returns>The number.</returns>
    public static int ToInteger(this string value)
    {
        return int.Parse(value, CultureInfo.InvariantCulture);
    }

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
    /// Convert decimal number to invariant culture string.
    /// </summary>
    /// <param name="value">The number.</param>
    /// <returns>The value as string.</returns>
    public static string ToInvariantString(this decimal value)
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

    /// <summary>
    /// Convert an invariant culture string to a long integer number.
    /// </summary>
    /// <param name="value">The number string.</param>
    /// <returns>The number.</returns>
    public static long ToLong(this string value)
    {
        return long.Parse(value, CultureInfo.InvariantCulture);
    }
}