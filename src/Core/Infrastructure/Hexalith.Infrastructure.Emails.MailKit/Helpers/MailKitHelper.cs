// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Emails.MailKit
// Author           : Jérôme Piquot
// Created          : 01-14-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-14-2024
// ***********************************************************************
// <copyright file="MailKitHelper.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Emails.MailKit.Helpers;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.Emails;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Emails.Abstractions.Configurations;
using Hexalith.Infrastructure.Emails.Mailkit.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class MailKitHelper.
/// </summary>
public static class MailKitHelper
{
    /// <summary>
    /// Adds the send grid email.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddMailKitEmail([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        services
            .ConfigureSettings<EmailServerSettings>(configuration)
            .TryAddTransient<IEmailService, MailKitEmailService>();
        return services;
    }
}