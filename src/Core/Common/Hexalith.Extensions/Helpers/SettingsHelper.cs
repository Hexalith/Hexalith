// <copyright file="SettingsHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Extensions.Configuration;

using Microsoft.Extensions.Configuration;

/// <summary>
/// Settings helper class.
/// </summary>
public static class SettingsHelper
{
    /// <summary>
    /// Gets the settings from the configuration.
    /// </summary>
    /// <typeparam name="TSettings">The type of the settings.</typeparam>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The settings instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the configuration is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the configuration section is not found.</exception>
    public static TSettings GetSettings<TSettings>([NotNull] this IConfiguration configuration)
        where TSettings : class, ISettings, new()
    {
        ArgumentNullException.ThrowIfNull(configuration);
        TSettings settings = new();
        string configurationName = TSettings.ConfigurationName();
        IConfigurationSection section = configuration.GetSection(configurationName)
            ?? throw new InvalidOperationException($"Configuration section '{configurationName}' not found.");
        section.Bind(settings);
        return settings;
    }
}