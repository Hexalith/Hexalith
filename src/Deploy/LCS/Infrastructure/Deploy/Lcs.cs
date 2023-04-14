// ***********************************************************************
// Assembly         : Deploy
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-11-2023
// ***********************************************************************
// <copyright file="Lcs.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace DeployLCS.Infrastructure.Deploy;

using Azure.ResourceManager.KeyVault.Models;

using Hexalith.Infrastructure.AzureCloud.Builders;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class Global.
/// </summary>
internal class Lcs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Lcs"/> class.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <param name="resourceGroupName">Name of the resource group.</param>
    /// <param name="keyVaultName">Name of the key vault.</param>
    /// <param name="keyVaultSku">The key vault sku.</param>
    /// <param name="lcsUserName">Name of the LCS user.</param>
    /// <param name="lcsPassword">The LCS password.</param>
    /// <param name="tokenLogicAppName">Name of the token logic application.</param>
    /// <param name="location">The location.</param>
    /// <param name="tenantId">The tenant identifier.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    public Lcs(
        string subscriptionId,
        string resourceGroupName,
        string keyVaultName,
        KeyVaultSkuName? keyVaultSku,
        string lcsUserName,
        string lcsPassword,
        string tokenLogicAppName,
        string location,
        string? tenantId,
        ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentException.ThrowIfNullOrEmpty(subscriptionId);
        ArgumentException.ThrowIfNullOrEmpty(resourceGroupName);
        ArgumentException.ThrowIfNullOrEmpty(keyVaultName);
        ArgumentException.ThrowIfNullOrEmpty(lcsPassword);
        ArgumentException.ThrowIfNullOrEmpty(lcsUserName);
        ArgumentException.ThrowIfNullOrEmpty(tokenLogicAppName);
        ArgumentException.ThrowIfNullOrEmpty(location);
        SubscriptionId = subscriptionId;
        KeyVaultName = keyVaultName;
        KeyVaultSku = keyVaultSku;
        LcsUserName = lcsUserName;
        LcsPassword = lcsPassword;
        TokenLogicAppName = tokenLogicAppName;
        ResourceGroupName = resourceGroupName;
        Location = location;
        TenantId = tenantId;
        LoggerFactory = loggerFactory;
    }

    /// <summary>
    /// Gets the name of the key vault.
    /// </summary>
    /// <value>The name of the key vault.</value>
    public string KeyVaultName { get; }

    /// <summary>
    /// Gets the key vault sku.
    /// </summary>
    /// <value>The key vault sku.</value>
    public KeyVaultSkuName? KeyVaultSku { get; }

    /// <summary>
    /// Gets the LCS password.
    /// </summary>
    /// <value>The LCS password.</value>
    public string LcsPassword { get; }

    /// <summary>
    /// Gets the name of the LCS user.
    /// </summary>
    /// <value>The name of the LCS user.</value>
    public string LcsUserName { get; }

    /// <summary>
    /// Gets the location.
    /// </summary>
    /// <value>The location.</value>
    public string Location { get; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    public ILoggerFactory LoggerFactory { get; }

    /// <summary>
    /// Gets the name of the resource group.
    /// </summary>
    /// <value>The name of the resource group.</value>
    public string ResourceGroupName { get; }

    /// <summary>
    /// Gets the subscription identifier.
    /// </summary>
    /// <value>The subscription identifier.</value>
    public string SubscriptionId { get; }

    /// <summary>
    /// Gets the tenant identifier.
    /// </summary>
    /// <value>The tenant identifier.</value>
    public string? TenantId { get; }

    /// <summary>
    /// Gets the name of the token logic application.
    /// </summary>
    /// <value>The name of the token logic application.</value>
    public string TokenLogicAppName { get; }

    /// <summary>
    /// Deploys the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A <see cref="Task" /> representing the result of the asynchronous operation.</returns>
    public async Task DeployAsync(CancellationToken cancellationToken)
    {
        AzureBuilder azureBuilder = string.IsNullOrEmpty(TenantId)
            ? new(SubscriptionId, LoggerFactory)
            : new(SubscriptionId, TenantId, LoggerFactory);
        ResourceGroupBuilder resourceGroup = azureBuilder.AddResourceGroup(ResourceGroupName, Location);
        _ = resourceGroup
            .AddKeyVault(KeyVaultName, KeyVaultSku, Location)
            .AddSecret(nameof(LcsUserName), LcsUserName)
            .AddSecret(nameof(LcsPassword), LcsPassword);
        _ = resourceGroup.AddLogicWorkflow(TokenLogicAppName,, Location);
        await azureBuilder.BuildAsync(cancellationToken);
    }
}