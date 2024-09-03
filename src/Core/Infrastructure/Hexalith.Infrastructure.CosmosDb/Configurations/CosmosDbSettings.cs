// <copyright file="CosmosDbSettings.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.CosmosDb.Configurations;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Class CosmosDb Settings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
public class CosmosDbSettings : ISettings
{
    /// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    /// <value>The connection string.</value>
    public string? ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the container name.
    /// </summary>
    /// <value>The container name.</value>
    public string? ContainerName { get; set; }

    /// <summary>
    /// Gets or sets the database name.
    /// </summary>
    /// <value>The database name.</value>
    public string? DatabaseName { get; set; }

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => nameof(Microsoft.Azure.Cosmos);
}