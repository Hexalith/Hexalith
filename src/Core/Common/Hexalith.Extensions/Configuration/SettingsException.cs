// ***********************************************************************
// Assembly         : Hexalith.Extensions
// Author           : Jérôme Piquot
// Created          : 04-23-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-23-2023
// ***********************************************************************
// <copyright file="SettingsException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Extensions.Configuration;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

/// <summary>
/// Class SettingsException.
/// Implements the <see cref="ArgumentException" />.
/// </summary>
/// <typeparam name="TSettings">The type of the t settings.</typeparam>
/// <seealso cref="ArgumentException" />
public class SettingsException<TSettings> : ArgumentException
    where TSettings : ISettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsException{TSettings}" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public SettingsException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsException{TSettings}"/> class.
    /// </summary>
    public SettingsException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsException{TSettings}"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public SettingsException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsException{TSettings}"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="paramName">The name of the parameter that caused the current exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public SettingsException(string? message, string? paramName, Exception? innerException)
        : base(message, paramName, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsException{TSettings}"/> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="paramName">The name of the parameter that caused the current exception.</param>
    public SettingsException(string? message, string? paramName)
        : base(message, paramName)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsException{TSettings}"/> class.
    /// </summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    protected SettingsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Throws if undefined.
    /// </summary>
    /// <param name="argument">The argument.</param>
    /// <param name="paramName">Name of the parameter.</param>
    public static void ThrowIfUndefined(
        [NotNull] object? argument,
        [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null || (argument is string str && string.IsNullOrWhiteSpace(str)))
        {
            string[] parts = string.IsNullOrEmpty(paramName) ? Array.Empty<string>() : paramName.Split(".", 2);
            Throw($"The {TSettings.ConfigurationName()}.{(parts.Length > 1 ? parts[1] : "?")} setting has not been defined.", paramName);
        }
    }

    /// <summary>
    /// Throws the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <exception cref="SettingsException{TSettings}">Throw settings exception.</exception>
    [DoesNotReturn]
    internal static void Throw(string? message, string? paramName) =>
    throw new SettingsException<TSettings>(message, paramName);
}