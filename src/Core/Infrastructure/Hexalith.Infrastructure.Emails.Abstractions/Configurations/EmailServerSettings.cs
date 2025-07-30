// <copyright file="EmailServerSettings.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Emails.Abstractions.Configurations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Class EmailServerSettings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
public class EmailServerSettings : ISettings
{
    /// <summary>
    /// Gets or sets the application identifier.
    /// </summary>
    /// <value>The application identifier.</value>
    public string? ApplicationId { get; set; }

    /// <summary>
    /// Gets or sets the application secret.
    /// </summary>
    /// <value>The application secret.</value>
    public string? ApplicationSecret { get; set; }

    /// <summary>
    /// Gets or sets from email.
    /// </summary>
    /// <value>From email.</value>
    public string? FromEmail { get; set; }

    /// <summary>
    /// Gets or sets from name.
    /// </summary>
    /// <value>From name.</value>
    public string? FromName { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the provider name.
    /// </summary>
    public string? ProviderName { get; set; }

    /// <summary>
    /// Gets or sets the name of the server.
    /// </summary>
    /// <value>The name of the server.</value>
    public string? ServerName { get; set; }

    /// <summary>
    /// Gets or sets the server port.
    /// </summary>
    /// <value>The server port.</value>
    public int ServerPort { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    public string? UserName { get; set; }

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => "EmailServer";
}