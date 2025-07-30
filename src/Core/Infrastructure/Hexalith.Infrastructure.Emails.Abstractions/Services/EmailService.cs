// <copyright file="EmailService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Emails.Abstractions.Services;

using System;
using System.Threading.Tasks;

using Hexalith.Application.Emails;
using Hexalith.Infrastructure.Emails.Abstractions.Configurations;

using Microsoft.Extensions.Options;

/// <summary>
/// Class EmailServiceBase.
/// Implements the <see cref="IEmailProvider" />.
/// </summary>
/// <seealso cref="IEmailProvider" />
/// <remarks>
/// Initializes a new instance of the <see cref="EmailService" /> class.
/// </remarks>
/// <param name="settings">The settings.</param>
/// <param name="providers">The email providers.</param>
/// <exception cref="System.ArgumentNullException">null.</exception>
public abstract class EmailService(IOptions<EmailServerSettings> settings, IEnumerable<IEmailProvider> providers) : IEmailService
{
    private string? ProviderName { get; } = settings?.Value?.ProviderName;

    private IEnumerable<IEmailProvider> Providers { get; } = providers;

    /// <inheritdoc/>
    public abstract Task SendAsync(string fromEmail, string fromName, string toEmail, string subject, string? plainTextContent, string? htmlContent, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public virtual async Task SendAsync(string toEmail, string subject, string? plainTextContent, string? htmlContent, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(ProviderName))
        {
            return;
        }

        IEmailProvider? provider = Providers.FirstOrDefault(p => p.ProviderName == ProviderName)
            ?? throw new InvalidOperationException(
                $"Email provider '{ProviderName}' is not configured. Set application settings : {EmailServerSettings.ConfigurationName()}.{new EmailServerSettings().ProviderName}.");

        await provider.SendAsync(
            toEmail,
            subject,
            plainTextContent,
            htmlContent,
            cancellationToken).ConfigureAwait(false);
    }
}