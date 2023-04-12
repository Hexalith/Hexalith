// ***********************************************************************
// Assembly         : Deploy
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-03-2023
// ***********************************************************************
// <copyright file="DeploymentCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Azure.ResourceManager.KeyVault.Models;

using Cocona;

using Deploy.Configuration.Global;
using Deploy.Infrastructure.Global;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class Deployment Commands.
/// </summary>
internal class DeploymentCommand
{
    /// <summary>
    /// The global settings.
    /// </summary>
    private readonly IOptions<GlobalSettings> _globalSettings;

    private readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeploymentCommand"/> class.
    /// </summary>
    /// <param name="globalSettings">The global settings.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    public DeploymentCommand(IOptions<GlobalSettings> globalSettings, ILoggerFactory loggerFactory)
    {
        _globalSettings = globalSettings;
        _loggerFactory = loggerFactory;
    }

    /// <summary>
    /// Deploy global resources as an asynchronous operation.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <param name="resourceGroupName">Name of the resource group.</param>
    /// <param name="containerRegistryName">Name of the container registry.</param>
    /// <param name="containerRegistrySku">The container registry sku.</param>
    /// <param name="cognitiveServicesName">Name of the cognitive services.</param>
    /// <param name="keyVaultName">Name of the key vault.</param>
    /// <param name="keyVaultSku">The key vault sku.</param>
    /// <param name="location">The location.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    [Command("global")]
    public async Task DeployGlobalResourcesAsync(
        string? subscriptionId,
        string? resourceGroupName,
        string? containerRegistryName,
        string? containerRegistrySku,
        string? cognitiveServicesName,
        string? keyVaultName,
        KeyVaultSkuName? keyVaultSku,
        string? location)
    {
        subscriptionId = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            subscriptionId,
            _globalSettings.Value.SubscriptionId,
            GlobalSettings.ConfigurationName());
        resourceGroupName = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            resourceGroupName,
            _globalSettings.Value.ResourceGroupName,
            GlobalSettings.ConfigurationName());
        keyVaultName = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            keyVaultName,
            _globalSettings.Value.KeyVaultName,
            GlobalSettings.ConfigurationName());
        keyVaultSku ??= _globalSettings.Value.KeyVaultSku;
        location = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            location,
            _globalSettings.Value.Location,
            GlobalSettings.ConfigurationName());
        containerRegistryName = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            containerRegistryName,
            _globalSettings.Value.ContainerRegistryName,
            GlobalSettings.ConfigurationName());
        cognitiveServicesName = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            cognitiveServicesName,
            _globalSettings.Value.CognitiveServicesAccountName,
            GlobalSettings.ConfigurationName());
        await new Global(
            subscriptionId,
            resourceGroupName,
            containerRegistryName,
            containerRegistrySku ?? _globalSettings.Value.ContainerRegistrySku,
            cognitiveServicesName,
            keyVaultName,
            keyVaultSku,
            location,
            _loggerFactory)
            .DeployAsync(CancellationToken.None);
    }
}