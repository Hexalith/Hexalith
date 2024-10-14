// <copyright file="OptionsHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

/// <summary>
/// Helper class to configure settings.
/// </summary>
public static class OptionsHelper
{
    /// <summary>
    /// Configure settings.
    /// </summary>
    /// <typeparam name="T">The type of the settings object.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration instance.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection ConfigureSettings<T>(this IServiceCollection services, IConfiguration configuration)
        where T : class, ISettings
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
        IConfigurationSection? section = configuration.GetSection(T.ConfigurationName())
                                         ?? throw new InvalidOperationException($"Could not load settings section '{T.ConfigurationName()}'");
        _ = services
            .AddOptions<T>()
            .Bind(section)
            .ValidateDataAnnotations();
        services.TryAddSingleton<IValidateOptions<T>>((s) =>
            new FluentValidateOptions<T>(
                T.ConfigurationName(),
                s));
        return services;
    }
}