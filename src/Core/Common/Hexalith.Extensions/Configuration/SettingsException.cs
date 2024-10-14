// <copyright file="SettingsException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Configuration;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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
    /// Throws if undefined.
    /// </summary>
    /// <param name="argument">The argument.</param>
    /// <param name="paramName">Name of the parameter.</param>
#pragma warning disable CA1000 // Do not declare static members on generic types

    public static void ThrowIfUndefined(
        [NotNull] object? argument,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument is null || (argument is string str && string.IsNullOrWhiteSpace(str)))
        {
            string? settingsName = string.IsNullOrWhiteSpace(paramName)
                ? string.Empty
                : paramName.Split(".").LastOrDefault();
            if (string.IsNullOrWhiteSpace(settingsName))
            {
                settingsName = "Unknown";
            }

            Throw($"The {settingsName} value is undefined in {TSettings.ConfigurationName()} settings. Argument : {paramName}.", paramName);
        }
    }

#pragma warning restore CA1000 // Do not declare static members on generic types

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