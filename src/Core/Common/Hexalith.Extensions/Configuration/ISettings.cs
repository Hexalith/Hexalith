// <copyright file="ISettings.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Configuration;

/// <summary>
/// Interface for settings.
/// </summary>
public interface ISettings
{
    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    static abstract string ConfigurationName();
}