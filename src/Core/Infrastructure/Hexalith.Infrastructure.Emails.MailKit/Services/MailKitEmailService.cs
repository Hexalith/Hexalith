﻿// <copyright file="MailKitEmailService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Emails.Mailkit.Services;

using System.Threading;
using System.Threading.Tasks;

using global::MailKit.Net.Smtp;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Emails.Abstractions.Configurations;
using Hexalith.Infrastructure.Emails.Abstractions.Services;

using Microsoft.Extensions.Options;

using MimeKit;
using MimeKit.Text;

/// <summary>
/// Class MailKitEmailService.
/// Implements the <see cref="EmailProviderBase" />.
/// </summary>
/// <seealso cref="EmailProviderBase" />
public class MailKitEmailService : EmailProviderBase
{
    private readonly bool _enabled;

    /// <summary>
    /// The password.
    /// </summary>
    private readonly string? _password;

    /// <summary>
    /// The server name.
    /// </summary>
    private readonly string? _serverName;

    /// <summary>
    /// The server port.
    /// </summary>
    private readonly int _serverPort;

    /// <summary>
    /// The user name.
    /// </summary>
    private readonly string? _userName;

    /// <summary>
    /// Initializes a new instance of the <see cref="MailKitEmailService" /> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public MailKitEmailService(IOptions<EmailServerSettings> settings)
        : base(settings)
    {
        if (settings.Value.ProviderName != ProviderName)
        {
            _enabled = false;
            return;
        }

        SettingsException<EmailServerSettings>.ThrowIfNullOrEmpty(settings.Value.Password);
        SettingsException<EmailServerSettings>.ThrowIfNullOrEmpty(settings.Value.UserName);
        SettingsException<EmailServerSettings>.ThrowIfNullOrEmpty(settings.Value.ServerName);
        SettingsException<EmailServerSettings>.ThrowIfUndefined(settings.Value.ServerPort);

        _password = settings.Value.Password;
        _userName = settings.Value.UserName;
        _serverName = settings.Value.ServerName;
        _serverPort = settings.Value.ServerPort;
    }

    /// <inheritdoc/>
    public override string? ProviderName => nameof(MailKit);

    /// <inheritdoc/>
    public override async Task SendAsync(string fromEmail, string fromName, string toEmail, string subject, string? plainTextContent, string? htmlContent, CancellationToken cancellationToken)
    {
        if (!_enabled)
        {
            return;
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(fromEmail);
        ArgumentException.ThrowIfNullOrWhiteSpace(toEmail);
        ArgumentException.ThrowIfNullOrWhiteSpace(subject);

        // create message
        using MimeMessage email = new()
        {
            Sender = MailboxAddress.Parse(fromEmail),
        };
        if (!string.IsNullOrWhiteSpace(fromName))
        {
            email.Sender.Name = fromEmail;
        }

        email.From.Add(email.Sender);
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = string.IsNullOrWhiteSpace(htmlContent)
            ? new TextPart(TextFormat.Plain) { Text = plainTextContent }
            : new TextPart(TextFormat.Html) { Text = htmlContent };

        // send email
        using SmtpClient smtp = new();
        await smtp.ConnectAsync(_serverName, _serverPort, cancellationToken: cancellationToken).ConfigureAwait(false);
        await smtp.AuthenticateAsync(_userName, _password, cancellationToken).ConfigureAwait(false);
        _ = await smtp.SendAsync(email, cancellationToken).ConfigureAwait(false);
        await smtp.DisconnectAsync(true, cancellationToken).ConfigureAwait(false);
    }
}