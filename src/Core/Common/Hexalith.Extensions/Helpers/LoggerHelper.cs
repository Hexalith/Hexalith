// ***********************************************************************
// Assembly         : Hexalith.Extensions
// Author           : Jérôme Piquot
// Created          : 12-19-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-19-2023
// ***********************************************************************
// <copyright file="LoggerHelper.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Extensions.Helpers;

using System;
using System.Globalization;

using Hexalith.Extensions.Common;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class LoggerHelper.
/// </summary>
public static partial class LoggerHelper
{
    /// <summary>
    /// Logs the application technical error.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="technicalMessage">The message.</param>
    [LoggerMessage(2, LogLevel.Error, "{Title}: {Message}\n{TechnicalMessage}")]
    public static partial void LogApplicationError(this ILogger logger, Exception? exception, string? title, string? message, string? technicalMessage);

    /// <summary>
    /// Logs the application error details.
    /// </summary>
    /// <param name="applicationError">The application error.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="exception">The exception.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static void LogApplicationErrorDetails(this ApplicationError applicationError, ILogger logger, Exception? exception)
    {
        ArgumentNullException.ThrowIfNull(applicationError);
        ArgumentNullException.ThrowIfNull(logger);
        string? details = applicationError.Detail == null
            ? null
            : StringHelper.FormatWithNamedPlaceholders(CultureInfo.InvariantCulture, applicationError.Detail, applicationError.Arguments);
        string? technicalDetails = applicationError.TechnicalDetail == null
            ? null
            : StringHelper.FormatWithNamedPlaceholders(CultureInfo.InvariantCulture, applicationError.TechnicalDetail, applicationError.TechnicalArguments);
        logger.LogApplicationError(
            exception,
            applicationError.Title,
            details,
            technicalDetails);

        applicationError.InnerError?.LogApplicationErrorDetails(logger, null);
    }
}