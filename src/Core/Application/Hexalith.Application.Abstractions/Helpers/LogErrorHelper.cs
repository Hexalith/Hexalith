// <copyright file="LogErrorHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Helpers;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.Errors;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class LogErrorHelper.
/// </summary>
public static class LogErrorHelper
{
    /// <summary>
    /// Logs the error.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="error">The error.</param>
    public static void LogError([NotNull] this ILogger logger, [NotNull] ApplicationError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        ArgumentNullException.ThrowIfNull(logger);
#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA2254 // Template should be a static expression
        if (error.Detail != null)
        {
            logger.LogError(error.Detail, error.Arguments);
        }

        if (error.TechnicalDetail != null)
        {
            logger.LogError(error.TechnicalDetail, error.TechnicalArguments);
        }
#pragma warning restore CA2254 // Template should be a static expression
#pragma warning restore CA1848 // Use the LoggerMessage delegates
    }

    /// <summary>
    /// Logs the error.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="error">The error.</param>
    /// <exception cref="System.ArgumentNullException">null arguments.</exception>
    public static void LogError([NotNull] this ILogger logger, [NotNull] ApplicationErrorException error)
    {
        ArgumentNullException.ThrowIfNull(error);
        ArgumentNullException.ThrowIfNull(logger);
#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA2254 // Template should be a static expression
        if (error.Error == null)
        {
            logger.LogError(error, "An application exception occurred without any error description. Exception message : {ErrorMessage}.", error.FullMessage());
            return;
        }

        if (!string.IsNullOrWhiteSpace(error.Error.Detail))
        {
            logger.LogError(error, error.Error.Detail, error.Error.Arguments);
        }

        if (!string.IsNullOrWhiteSpace(error.Error.TechnicalDetail))
        {
            logger.LogError(error, error.Error.TechnicalDetail, error.Error.TechnicalArguments);
        }
#pragma warning restore CA2254 // Template should be a static expression
#pragma warning restore CA1848 // Use the LoggerMessage delegates
    }
}