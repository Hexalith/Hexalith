// <copyright file="ExceptionHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
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
    /// <returns>A message containing the exception and it's inner exceptions messages.</returns>
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
}