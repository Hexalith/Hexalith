﻿// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Bus
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="RequestBusSettings.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Buses;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Class RequestBusSettings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
public class RequestBusSettings : ISettings
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; init; } = ApplicationConstants.RequestBus;

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => nameof(ApplicationConstants.RequestBus);
}