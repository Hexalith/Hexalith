// ***********************************************************************
// Assembly         : Deploy
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-03-2023
// ***********************************************************************
// <copyright file="ArgumentMissingException.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

/// <summary>
/// Class ArgumentMissingException.
/// Implements the <see cref="System.Exception" />.
/// </summary>
/// <seealso cref="System.Exception" />
[Serializable]
internal class ArgumentMissingException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentMissingException" /> class.
    /// </summary>
    public ArgumentMissingException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentMissingException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ArgumentMissingException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentMissingException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
    public ArgumentMissingException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentMissingException" /> class.
    /// </summary>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <param name="settingsName">Name of the settings.</param>
    /// <param name="settingsSection">The settings section.</param>
    public ArgumentMissingException(string? parameterName, string? settingsName, string? settingsSection)
        : base(CreateMessage(parameterName, settingsName, settingsSection))
    {
        ParameterName = parameterName;
        SettingsName = settingsName;
        SettingsSection = settingsSection;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentMissingException" /> class.
    /// </summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected ArgumentMissingException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Gets the name of the parameter.
    /// </summary>
    /// <value>The name of the parameter.</value>
    public string? ParameterName { get; }

    /// <summary>
    /// Gets the name of the settings.
    /// </summary>
    /// <value>The name of the settings.</value>
    public string? SettingsName { get; }

    /// <summary>
    /// Gets the settings section.
    /// </summary>
    /// <value>The settings section.</value>
    public string? SettingsSection { get; }

    /// <summary>
    /// Throws if null.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="parameter">The parameter.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="settingsSection">The settings section.</param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <param name="settingsName">Name of the settings.</param>
    /// <returns>T.</returns>
    /// <exception cref="global.ArgumentMissingException"></exception>
    public static T ThrowIfNull<T>(
        T parameter,
        T settings,
        string settingsSection,
        [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
        [CallerArgumentExpression(nameof(settings))] string? settingsName = null)
    {
        return parameter ?? settings ?? throw new ArgumentMissingException(parameterName, settingsName, settingsSection);
    }

    /// <summary>
    /// Throws if null or white space.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="settingsSection">The settings section.</param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <param name="settingsName">Name of the settings.</param>
    /// <returns>System.String.</returns>
    /// <exception cref="global.ArgumentMissingException">Null.</exception>
    public static string ThrowIfNullOrWhiteSpace(
            string? parameter,
            string? settings,
            string settingsSection,
            [CallerArgumentExpression(nameof(parameter))] string? parameterName = null,
            [CallerArgumentExpression(nameof(settings))] string? settingsName = null)
    {
        return string.IsNullOrWhiteSpace(parameter)
            ? string.IsNullOrWhiteSpace(settings)
                ? throw new ArgumentMissingException(parameterName, settingsName, settingsSection)
                : settings
            : parameter;
    }

    private static string? CreateMessage(string? parameterName, string? settingsName, string? settingsSection)
    {
        return $"The parameter '{parameterName ?? "Unknown"}' is not defined, neither the default value '{(string.IsNullOrWhiteSpace(settingsSection) ? string.Empty : settingsSection + ".")}{settingsName?.Split('.').Last() ?? "Unknown"}' in the application settings.";
    }
}