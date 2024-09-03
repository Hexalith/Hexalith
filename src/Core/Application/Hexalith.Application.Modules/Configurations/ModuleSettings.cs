// <copyright file="ModuleSettings.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Configurations;

using System.Runtime.Serialization;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Hexalith modules settings.
/// </summary>
[DataContract]
public sealed record ModuleSettings(
        /// <summary>
        /// Gets the list of disabled modules.
        /// </summary>
        [property: DataMember(Order = 1)]
        IEnumerable<string>? DisabledModules,
        /// <summary>
        /// Gets the name of the module containing the home page.
        /// </summary>
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