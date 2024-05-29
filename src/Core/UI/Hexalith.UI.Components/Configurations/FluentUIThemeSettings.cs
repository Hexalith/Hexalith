// <copyright file="FluentUIThemeSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Configurations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Represents the Fluent UI theme settings.
/// </summary>
public record FluentUIThemeSettings : ISettings
{
    /// <summary>
    /// Gets the configuration name for Fluent UI theme.
    /// </summary>
    /// <returns>The configuration name.</returns>
    public static string ConfigurationName() => "Hexalith:FluentUITheme";
}