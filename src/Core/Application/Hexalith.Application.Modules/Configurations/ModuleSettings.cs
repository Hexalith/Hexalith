// <copyright file="ModuleSettings.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Configurations;

using System.Runtime.Serialization;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Represents the settings for modules in the Hexalith application.
/// </summary>
/// <param name="DisabledModules">The list of disabled modules.</param>
/// <param name="HomePageModule">The module to be used as the home page.</param>
[DataContract]
public sealed record ModuleSettings(
        [property: DataMember(Order = 1)]
        IEnumerable<string>? DisabledModules,
        [property: DataMember(Order = 2)]
        string? HomePageModule = "Hexalith.Shared") : ISettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleSettings"/> class.
    /// </summary>
    public ModuleSettings()
        : this(DisabledModules: null)
    {
    }

    /// <summary>
    /// Gets the configuration name for Hexalith modules.
    /// </summary>
    /// <returns>The configuration name.</returns>
    public static string ConfigurationName() => "Hexalith:Modules";
}