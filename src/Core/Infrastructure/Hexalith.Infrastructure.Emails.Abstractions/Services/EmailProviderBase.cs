// <copyright file="EmailProviderBase.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Emails.Abstractions.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Hexalith.Infrastructure.Emails.Abstractions.Configurations;

using Microsoft.Extensions.Options;

/// <summary>
/// Class EmailServiceBase.
/// Implements the <see cref="IEmailProvider" />.
/// </summary>
/// <seealso cref="IEmailProvider" />
public abstract class EmailProviderBase : IEmailProvider
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailProviderBase" /> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected EmailProviderBase([NotNull] IOptions<EmailServerSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(settings.Value);
        DefaultFromEmail = settings.Value.FromEmail;
        DefaultFromName = settings.Value.FromName;
    }

    /// <summary>
    /// Gets the default from email.
    /// </summary>
    /// <value>The default from email.</value>
    public string? DefaultFromEmail { get; }

    /// <summary>
    /// Gets the default from name.
    /// </summary>
    /// <value>The default from name.</value>
    public string? DefaultFromName { get; }

    /// <inheritdoc/>
    public abstract string? ProviderName { get; }

    /// <inheritdoc/>
    public abstract Task SendAsync(string fromEmail, string fromName, string toEmail, string subject, string? plainTextContent, string? htmlContent, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public virtual async Task SendAsync(string toEmail, string subject, string? plainTextContent, string? htmlContent, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(DefaultFromEmail))
        {
            throw new InvalidOperationException($"Default sender email is not defined. Set application settings : {EmailServerSettings.ConfigurationName()}.{new EmailServerSettings().FromEmail}.");
        }

        await SendAsync(DefaultFromEmail, DefaultFromName ?? DefaultFromEmail, toEmail, subject, plainTextContent, htmlContent, cancellationToken)
            .ConfigureAwait(false);
    }
}