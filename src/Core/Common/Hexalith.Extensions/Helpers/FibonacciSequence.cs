// <copyright file="FibonacciSequence.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

using System;

/// <summary>
/// Calculate the Fibonacci sequence helper.
/// </summary>
public static class FibonacciSequence
{
    /// <summary>
    /// Calculate the Fibonacci sequence.
    /// </summary>
    /// <param name="n">The sequence number.</param>
    /// <returns>The sequence Fibonacci value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Must be greater than zero.</exception>
    public static long Number(long n)
    {
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Must be greater than zero.");
        }

        if (n == 0)
        {
            return 0;
        }

        if (n == 1)
        {
            return 1;
        }

        long a = 0;
        long b = 1;
        for (int i = 2; i <= n; i++)
        {
            long c = a + b;
            a = b;
            b = c;
        }

        return b;
    }
}