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
    /// <returns>The settings object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the configuration is null.</exception>
    public static TSettings GetSettings<TSettings>([NotNull] this IConfiguration configuration)
        where TSettings : ISettings, new()
    {
        ArgumentNullException.ThrowIfNull(configuration);
        return configuration.GetSection(TSettings.ConfigurationName()).Get<TSettings>() ?? new TSettings();
    }
}