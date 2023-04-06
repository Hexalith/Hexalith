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
    /// All as an asynchronous operation.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <param name="globalResourceGroupName">Name of the global resource group.</param>
    /// <param name="containerRegistryName">Name of the container registry.</param>
    /// <param name="containerRegistrySku">The container registry sku.</param>
    /// <param name="globalLocation">The global location.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task AllAsync(
        string? subscriptionId,
        string? globalResourceGroupName,
        string? containerRegistryName,
        string? containerRegistrySku,
        string? globalLocation)
    {
        await DeployGlobalResourcesAsync(subscriptionId, globalResourceGroupName, containerRegistryName, containerRegistrySku, globalLocation);
        await DevelopmentAsync();
        await StagingAsync();
        await ProductionAsync();
    }

    /// <summary>
    /// Deploy Development resources only.
    /// </summary>
    /// <returns>Task.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public Task DevelopmentAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Globals the asynchronous.
    /// </summary>
    /// <param name="subscriptionId">The subscription identifier.</param>
    /// <param name="globalResourceGroupName">Name of the global resource group.</param>
    /// <param name="containerRegistryName">Name of the container registry.</param>
    /// <param name="globalLocation">The global location.</param>
    /// <returns>Task.</returns>
    [Command("global")]
    private async Task DeployGlobalResourcesAsync(
        string? subscriptionId,
        string? globalResourceGroupName,
        string? containerRegistryName,
        string? containerRegistrySku,
        string? globalLocation)
    {
        subscriptionId = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            subscriptionId,
            _globalSettings.Value.SubscriptionId,
            GlobalSettings.ConfigurationName());
        globalResourceGroupName = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            globalResourceGroupName,
            _globalSettings.Value.ResourceGroupName,
            GlobalSettings.ConfigurationName());
        globalLocation = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            globalLocation,
            _globalSettings.Value.Location,
            GlobalSettings.ConfigurationName());
        containerRegistryName = ArgumentMissingException.ThrowIfNullOrWhiteSpace(
            containerRegistryName,
            _globalSettings.Value.ContainerRegistryName,
            GlobalSettings.ConfigurationName());
        await new Global(
            subscriptionId,
            globalResourceGroupName,
            containerRegistryName,
            containerRegistrySku ?? _globalSettings.Value.ContainerRegistrySku,
            globalLocation,
            _loggerFactory)
            .DeployAsync(CancellationToken.None);
    }

    /// <summary>
    /// Productions the asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    private Task ProductionAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Stagings the asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    private Task StagingAsync()
    {
        throw new NotImplementedException();
    }
}