// <copyright file="SendGridEmailService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Emails.SendGrid.Services;

using System.Threading.Tasks;

using global::SendGrid;
using global::SendGrid.Helpers.Mail;

using Hexalith.Application.Emails;
using Hexalith.Infrastructure.Emails.Abstractions.Configurations;
using Hexalith.Infrastructure.Emails.Abstractions.Services;

using Microsoft.Extensions.Options;

/// <summary>
/// Class SendGridEmailService.
/// Implements the <see cref="IEmailService" />.
/// </summary>
/// <seealso cref="IEmailService" />
public class SendGridEmailService : EmailServiceBase
{
    /// <summary>
    /// The API key.
    /// </summary>
    private readonly string _apiKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendGridEmailService" /> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public SendGridEmailService(IOptions<EmailServerSettings> settings)
        : base(settings)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.Value.ApplicationSecret);
        _apiKey = settings.Value.ApplicationSecret;
    }

    /// <inheritdoc/>
    public override async Task SendAsync(string fromEmail, string fromName, string toEmail, string subject, string? plainTextContent, string? htmlContent, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fromEmail);
        ArgumentException.ThrowIfNullOrWhiteSpace(toEmail);
        ArgumentException.ThrowIfNullOrWhiteSpace(subject);
        SendGridClient client = new(_apiKey);
        EmailAddress from = new(fromEmail, fromName);
        EmailAddress to = new(toEmail);
        SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        Response? response = await client.SendEmailAsync(msg, cancellationToken).ConfigureAwait(false);
        if (response == null || !response.IsSuccessStatusCode)
        {
            string message = response == null
                ? "Response from server is null"
                : await response.Body.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new InvalidOperationException($"Send email to {toEmail} with subject {subject} failed : {message}");
        }
    }
}