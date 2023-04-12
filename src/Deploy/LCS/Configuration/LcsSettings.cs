// ***********************************************************************
// Assembly         : Deploy
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-11-2023
// ***********************************************************************
// <copyright file="LcsSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace DeployLCS.Configuration;

using Azure.ResourceManager.KeyVault.Models;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Class LcsSettings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
public class LcsSettings : ISettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LcsSettings" /> class.
    /// </summary>
    public LcsSettings()
    {
    }

    /// <summary>
    /// Gets or sets the dynamics staging instance.
    /// </summary>
    /// <value>The dynamics staging instance.</value>
    public string? DynamicsStagingInstance { get; set; }

    /// <summary>
    /// Gets or sets the dynamics support instance.
    /// </summary>
    /// <value>The dynamics support instance.</value>
    public string? DynamicsSupportInstance { get; set; }

    /// <summary>
    /// Gets or sets the name of the export database logic application.
    /// </summary>
    /// <value>The name of the export database logic application.</value>
    public string? ExportDatabaseLogicAppName { get; set; }

    /// <summary>
    /// Gets or sets the name of the key vault.
    /// </summary>
    /// <value>The name of the key vault.</value>
    public string? KeyVaultName { get; set; }

    /// <summary>
    /// Gets or sets the key vault sku.
    /// </summary>
    /// <value>The key vault sku.</value>
    public KeyVaultSkuName? KeyVaultSku { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    public string? LcsPassword { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <value>The name of the user.</value>
    public string? LcsUserName { get; set; }

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
    /// Gets or sets the scope.
    /// </summary>
    /// <value>The scope.</value>
    public string? Scope { get; set; }

    /// <summary>
    /// Gets or sets the subscription identifier.
    /// </summary>
    /// <value>The subscription identifier.</value>
    public string? SubscriptionId { get; set; }

    /// <summary>
    /// Gets or sets the tenant id.
    /// </summary>
    /// <value>The tenant.</value>
    public string? TenantId { get; set; }

    /// <summary>
    /// Gets or sets the name of the token logic application.
    /// </summary>
    /// <value>The name of the token logic application.</value>
    public string? TokenLogicAppName { get; set; }

    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName()
    {
        return "Lcs";
    }
}