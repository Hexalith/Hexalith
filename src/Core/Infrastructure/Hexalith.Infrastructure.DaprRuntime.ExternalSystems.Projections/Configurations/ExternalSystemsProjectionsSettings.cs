// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Projections
// Author           : Jérôme Piquot
// Created          : 10-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-04-2023
// ***********************************************************************
// <copyright file="ExternalSystemsProjectionsSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Configurations;

using Hexalith.Domain.Aggregates;
using Hexalith.Extensions.Configuration;

/// <summary>
/// Class ExternalSystemsProjectionsSettings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
public class ExternalSystemsProjectionsSettings : ISettings
{
    /// <summary>
    /// Gets or sets the name of the aggregate external reference projection actor.
    /// </summary>
    /// <value>The name of the aggregate external reference projection actor.</value>
    public string? AggregateExternalReferenceProjectionActorName { get; set; }

    /// <summary>
    /// Gets or sets the name of the external reference projection actor.
    /// </summary>
    /// <value>The name of the external reference projection actor.</value>
    public string? ExternalReferenceProjectionActorName { get; set; }

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => nameof(ExternalSystemReference);
}