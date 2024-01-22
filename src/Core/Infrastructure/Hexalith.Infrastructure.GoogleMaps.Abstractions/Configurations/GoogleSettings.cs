// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.GoogleMaps.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-19-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-19-2024
// ***********************************************************************
// <copyright file="GoogleSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.GoogleMaps.Abstractions.Configurations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Class GoogleSettings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
public class GoogleSettings : ISettings
{
    /// <summary>
    /// Gets or sets the API key.
    /// </summary>
    /// <value>The API key.</value>
    public string? ApiKey { get; set; }

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => "Google";
}