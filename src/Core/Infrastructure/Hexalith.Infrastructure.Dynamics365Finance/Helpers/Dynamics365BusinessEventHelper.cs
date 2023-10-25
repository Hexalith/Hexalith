// <copyright file="Dynamics365BusinessEventHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Helpers;

using System;

/// <summary>
/// The dynamics365 business event helper.
/// </summary>
public static class Dynamics365BusinessEventHelper
{
    private static readonly char[] _separator = ['(', ')'];

    /// <summary>
    /// Parses the dynamics365 event time.
    /// </summary>
    /// <param name="d365Time">The d365 time.</param>
    /// <returns>A DateTime? .</returns>
    public static DateTime? ParseDynamics365EventTime(string? d365Time)
    {
        if (string.IsNullOrWhiteSpace(d365Time))
        {
            return null;
        }

        string[] dateParts = d365Time.Split(_separator);
        return dateParts.Length != 3 || !string.Equals(dateParts[0], "/Date", StringComparison.Ordinal) || !string.Equals(dateParts[2], "/", StringComparison.Ordinal) || !long.TryParse(dateParts[1], out long milliseconds)
            ? null
            : DateTime.UnixEpoch.AddMilliseconds(milliseconds);
    }

    /// <summary>
    /// Parses the dynamics365 event time offset.
    /// </summary>
    /// <param name="d365Time">The D365 time.</param>
    /// <returns>System.Nullable&lt;DateTimeOffset&gt;.</returns>
    public static DateTimeOffset? ParseDynamics365EventTimeOffset(string? d365Time)
    {
        DateTime? date = ParseDynamics365EventTime(d365Time);
        return date == null ? null : new DateTimeOffset(date.Value, TimeSpan.Zero);
    }
}