// <copyright file="ExceptionHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

using System;
using System.Text;

/// <summary>
/// Helper class for exceptions.
/// </summary>
public static class ExceptionHelper
{
    /// <summary>
    /// Gets the innermost exception.
    /// </summary>
    /// <param name="ex">The exception to get message from.</param>
    /// <returns>A message containing the exception and its inner exceptions messages.</returns>
    public static string FullMessage(this Exception ex)
    {
        if (ex == null)
        {
            return string.Empty;
        }

        StringBuilder message = new(ex.Message);
        Exception? inner = ex;
        while ((inner = inner.InnerException) != null)
        {
            _ = message
                .Append(' ')
                .Append(inner.Message);
        }

        return message.ToString();
    }

    /// <summary>
    /// Get inner exception of type T.
    /// </summary>
    /// <typeparam name="T">The type of the inner exception.</typeparam>
    /// <param name="ex">The exception to get the inner exception from.</param>
    /// <returns>The inner exception of type T, or null if not found.</returns>
    public static T? GetInnerException<T>(this Exception ex)
        where T : Exception
    {
        if (ex == null)
        {
            return null;
        }

        Exception? inner = ex;
        while ((inner = inner.InnerException) != null)
        {
            if (inner is T t)
            {
                return t;
            }
        }

        return null;
    }
}