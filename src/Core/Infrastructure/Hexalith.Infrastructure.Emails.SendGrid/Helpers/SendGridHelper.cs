// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Emails.SendGrid
// Author           : Jérôme Piquot
// Created          : 01-14-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-14-2024
// ***********************************************************************
// <copyright file="SendGridHelper.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Emails.SendGrid.Helpers;

using System.Diagnostics.CodeAnalysis;

using global::SendGrid.Extensions.DependencyInjection;

using Hexalith.Application.Emails;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Emails.Abstractions.Configurations;
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
        services.TryAddTransient<IEmailService, SendGridEmailService>();
        return services;
    }
}