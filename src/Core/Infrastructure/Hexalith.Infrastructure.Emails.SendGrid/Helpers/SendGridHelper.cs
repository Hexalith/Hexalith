// <copyright file="SendGridHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Emails.SendGrid.Helpers;

using System.Diagnostics.CodeAnalysis;

using global::SendGrid.Extensions.DependencyInjection;

using Hexalith.Application.Emails;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Emails.Abstractions.Configurations;
using Hexalith.Infrastructure.Emails.Abstractions.Services;
using Hexalith.Infrastructure.Emails.SendGrid.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class SendGridHelper.
/// </summary>
public static class SendGridHelper
{
    /// <summary>
    /// Adds the send grid email.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddSendGridEmail([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        _ = services
            .ConfigureSettings<EmailServerSettings>(configuration)
            .AddSendGrid(o =>
            {
                EmailServerSettings emailServerSettings = new();
                configuration.GetSection(EmailServerSettings.ConfigurationName()).Bind(emailServerSettings);
                SettingsException<EmailServerSettings>.ThrowIfNullOrEmpty(emailServerSettings.ApplicationSecret);
                o.ApiKey = emailServerSettings.ApplicationSecret;
            });
        services
            .AddTransient<IEmailProvider, SendGridEmailService>()
            .TryAddTransient<IEmailService, EmailService>();
        return services;
    }
}