// <copyright file="DateHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

using Ardalis.GuardClauses;

using System;

/// <summary>
/// Class DateHelper.
/// </summary>
public static class DateHelper
{
    /// <summary>
    /// Converts a date only value to a local date time with the specified offset.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <param name="offset">The time offset.</param>
    /// <returns>DateTimeOffset.</returns>
    public static DateTimeOffset ToLocalTime(this DateOnly date, TimeSpan offset)
    {
        _ = Guard.Against.Null(date);
        return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, offset);
    }

    /// <summary>
    /// Converts a date only value to a GMT(UTC+0) date time.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns>DateTimeOffset.</returns>
    public static DateTimeOffset ToUniveralTime(this DateOnly date)
    {
        _ = Guard.Against.Null(date);
        return date.ToLocalTime(TimeSpan.Zero);
    }
}