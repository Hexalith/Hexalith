// <copyright file="UniqueIdHelper.cs.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

using System;
using System.Globalization;

public static class UniqueIdHelper
{
    private static readonly object _lock = new();
    private static DateTime _lastUsedDateTime = DateTime.MinValue;

    /// <summary>
    /// Generates a new unique ID based on the current UTC date/time with the format "yyyyMMddHHmmssfff".
    /// It ensures uniqueness by incrementing the time if multiple calls occur within the same millisecond.
    /// </summary>
    /// <returns>A unique 17-character ID string derived from the current date/time.</returns>
    public static string GenerateDateTimeId()
    {
        lock (_lock)
        {
            DateTime now = DateTime.UtcNow;

            // If now is not strictly greater than the last used timestamp, increment the last timestamp by one millisecond.
            if (now <= _lastUsedDateTime)
            {
                now = _lastUsedDateTime.AddMilliseconds(1);
            }

            _lastUsedDateTime = now;
            return now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Generates a unique 22-character ID string derived from a GUID encoded in Base64 URL-safe format.
    /// </summary>
    /// <returns>A 22-character unique ID string.</returns>
    public static string GenerateUniqueStringId()
    {
        return Convert
            .ToBase64String(Guid.NewGuid().ToByteArray())[..22]
            .Replace("/", "_", StringComparison.InvariantCulture)
            .Replace("+", "-", StringComparison.InvariantCulture);
    }
}