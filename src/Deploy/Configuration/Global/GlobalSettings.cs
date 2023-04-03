// ***********************************************************************
// Assembly         : Deploy
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-02-2023
// ***********************************************************************
// <copyright file="GlobalSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Deploy.Configuration.Global;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Class GlobalSettings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
public class GlobalSettings : ISettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalSettings"/> class.
    /// </summary>
    public GlobalSettings()
    {
    }

    /// <summary>
    /// Gets or sets the name of the container registry.
    /// </summary>
    /// <value>The name of the container registry.</value>
    public string? ContainerRegistryName { get; set; }

    /// <summary>
    /// Gets or sets the container registry sku.
    /// </summary>
    /// <value>The container registry sku.</value>
    public string? ContainerRegistrySku { get; set; }

    /// <summary>
    /// Gets or sets the location.
    /// </summary>
    /// <value>The location.</value>
    public string? Location { get; set; }

    /// <summary>
    /// Gets or sets the name of the resource group.
    /// </summary>
    /// <value>The name of the resource group.</value>
    public string? ResourceGroupName { get; set; }

    /// <summary>
    /// Gets or sets the subscription identifier.
    /// </summary>
    /// <value>The subscription identifier.</value>
    public string? SubscriptionId { get; set; }

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName()
    {
        return "Global";
    }
}