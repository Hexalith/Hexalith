// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Emails.MailKit
// Author           : Jérôme Piquot
// Created          : 01-14-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-14-2024
// ***********************************************************************
// <copyright file="MailKitEmailService.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Emails.Mailkit.Services;

using System.Threading;
using System.Threading.Tasks;

using global::MailKit.Net.Smtp;
using global::MailKit.Security;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Emails.Abstractions.Configurations;
using Hexalith.Infrastructure.Emails.Abstractions.Services;

using Microsoft.Extensions.Options;

using MimeKit;
using MimeKit.Text;

/// <summary>
/// Class MailKitEmailService.
/// Implements the <see cref="EmailServiceBase" />.
/// </summary>
/// <seealso cref="EmailServiceBase" />
public class MailKitEmailService : EmailServiceBase
{
    /// <summary>
    /// The password.
    /// </summary>
    private readonly string _password;

    /// <summary>
    /// The server name.
    /// </summary>
    private readonly string _serverName;

    /// <summary>
    /// The server port.
    /// </summary>
    private readonly int _serverPort;

    /// <summary>
    /// The user name.
    /// </summary>
    private readonly string _userName;

    /// <summary>
    /// Initializes a new instance of the <see cref="MailKitEmailService" /> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public MailKitEmailService(IOptions<EmailServerSettings> settings)
        : base(settings)
    {
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
    public override async Task SendAsync(string fromEmail, string fromName, string toEmail, string subject, string? plainTextContent, string? htmlContent, CancellationToken cancellationToken)
    {
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
        await smtp.ConnectAsync(_serverName, _serverPort, SecureSocketOptions.Auto, cancellationToken).ConfigureAwait(false);
        await smtp.AuthenticateAsync(_userName, _password, cancellationToken).ConfigureAwait(false);
        _ = await smtp.SendAsync(email, cancellationToken).ConfigureAwait(false);
        await smtp.DisconnectAsync(true, cancellationToken).ConfigureAwait(false);
    }
}