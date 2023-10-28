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

using DeployLCS.Configuration;
using DeployLCS.Infrastructure.Deploy;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Class Deployment Commands.
/// </summary>
internal class DeploymentCommand
{
    /// <summary>
    /// The Lcs settings.
    /// </summary>
    private readonly IOptions<LcsSettings> _lcsSettings;

    private readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeploymentCommand"/> class.
    /// </summary>
    /// <param name="lcsSettings">The Lcs settings.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    public DeploymentCommand(IOptions<LcsSettings> lcsSettings, ILoggerFactory loggerFactory)
    {
        _lcsSettings = lcsSettings;
        _loggerFactory = loggerFactory;
    }

    /// <summary>
    /// Deploy LCS resources as an asynchronous operation.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <param name="resourceGroupName">Name of the resource group.</param>
    /// <param name="lcsUserName">Name of the LCS user.</param>
    /// <param name="lcsPassword">The LCS password.</param>
    /// <param name="tokenLogicAppName">Name of the token logic application.</param>
    /// <param name="keyVaultName">Name of the key vault.</param>
    /// <param name="keyVaultSku">The key vault sku.</param>
    /// <param name="tenantId">The tenant identifier.</param>
    /// <param name="location">The location.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    [Command("all")]
    public async Task DeployLcsResourcesAsync(
        string? subscriptionId,
        string? resourceGroupName,
        string? lcsUserName,
        string? lcsPassword,
        string? tokenLogicAppName,
        string? keyVaultName,
        KeyVaultSkuName? keyVaultSku,
        string? tenantId,
        string? location)
    {
        subscriptionId = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            subscriptionId,
            _lcsSettings.Value.SubscriptionId,
            LcsSettings.ConfigurationName());
        resourceGroupName = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            resourceGroupName,
            _lcsSettings.Value.ResourceGroupName,
            LcsSettings.ConfigurationName());
        keyVaultName = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            keyVaultName,
            _lcsSettings.Value.KeyVaultName,
            LcsSettings.ConfigurationName());
        keyVaultSku ??= _lcsSettings.Value.KeyVaultSku;
        location = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            location,
            _lcsSettings.Value.Location,
            LcsSettings.ConfigurationName());
        lcsUserName = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            lcsUserName,
            _lcsSettings.Value.LcsUserName,
            LcsSettings.ConfigurationName());
        lcsPassword = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            lcsPassword,
            _lcsSettings.Value.LcsPassword,
            LcsSettings.ConfigurationName());
        tokenLogicAppName = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            tokenLogicAppName,
            _lcsSettings.Value.TokenLogicAppName,
            LcsSettings.ConfigurationName());
        tenantId = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            tenantId,
            _lcsSettings.Value.TenantId,
            LcsSettings.ConfigurationName());
        await new Lcs(
            subscriptionId,
            resourceGroupName,
            keyVaultName,
            keyVaultSku,
            lcsUserName,
            lcsPassword,
            tokenLogicAppName,
            location,
            tenantId,
            _loggerFactory)
            .DeployAsync(CancellationToken.None).ConfigureAwait(false);
    }
}