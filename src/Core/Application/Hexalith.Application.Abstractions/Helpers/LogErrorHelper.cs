// <copyright file="LogErrorHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Helpers;

using Ardalis.GuardClauses;

using Hexalith.Extensions.Common;

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
    public static void LogError(this ILogger logger, Error error)
    {
        _ = Guard.Against.Null(logger);
        _ = Guard.Against.Null(error);
#pragma warning disable CA2254 // Template should be a static expression
        logger.LogError(error.TechnicalDetail, error.TechnicalArguments);
#pragma warning restore CA2254 // Template should be a static expression
    }
}