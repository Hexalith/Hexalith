// <copyright file="DateHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

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
    public static DateTimeOffset ToLocalTime(this DateOnly date, TimeSpan offset) => new(date.Year, date.Month, date.Day, 0, 0, 0, offset);

    /// <summary>
    /// Converts a date only value to a GMT(UTC+0) date time.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns>DateTimeOffset.</returns>
    public static DateTimeOffset ToUniversalTime(this DateOnly date) => date.ToLocalTime(TimeSpan.Zero);

    /// <summary>
    /// Waits the time.
    /// </summary>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    /// <returns>TimeSpan.</returns>
    public static TimeSpan WaitTime(this DateTimeOffset? from, DateTimeOffset? to) => from is null ? TimeSpan.Zero : from.Value.WaitTime(to);

    /// <summary>
    /// Waits the time.
    /// </summary>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    /// <returns>TimeSpan.</returns>
    public static TimeSpan WaitTime(this DateTimeOffset from, DateTimeOffset? to)
    {
        if (to is null)
        {
            return TimeSpan.Zero;
        }

        long waitTime = to.Value.UtcTicks - from.UtcTicks;
        return waitTime > 0 ? new TimeSpan(waitTime) : TimeSpan.Zero;
    }
}