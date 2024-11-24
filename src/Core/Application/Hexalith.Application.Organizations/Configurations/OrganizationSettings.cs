// <copyright file="OrganizationSettings.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Organizations.Configurations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Class OrganizationSettings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
public class OrganizationSettings : ISettings
{
    /// <summary>
    /// Gets or sets the default company identifier.
    /// </summary>
    /// <value>The default company identifier.</value>
    public string? DefaultCompanyId { get; set; }

    /// <summary>
    /// Gets or sets the default origin identifier.
    /// </summary>
    /// <value>The default origin identifier.</value>
    public string? DefaultOriginId { get; set; }

    /// <summary>
    /// Gets or sets the default partition identifier.
    /// </summary>
    /// <value>The default partition identifier.</value>
    public string? DefaultPartitionId { get; set; }

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => "Organization";
}