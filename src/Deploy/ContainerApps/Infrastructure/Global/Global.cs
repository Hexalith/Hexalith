// ***********************************************************************
// Assembly         : Deploy
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-03-2023
// ***********************************************************************
// <copyright file="Global.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace DeployACA.Infrastructure.Global;

using Azure.ResourceManager.KeyVault.Models;

using Hexalith.Infrastructure.AzureCloud.Builders;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class Global.
/// </summary>
internal class Global
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Global"/> class.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <param name="resourceGroupName">Name of the resource group.</param>
    /// <param name="containerRegistryName">Name of the container registry.</param>
    /// <param name="containerRegistrySku">The container registry sku.</param>
    /// <param name="cognitiveServicesAccountName">Name of the cognitive services account.</param>
    /// <param name="location">The location.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    /// <exception cref="ArgumentNullException">Null.</exception>
    public Global(
        string subscriptionId,
        string resourceGroupName,
        string containerRegistryName,
        string? containerRegistrySku,
        string cognitiveServicesAccountName,
        string keyVaultName,
        KeyVaultSkuName? keyVaultSku,
        string location,
        ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentException.ThrowIfNullOrEmpty(subscriptionId);
        ArgumentException.ThrowIfNullOrEmpty(containerRegistryName);
        ArgumentException.ThrowIfNullOrEmpty(resourceGroupName);
        ArgumentException.ThrowIfNullOrEmpty(cognitiveServicesAccountName);
        ArgumentException.ThrowIfNullOrEmpty(location);
        SubscriptionId = subscriptionId;
        ContainerRegistryName = containerRegistryName;
        ContainerRegistrySku = containerRegistrySku;
        CognitiveServicesAccountName = cognitiveServicesAccountName;
        KeyVaultName = keyVaultName;
        KeyVaultSku = keyVaultSku;
        ResourceGroupName = resourceGroupName;
        Location = location;
        LoggerFactory = loggerFactory;
    }

    /// <summary>
    /// Gets the name of the cognitive services account.
    /// </summary>
    /// <value>The name of the cognitive services account.</value>
    public string CognitiveServicesAccountName { get; }

    /// <summary>
    /// Gets the name of the container registry.
    /// </summary>
    /// <value>The name of the container registry.</value>
    public string ContainerRegistryName { get; }

    /// <summary>
    /// Gets the container registry sku.
    /// </summary>
    /// <value>The container registry sku.</value>
    public string? ContainerRegistrySku { get; }

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
    /// Deploys the asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A <see cref="Task" /> representing the result of the asynchronous operation.</returns>
    public async Task DeployAsync(CancellationToken cancellationToken)
    {
        AzureBuilder azureBuilder = new(SubscriptionId, LoggerFactory);
        ResourceGroupBuilder resourceGroup = azureBuilder.AddResourceGroup(ResourceGroupName, Location);
        _ = resourceGroup.AddContainerRegistry(ContainerRegistryName, ContainerRegistrySku, Location);
        _ = resourceGroup.AddOpenAIAccount(CognitiveServicesAccountName, "eastus"); // Waiting for general availability
        _ = resourceGroup.AddKeyVault(KeyVaultName, KeyVaultSku, Location);
        await azureBuilder.BuildAsync(cancellationToken);
    }
}