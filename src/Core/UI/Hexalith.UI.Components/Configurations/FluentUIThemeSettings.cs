// <copyright file="FluentUIThemeSettings.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Configurations;

using System.Runtime.Serialization;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Represents the Fluent UI theme settings.
/// </summary>
[DataContract]
public class FluentUIThemeSettings : ISettings
{
    /// <summary>
    /// Gets or sets the accent base color.
    /// </summary>
    [DataMember]
    public string? AccentBaseColor { get; set; }

    /// <summary>
    /// Gets or sets the fill color.
    /// </summary>
    [DataMember]
    public string? FillColor { get; set; }

    /// <summary>
    /// Gets or sets the stroke width.
    /// </summary>
    [DataMember]
    public int StrokeWidth { get; set; } = 2;

    /// <summary>
    /// Gets the configuration name for Fluent UI theme.
    /// </summary>
    /// <returns>The configuration name.</returns>
    public static string ConfigurationName() => "FluentUITheme";
}