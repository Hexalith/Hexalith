// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis
// Author           : Jérôme Piquot
// Created          : 12-11-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-11-2023
// ***********************************************************************
// <copyright file="ProblemDetailsHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.Helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

/// <summary>
/// Class ProblemDetailsHelper.
/// </summary>
public static partial class ProblemDetailsHelper
{
    /// <summary>
    /// Logs the problem details.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="level">The level.</param>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    [LoggerMessage(EventId = 0)]
    public static partial void LogProblemDetails(this ILogger logger, LogLevel level, string? title, string? message);

    /// <summary>
    /// Logs the error.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="problem">The problem.</param>
    public static void LogProblemError(this ILogger logger, ProblemDetails problem)
        => LogProblemDetails(logger, LogLevel.Error, problem?.Title, problem?.Detail);

    /// <summary>
    /// Logs the warning.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="problem">The problem.</param>
    public static void LogProblemWarning(this ILogger logger, ProblemDetails problem)
        => LogProblemDetails(logger, LogLevel.Warning, problem?.Title, problem?.Detail);
}