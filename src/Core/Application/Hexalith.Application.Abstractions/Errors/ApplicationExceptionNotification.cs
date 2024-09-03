// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-26-2023
// ***********************************************************************
// <copyright file="ApplicationExceptionNotification.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Errors;

using System.Globalization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Notifications;
using Hexalith.Extensions.Errors;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class ApplicationExceptionNotification.
/// Implements the <see cref="BaseNotification" />.
/// </summary>
/// <seealso cref="BaseNotification" />
public class ApplicationExceptionNotification : BaseNotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationExceptionNotification"/> class.
    /// </summary>
    [Obsolete("For serialization purpose only", true)]
    public ApplicationExceptionNotification()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationExceptionNotification"/> class.
    /// </summary>
    /// <param name="sourceAggregateName">Name of the source aggregate.</param>
    /// <param name="sourceAggregateId">The source aggregate identifier.</param>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="technicalDescription">The technical description.</param>
    [JsonConstructor]
    public ApplicationExceptionNotification(
        string sourceAggregateName,
        string sourceAggregateId,
        string title,
        string message,
        string? technicalDescription)
        : base(sourceAggregateName, sourceAggregateId, title, message, NotificationSeverity.Error, technicalDescription)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationExceptionNotification"/> class.
    /// </summary>
    /// <param name="sourceAggregateName">Name of the source aggregate.</param>
    /// <param name="sourceAggregateId">The source aggregate identifier.</param>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="e">The e.</param>
    public ApplicationExceptionNotification(
        string sourceAggregateName,
        string sourceAggregateId,
        string title,
        string message,
        Exception e)
        : this(sourceAggregateName, sourceAggregateId, title, message, e?.FullMessage())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationExceptionNotification"/> class.
    /// </summary>
    /// <param name="sourceAggregateName">Name of the source aggregate.</param>
    /// <param name="sourceAggregateId">The source aggregate identifier.</param>
    /// <param name="exception">The exception.</param>
    public ApplicationExceptionNotification(
        string sourceAggregateName,
        string sourceAggregateId,
        ApplicationErrorException exception)
        : this(
              sourceAggregateName,
              sourceAggregateId,
              GetTitle(exception),
              GetMessage(exception),
              GetTechnicalDescription(exception))
    {
    }

    private static string GetMessage(ApplicationErrorException exception)
    {
        return exception.Error == null || string.IsNullOrWhiteSpace(exception.Error.Detail)
            ? exception.FullMessage()
            : StringHelper.FormatWithNamedPlaceholders(
            CultureInfo.InvariantCulture,
            exception.Error.Detail,
            exception.Error.Arguments);
    }

    private static string GetTechnicalDescription(ApplicationErrorException exception)
    {
        return exception.Error == null || string.IsNullOrWhiteSpace(exception.Error.TechnicalDetail)
            ? exception.FullMessage()
            : StringHelper.FormatWithNamedPlaceholders(
                CultureInfo.InvariantCulture,
                exception.Error.TechnicalDetail,
                exception.Error.TechnicalArguments);
    }

    private static string GetTitle(ApplicationErrorException exception)
            => exception?.Error?.Title ?? "Error";
}