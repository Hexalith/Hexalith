// ***********************************************************************
// Assembly         : Hexalith.Extensions
// Author           : JérômePiquot
// Created          : 01-13-2023
//
// Last Modified By : JérômePiquot
// Last Modified On : 01-16-2023
// ***********************************************************************
// <copyright file="StringHelper.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
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
    public static string FormatWithNamedPlaceholders(IFormatProvider provider, string value, IEnumerable<object>? args)
    {
        string format = ReplacePlaceholderNamesByIndex(value);
        try
        {
            if (args != null)
            {
                object[] arguments = args.ToArray();
                if (arguments.Length > 0)
                {
                    return string.Format(
                        provider,
                        format,
                        arguments);
                }
            }

            return value;
        }
        catch (Exception e)
        {
            IEnumerable<string> argValues = (args ?? []).Select(p => $"{p?.GetType().Name ?? "null"}:{p ?? "null"}");
            throw new InvalidOperationException(
                $"Could not format :\nOriginal={value}\nIndexed={format}\nValues={string.Join("\n", argValues)}", e);
        }
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
        => decimal.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>
    /// Convert an invariant culture string to a long integer number.
    /// </summary>
    /// <param name="value">The number string.</param>
    /// <returns>The number.</returns>
    public static double ToDouble(this string value)
        => double.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>
    /// Convert an invariant culture string to an integer number.
    /// </summary>
    /// <param name="value">The number string.</param>
    /// <returns>The number.</returns>
    public static int ToInteger(this string value)
        => int.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>
    /// Convert double float number to invariant culture string.
    /// </summary>
    /// <param name="value">The number.</param>
    /// <returns>The value as string.</returns>
    public static string ToInvariantString(this double value)
        => value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Convert long number to invariant culture string.
    /// </summary>
    /// <param name="value">The number.</param>
    /// <returns>The value as string.</returns>
    public static string ToInvariantString(this long value)
        => value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Convert decimal number to invariant culture string.
    /// </summary>
    /// <param name="value">The number.</param>
    /// <returns>The value as string.</returns>
    public static string ToInvariantString(this decimal value)
        => value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Convert number to invariant culture string.
    /// </summary>
    /// <param name="value">The number.</param>
    /// <returns>The value as string.</returns>
    public static string ToInvariantString(this int value)
        => value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Convert an invariant culture string to a long integer number.
    /// </summary>
    /// <param name="value">The number string.</param>
    /// <returns>The number.</returns>
    public static long ToLong(this string value)
        => long.Parse(value, CultureInfo.InvariantCulture);
}