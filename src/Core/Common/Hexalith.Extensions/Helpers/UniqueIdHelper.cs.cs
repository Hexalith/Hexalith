// <copyright file="UniqueIdHelper.cs.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

using System.Globalization;

/// <summary>
/// Unique Id generators.
/// </summary>
public static class UniqueIdHelper
{
    private static readonly Lock _lock = new();
    private static string? _previousDateTimeId;

    /// <summary>
    /// Generate a new unique id of 17 characters from the current date time (yyyyMMddHHmmssfff). Only one Id per millisecond can be generated.
    /// </summary>
    /// <returns>Id string.</returns>
    public static string GenerateDateTimeId()
    {
        lock (_lock)
        {
            string value = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
            while (string.Equals(value, _previousDateTimeId, StringComparison.Ordinal))
            {
                Thread.Sleep(1);
                value = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
            }

            return _previousDateTimeId = value;
        }
    }

    /// <summary>
    /// Generate a new unique id of 22 characters.
    /// </summary>
    /// <returns>Id string.</returns>
    public static string GenerateUniqueStringId()
    {
        return Convert
                .ToBase64String(Guid.NewGuid().ToByteArray())[..22]
                .Replace("/", "_", StringComparison.InvariantCulture)
                .Replace("+", "-", StringComparison.InvariantCulture);
    }
}