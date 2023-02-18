// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprBus
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="CommandBusSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Configuration;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Class CommandBusSettings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
public class CommandBusSettings : ISettings
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; init; } = "command-bus";

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName()
    {
        return "CommandBus";
    }
}